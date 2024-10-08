using System.Reflection;
using Dntc.Common.Conversion;
using Dntc.Common.Syntax;
using Dntc.Common.Syntax.Expressions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class LoadHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Ldfld, new LdFldHandler(false) },
        { Code.Ldflda, new LdFldHandler(true) },

        { Code.Ldind_I, new LdIndHandler() },
        { Code.Ldind_I1, new LdIndHandler() },
        { Code.Ldind_I2, new LdIndHandler() },
        { Code.Ldind_I4, new LdIndHandler() },
        { Code.Ldind_I8, new LdIndHandler() },
        { Code.Ldind_R4, new LdIndHandler() },
        { Code.Ldind_R8, new LdIndHandler() },
        { Code.Ldind_U1, new LdIndHandler() },
        { Code.Ldind_U2, new LdIndHandler() },
        { Code.Ldind_U4, new LdIndHandler() },

        { Code.Ldarg, new LdArgHandler(null, false) },
        { Code.Ldarg_S, new LdArgHandler(null, false) },
        { Code.Ldarg_0, new LdArgHandler(0, false) },
        { Code.Ldarg_1, new LdArgHandler(1, false) },
        { Code.Ldarg_2, new LdArgHandler(2, false) },
        { Code.Ldarg_3, new LdArgHandler(3, false) },
        { Code.Ldarga, new LdArgHandler(null, true) },
        { Code.Ldarga_S, new LdArgHandler(null, true) },

        { Code.Ldc_I4, new LdCHandler(null) },
        { Code.Ldc_I4_S, new LdCHandler(null) },
        { Code.Ldc_I4_0, new LdCHandler(0) },
        { Code.Ldc_I4_1, new LdCHandler(1) },
        { Code.Ldc_I4_2, new LdCHandler(2) },
        { Code.Ldc_I4_3, new LdCHandler(3) },
        { Code.Ldc_I4_4, new LdCHandler(4) },
        { Code.Ldc_I4_5, new LdCHandler(5) },
        { Code.Ldc_I4_6, new LdCHandler(6) },
        { Code.Ldc_I4_7, new LdCHandler(7) },
        { Code.Ldc_I4_8, new LdCHandler(8) },
        { Code.Ldc_I4_M1, new LdCHandler(-1) },

        { Code.Ldc_R4, new LdCHandler(null) },
        { Code.Ldc_R8, new LdCHandler(null) },
        
        { Code.Ldloc, new LdLocHandler(null, false) },
        { Code.Ldloc_S, new LdLocHandler(null, false) },
        { Code.Ldloc_0, new LdLocHandler(0, false) },
        { Code.Ldloc_1, new LdLocHandler(1, false) },
        { Code.Ldloc_2, new LdLocHandler(2, false) },
        { Code.Ldloc_3, new LdLocHandler(3, false) },
        { Code.Ldloca, new LdLocHandler(null, true) },
        { Code.Ldloca_S, new LdLocHandler(null, true) },
        
        { Code.Ldobj, new LdObjHandler() },
    };

    private class LdFldHandler(bool getAddress) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction,
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod,
            ConversionCatalog conversionCatalog)
        {
            var field = (FieldDefinition)currentInstruction.Operand;
            var items = expressionStack.Pop(1);
            var objectExpression = items[0];

            CBaseExpression newExpression = new FieldAccessExpression(
                objectExpression,
                new UntypedVariable(field.Name, field.FieldType.IsByReference));

            if (getAddress)
            {
                newExpression = new AddressOfValueExpression(newExpression);
            }

            expressionStack.Push(newExpression);
            return new OpCodeHandlingResult(null);
        }
    }

    private class LdIndHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction,
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod,
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(1);
            if (!items[0].ProducesAPointer)
            {
                var message = "Expected top most expression to produce a pointer, but it does not";
                throw new InvalidOperationException(message);
            }

            var dereferencedExpression = new DereferencedValueExpression(items[0]);
            expressionStack.Push(dereferencedExpression);

            return new OpCodeHandlingResult(null);
        }
    }

    private class LdArgHandler(int? argIndex, bool loadAddress) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction,
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod,
            ConversionCatalog conversionCatalog)
        {
            var index = 0;
            if (argIndex == null)
            {
                index = currentInstruction.Operand switch
                {
                    int intIndex => intIndex,
                    ParameterDefinition paramDef => paramDef.Index,
                    _ => throw new ArgumentException(
                        $"Unknown ldarg operand type of {currentInstruction.Operand.GetType().FullName}"),
                };
            }
            else
            {
                index = argIndex.Value;
            }

            if (currentMethod.Parameters.Count <= index)
            {
                var message = $"Argument index #{index} referenced but method only " +
                              $"has ${currentMethod.Parameters.Count} parameters";
                throw new InvalidOperationException(message);
            }

            var parameter = currentMethod.Parameters[index];
            var variable = new Variable(parameter.ConversionInfo, parameter.Name, parameter.IsReference);
            CBaseExpression newExpression = new VariableValueExpression(variable);
            if (loadAddress)
            {
                newExpression = new AddressOfValueExpression(newExpression);
            }

            expressionStack.Push(newExpression);
            return new OpCodeHandlingResult(null);
        }
    }

    private class LdCHandler(int? hardCodedNumber) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction,
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod,
            ConversionCatalog conversionCatalog)
        {
            string numericLiteral;
            if (hardCodedNumber != null)
            {
                numericLiteral = hardCodedNumber.Value.ToString();
            }
            else
            {
                switch (currentInstruction.Operand)
                {
                    case sbyte sbyteValue:
                        numericLiteral = sbyteValue.ToString();
                        break;

                    case byte byteValue:
                        numericLiteral = byteValue.ToString();
                        break;

                    case short shortValue:
                        numericLiteral = shortValue.ToString();
                        break;

                    case ushort ushortValue:
                        numericLiteral = ushortValue.ToString();
                        break;

                    case int intValue:
                        numericLiteral = intValue.ToString();
                        break;

                    case float floatValue:
                        numericLiteral = floatValue.ToString();
                        break;

                    case double doubleValue:
                        numericLiteral = doubleValue.ToString();
                        break;

                    default:
                        throw new NotSupportedException(currentInstruction.Operand.GetType().FullName);
                }
            }

            var expression = new LiteralValueExpression(numericLiteral);
            expressionStack.Push(expression);

            return new OpCodeHandlingResult(null);
        }
    }

    private class LdLocHandler(int? index, bool getAddress) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction,
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod,
            ConversionCatalog conversionCatalog)
        {
            var localIndex = 0;
            if (index != null)
            {
                localIndex = index.Value;
            }
            else
            {
                var variable = (VariableDefinition)currentInstruction.Operand;
                localIndex = variable.Index;
            }

            if (currentMethod.Locals.Count <= localIndex)
            {
                var message = $"Requested to load local #{localIndex} but only " +
                              $"{currentMethod.Locals.Count} are defined";
                throw new InvalidOperationException(message);
            }

            var local = currentMethod.Locals[localIndex];
            var expression = new VariableValueExpression(
                new Variable(
                    local.ConversionInfo,
                    Utils.LocalName(localIndex),
                    local.IsReference));

            CBaseExpression newExpression = getAddress
                ? new AddressOfValueExpression(expression)
                : new DereferencedValueExpression(expression);

            expressionStack.Push(newExpression);
            
            return new OpCodeHandlingResult(null);
        }
    }
    
    private class LdObjHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(1);
            var objectAddress = items[0];
            
            expressionStack.Push(new DereferencedValueExpression(objectAddress));
            
            return new OpCodeHandlingResult(null);
        }
    }
}