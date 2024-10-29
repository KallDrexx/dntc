﻿using Dntc.Common.Conversion;
using Dntc.Common.Syntax;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class StoreHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Stfld, new StFldHandler() },
        { Code.Stobj, new StObjHandler() },
        { Code.Starg, new StArgHandler() },
        { Code.Starg_S, new StArgHandler() },
        
        { Code.Stind_I, new StIndHandler() },
        { Code.Stind_I2, new StIndHandler() },
        { Code.Stind_I4, new StIndHandler() },
        { Code.Stind_I8, new StIndHandler() },
        { Code.Stind_R4, new StIndHandler() },
        { Code.Stind_R8, new StIndHandler() },
        
        { Code.Stloc, new StLocHandler(null) },
        { Code.Stloc_0, new StLocHandler(0) },
        { Code.Stloc_1, new StLocHandler(1) },
        { Code.Stloc_2, new StLocHandler(2) },
        { Code.Stloc_3, new StLocHandler(3) },
        { Code.Stloc_S, new StLocHandler(null) },
    };

    /// <summary>
    /// Searches if we are about to store to a variable whose value is already referenced in the
    /// expression stack. If that's the case, we need to save the value to a temporary variable, and
    /// replace the current variable with the temporary one for proper behavior.
    /// </summary>
    /// <returns>Statement sets for temp variable replacement only if one is required</returns>
    private static CStatementSet? HandleReferencedVariable(
        ExpressionStack expressionStack,
        VariableValueExpression variable,
        int currentIlOffset)
    {
        var name = $"__temp_{currentIlOffset:x4}";
        var tempVariable = variable.Variable with { Name = name };
        var tempVariableExpression = new VariableValueExpression(tempVariable);
        
        if (expressionStack.ReplaceExpression(variable, tempVariableExpression))
        {
            // At least one expression in the stack was replaced
            var localDeclaration = new LocalDeclarationStatementSet(tempVariable);
            var assignment = new AssignmentStatementSet(tempVariableExpression, variable);
            return new CompoundStatementSet([localDeclaration, assignment]);
        }

        return null;
    }
        
    private class StFldHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var field = (FieldDefinition)context.CurrentInstruction.Operand;
            var items = context.ExpressionStack.Pop(2);
            var value = items[0];
            var obj = items[1];

            var left = new FieldAccessExpression(obj, new Variable(value.ResultingType, field.Name, field.FieldType.IsPointer));
            var right = new DereferencedValueExpression(value);
            var statement = new AssignmentStatementSet(left, right);

            return new OpCodeHandlingResult(statement);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
    
    private class StIndHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(2);
            var value = items[0];
            var address = items[1];

            var left = new DereferencedValueExpression(address);
            var right = new DereferencedValueExpression(value);
            var statement = new AssignmentStatementSet(left, right);

            return new OpCodeHandlingResult(statement);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
    
    private class StObjHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(2);
            var objectValue = items[0];
            var address = items[1];

            var left = new DereferencedValueExpression(address);
            var right = new DereferencedValueExpression(objectValue);
            var statement = new AssignmentStatementSet(left, right);

            return new OpCodeHandlingResult(statement);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
    
    private class StArgHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(1);
            var value = items[0];

            var argIndex = context.CurrentInstruction.Operand switch
            {
                int intOperand => intOperand,
                ParameterDefinition parameterDefinition => parameterDefinition.Index,
                _ => throw new NotSupportedException(context.CurrentInstruction.Operand.GetType().FullName),
            };

            if (context.CurrentMethodConversion.Parameters.Count < argIndex)
            {
                var message = $"starg on argument index {argIndex} is beyond the argument " +
                              $"count of {context.CurrentMethodConversion.Parameters.Count}";
                throw new InvalidOperationException(message);
            }

            var argument = context.CurrentMethodConversion.Parameters[argIndex];
            var storedVariableExpression = new VariableValueExpression(
                new Variable(argument.ConversionInfo, argument.Name, argument.IsReference));
            
            var left = new DereferencedValueExpression(storedVariableExpression);
            var right = new DereferencedValueExpression(value);
            var statement = new AssignmentStatementSet(left, right);

            var tempVariable = HandleReferencedVariable(
                context.ExpressionStack, 
                storedVariableExpression, 
                context.CurrentInstruction.Offset);

            CStatementSet result = tempVariable != null
                ? new CompoundStatementSet([tempVariable, statement])
                : statement;

            return new OpCodeHandlingResult(result);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
    
    private class StLocHandler(int? index) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(1);

            int localIndex;
            if (index != null)
            {
                localIndex = index.Value;
            }
            else
            {
                localIndex = context.CurrentInstruction.Operand switch
                {
                    int intIndex => intIndex,
                    VariableDefinition variableDefinition => variableDefinition.Index,
                    _ => throw new NotSupportedException(context.CurrentInstruction.Operand.GetType().FullName),
                };
            }

            if (context.CurrentMethodConversion.Locals.Count <= localIndex)
            {
                var message = $"stloc for local index {localIndex} but method only has " +
                              $"{context.CurrentMethodConversion.Locals.Count} locals";
                throw new InvalidOperationException(message);
            }

            var local = context.CurrentMethodConversion.Locals[localIndex];
            var localVariable = new VariableValueExpression(
                new Variable(local.ConversionInfo, Utils.LocalName(localIndex), local.IsReference));

            var left = new DereferencedValueExpression(localVariable);
            var right = new DereferencedValueExpression(items[0]);

            var tempStatement = HandleReferencedVariable(context.ExpressionStack, localVariable, context.CurrentInstruction.Offset);
            var statement = new AssignmentStatementSet(left, right);

            CStatementSet result = tempStatement != null
                ? new CompoundStatementSet([tempStatement, statement])
                : statement;

            return new OpCodeHandlingResult(result);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
}