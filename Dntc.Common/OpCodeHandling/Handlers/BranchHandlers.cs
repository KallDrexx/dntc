using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class BranchHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Br, new SimpleBranchHandler() },
        { Code.Br_S, new SimpleBranchHandler() },
        { Code.Brfalse, new BranchOnBool(false) },
        { Code.Brfalse_S, new BranchOnBool(false) },
        { Code.Brtrue, new BranchOnBool(true) },
        { Code.Brtrue_S, new BranchOnBool(true) },
        { Code.Beq, new BranchComparison("==") },
        { Code.Beq_S, new BranchComparison("==") },
        { Code.Ble, new BranchComparison("<=") },
        { Code.Ble_S, new BranchComparison("<=") },
        { Code.Ble_Un, new BranchComparison("<=") },
        { Code.Ble_Un_S, new BranchComparison("<=") },
        { Code.Blt, new BranchComparison("<") },
        { Code.Blt_S, new BranchComparison("<") },
        { Code.Blt_Un, new BranchComparison("<") },
        { Code.Blt_Un_S, new BranchComparison("<") },
        { Code.Bge, new BranchComparison(">=") },
        { Code.Bge_S, new BranchComparison(">=") },
        { Code.Bge_Un, new BranchComparison(">=") },
        { Code.Bge_Un_S, new BranchComparison(">=") },
        { Code.Bgt, new BranchComparison(">") },
        { Code.Bgt_S, new BranchComparison(">") },
        { Code.Bgt_Un, new BranchComparison(">") },
        { Code.Bgt_Un_S, new BranchComparison(">") },
        { Code.Bne_Un, new BranchComparison("!=") },
        { Code.Bne_Un_S, new BranchComparison("!=") },
        
        { Code.Switch, new SwitchHandler() },
    };
    
    private class SimpleBranchHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var target = (Instruction)context.CurrentInstruction.Operand;
           
            // When you write a ternary statement in C#, the IL that gets generated ends up having 4 sections
            // to it:
            // * Section 1: condition statement which if true branches to section 3
            // * Section 2: false expression bytecode, then branch to section 4
            // * Section 3: true expression bytecode
            // * Section 4: always expression bytecode
            //
            // Sections 2 and 3 both almost always put a value in the evaluation stack, and only one of the two 
            // sections will ever be entered based on the evaluation of the condition. This works fine when using
            // a runtime interpreter because it's maintaining an actual stack of values as it iterates the 
            // MSIL. However, when transpiling we are storing expressions until we can form a full statement, and
            // only section 4 generates an actual statement. However, at compile time we can't know if section 2
            // or section 3 will generate the expressions used for the statement because we can't statically 
            // guarantee the values into the condition. This causes the transpiler to keep an extra expression on the
            // stack with section 4 popping the wrong one for statement generation. This also means most downstream
            // expression stack pops are incorrect.
            //
            // To fix this, we can rely on the fact that section 2's branch is a simple/non-conditional branch. Since
            // when this branch occurs we have an expression on the stack, we are fairly confident that instructions
            // we are skipping probably are trying to push a possible replacement expression on the stack as well. So
            // we need to instruct the transpiler to create a checkpoint of the current expression stack values, so
            // that the correct expression is utilized when it transpiles the operation in section 4.
            var checkpointTarget = context.ExpressionStack.Count > 0 ? (int?)target.Offset : null;

            return new OpCodeHandlingResult(
                new GotoStatementSet(target.Offset, context.CurrentMethodConversion),
                checkpointTarget);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
    
    private class BranchOnBool(bool isTrueCheck) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(1);
            var item = items[0];
            var target = (Instruction)context.CurrentInstruction.Operand;
            
            CBaseExpression condition;
            if (item.ResultingType.IsReferenceType && item.PointerDepth > 0)
            {
                // For reference types, preserve the pointer depth to check the pointer itself (null check)
                condition = item;
            }
            else
            {
                // For value types and primitives, dereference to get the value for boolean evaluation
                condition = new AdjustPointerDepthExpression(item, 0);
            }
            
            if (!isTrueCheck)
            {
                condition = new NegateExpression(condition, true);
            }

            return new OpCodeHandlingResult(
                new IfConditionJumpStatementSet(condition, target.Offset, context.CurrentMethodConversion));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
    
    private class BranchComparison(string comparison) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(2);
            var value2 = items[0];
            var value1 = items[1];
            var target = (Instruction)context.CurrentInstruction.Operand;
            var boolType = context.ConversionCatalog.Find(new IlTypeName(typeof(bool).FullName!));
            var condition = new TwoExpressionEvalExpression(value1, comparison, value2, boolType);

            return new OpCodeHandlingResult(
                new IfConditionJumpStatementSet(condition, target.Offset, context.CurrentMethodConversion));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult
            {
                ReferencedTypes = new HashSet<IlTypeName>([new IlTypeName(typeof(bool).FullName!)]),
            };
        }
    }
    
    private class SwitchHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(1);
            var targets = (Instruction[])context.CurrentInstruction.Operand;
            var offsets = targets.Select(x => x.Offset).ToArray();

            var statement = new JumpTableStatementSet(items[0], offsets, context.CurrentMethodConversion);
            return new OpCodeHandlingResult(statement);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
}