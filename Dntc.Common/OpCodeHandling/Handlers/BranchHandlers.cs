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
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            if (expressionStack.Count > 0)
            {
                throw new NotImplementedException("Branch encountered while expression in stack");
            }
            
            var target = (Instruction)currentInstruction.Operand;
            return new OpCodeHandlingResult(new GotoStatementSet(target.Offset));
        }
    }
    
    private class BranchOnBool(bool isTrueCheck) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(1);
            var item = items[0];
            var target = (Instruction)currentInstruction.Operand;
            CBaseExpression condition = new DereferencedValueExpression(item);
            if (!isTrueCheck)
            {
                condition = new NotExpression(condition);
            }

            return new OpCodeHandlingResult(new IfConditionJumpStatementSet(condition, target.Offset));
        }
    }
    
    private class BranchComparison(string comparison) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(2);
            var value2 = new DereferencedValueExpression(items[0]);
            var value1 = new DereferencedValueExpression(items[1]);
            var target = (Instruction)currentInstruction.Operand;
            var boolType = conversionCatalog.Find(new IlTypeName(typeof(bool).FullName!));
            var condition = new TwoExpressionEvalExpression(value1, comparison, value2, boolType);

            return new OpCodeHandlingResult(new IfConditionJumpStatementSet(condition, target.Offset));
        }
    }
    
    private class SwitchHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(1);
            var targets = (Instruction[])currentInstruction.Operand;
            var offsets = targets.Select(x => x.Offset).ToArray();

            var statement = new JumpTableStatementSet(items[0], offsets);
            return new OpCodeHandlingResult(statement);
        }
    }
}