using System.Globalization;
using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
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
        { Code.Ldsfld, new LdFldHandler(false) },
        { Code.Ldsflda, new LdFldHandler(true) },

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
        { Code.Ldind_Ref, new LdIndRefHandler() },

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
        { Code.Ldstr, new LdStrHandler() },
    };

    private class LdFldHandler(bool getAddress) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var reference = (FieldReference)context.CurrentInstruction.Operand;
            var field = reference.Resolve();

            CBaseExpression newExpression;

            var fieldConversionInfo = context.ConversionCatalog.Find(new IlFieldId(field.FullName));
            if (field.IsStatic)
            {
                var variable = new Variable(fieldConversionInfo.FieldTypeConversionInfo,
                    fieldConversionInfo.NameInC.Value,
                    field.FieldType.IsPointer ? 1 : 0);

                newExpression = new VariableValueExpression(variable);
            }
            else
            {
                var items = context.ExpressionStack.Pop(1);
                var objectExpression = items[0];

                newExpression = GetFieldAccessExpression(field, fieldConversionInfo, objectExpression, context);
            }

            if (getAddress)
            {
                newExpression = new AdjustPointerDepthExpression(newExpression, 1);
            }

            context.ExpressionStack.Push(newExpression);
            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            var reference = (FieldReference)context.CurrentInstruction.Operand;
            var field = reference.Resolve();

            // We only need to return the declaring type if the field isn't static. If the
            // field is static than we don't actually need to reference the declaring type
            // in code.
            var declaringTypes = field.IsStatic
                ? Array.Empty<IlTypeName>()
                : [new IlTypeName(field.DeclaringType.FullName)];

            return new OpCodeAnalysisResult
            {
                ReferencedTypes = new HashSet<IlTypeName>(declaringTypes),
                ReferencedGlobal = field.IsStatic ? field : null,
            };
        }

        private static CBaseExpression GetFieldAccessExpression(
            FieldDefinition field,
            FieldConversionInfo fieldConversionInfo,
            CBaseExpression objectExpression,
            HandleContext context)
        {
            var originalObjectExpression = objectExpression;
            while (true)
            {
                var objectInfo = objectExpression.ResultingType;
                var containsField = objectInfo.OriginalTypeDefinition
                    .InstanceFields
                    .Any(x => x.Id.Value == field.FullName);

                if (containsField)
                {
                    var fieldVariable = new Variable(
                        fieldConversionInfo.FieldTypeConversionInfo,
                        fieldConversionInfo.NameInC.Value,
                        field.FieldType.IsByReference ? 1 : 0);

                    // If the object expression is a double pointer (ref reference type), 
                    // we need to dereference it once to access fields
                    var adjustedObjectExpression = objectExpression.PointerDepth == 2 && objectExpression.ResultingType.IsReferenceType
                        ? new AdjustPointerDepthExpression(objectExpression, 1)
                        : objectExpression;

                    return new FieldAccessExpression(adjustedObjectExpression, fieldVariable);
                }

                // If this is a dotnet expression, then check the type's base class
                if (objectInfo.OriginalTypeDefinition is DotNetDefinedType dotNetType
                    && dotNetType.Definition.BaseType != null)
                {
                    // Repeat the process for the base class
                    var parent = context.ConversionCatalog.Find(
                        new IlTypeName(dotNetType.Definition.BaseType.FullName));

                    var fieldVariable = new Variable(parent, "base", 0);
                    
                    // If the object expression is a double pointer (ref reference type), 
                    // we need to dereference it once to access fields
                    var adjustedObjectExpression = objectExpression.PointerDepth == 2 && objectExpression.ResultingType.IsReferenceType
                        ? new AdjustPointerDepthExpression(objectExpression, 1)
                        : objectExpression;
                    
                    objectExpression = new FieldAccessExpression(adjustedObjectExpression, fieldVariable);
                    continue;
                }

                // No matching field found
                var message = $"The type {originalObjectExpression.ResultingType.IlName} or its base types do not have " +
                              $"the field {field.FullName}";
                throw new InvalidOperationException(message);
            }
        }
    }

    private class LdIndHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(1);
            var dereferencedExpression = new AdjustPointerDepthExpression(items[0], 0);
            context.ExpressionStack.Push(dereferencedExpression);

            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class LdIndRefHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            // Stdind.ref takes an address off the stack and gets the reference to the object from it.
            // Since the item on the evaluation stack is the object ref, we don't need to do anything here.
            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class LdArgHandler(int? argIndex, bool loadAddress) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var index = 0;
            if (argIndex == null)
            {
                index = context.CurrentInstruction.Operand switch
                {
                    int intIndex => intIndex,
                    ParameterDefinition paramDef => paramDef.Index,
                    _ => throw new ArgumentException(
                        $"Unknown ldarg operand type of {context.CurrentInstruction.Operand.GetType().FullName}"),
                };
            }
            else
            {
                index = argIndex.Value;
            }

            if (context.CurrentMethodConversion.Parameters.Count <= index)
            {
                var message = $"Argument index #{index} referenced but method only " +
                              $"has ${context.CurrentMethodConversion.Parameters.Count} parameters";
                throw new InvalidOperationException(message);
            }

            var parameter = context.CurrentMethodConversion.Parameters[index];
            var parameterInfo = context.ConversionCatalog.Find(parameter.TypeName);
            
            var pointerDepth = parameter.IsReference ? 1 : 0;
            if (parameter.IsReferenceTypeByRef && parameterInfo.IsReferenceType) 
            {
                pointerDepth += 1; // ref reference types get double pointer
            }
            
            var variable = new Variable(parameterInfo, parameter.Name, pointerDepth);
            CBaseExpression newExpression = new VariableValueExpression(variable);
            if (loadAddress)
            {
                newExpression = new AdjustPointerDepthExpression(newExpression, 1);
            }

            context.ExpressionStack.Push(newExpression);
            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class LdCHandler(int? hardCodedNumber) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            string numericLiteral;
            TypeConversionInfo typeInfo;

            if (hardCodedNumber != null)
            {
                numericLiteral = hardCodedNumber.Value.ToString();
                typeInfo = context.ConversionCatalog.Find(new IlTypeName(typeof(int).FullName!));
            }
            else
            {
                switch (context.CurrentInstruction.Operand)
                {
                    case sbyte sbyteValue:
                        numericLiteral = sbyteValue.ToString();
                        typeInfo = context.ConversionCatalog.Find(new IlTypeName(typeof(sbyte).FullName!));
                        break;

                    case byte byteValue:
                        numericLiteral = byteValue.ToString();
                        typeInfo = context.ConversionCatalog.Find(new IlTypeName(typeof(byte).FullName!));
                        break;

                    case short shortValue:
                        numericLiteral = shortValue.ToString();
                        typeInfo = context.ConversionCatalog.Find(new IlTypeName(typeof(short).FullName!));
                        break;

                    case ushort ushortValue:
                        numericLiteral = ushortValue.ToString();
                        typeInfo = context.ConversionCatalog.Find(new IlTypeName(typeof(ushort).FullName!));
                        break;

                    case int intValue:
                        numericLiteral = intValue.ToString();
                        typeInfo = context.ConversionCatalog.Find(new IlTypeName(typeof(int).FullName!));
                        break;

                    case uint uintValue:
                        numericLiteral = uintValue.ToString();
                        typeInfo = context.ConversionCatalog.Find(new IlTypeName(typeof(uint).FullName!));
                        break;

                    case long longValue:
                        numericLiteral = longValue.ToString();
                        typeInfo = context.ConversionCatalog.Find(new IlTypeName(typeof(long).FullName!));
                        break;

                    case ulong ulongValue:
                        numericLiteral = ulongValue.ToString();
                        typeInfo = context.ConversionCatalog.Find(new IlTypeName(typeof(ulong).FullName!));
                        break;

                    case float floatValue:
                        numericLiteral = floatValue.ToString(CultureInfo.InvariantCulture);
                        typeInfo = context.ConversionCatalog.Find(new IlTypeName(typeof(float).FullName!));
                        break;

                    case double doubleValue:
                        numericLiteral = doubleValue.ToString(CultureInfo.InvariantCulture);
                        typeInfo = context.ConversionCatalog.Find(new IlTypeName(typeof(double).FullName!));
                        break;

                    default:
                        throw new NotSupportedException(context.CurrentInstruction.Operand.GetType().FullName);
                }
            }

            var expression = new LiteralValueExpression(numericLiteral, typeInfo, 0);
            context.ExpressionStack.Push(expression);

            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            // We need to make sure it's known that this instruction depends on the type of the
            // operand, otherwise we risk a native type missing from the conversion catalog.

            var referencedTypes = new HashSet<IlTypeName>();
            if (context.CurrentInstruction.Operand != null)
            {
                referencedTypes.Add(new IlTypeName(context.CurrentInstruction.Operand.GetType().FullName!));
            }

            return new OpCodeAnalysisResult
            {
                ReferencedTypes = referencedTypes,
            };
        }
    }

    private class LdLocHandler(int? index, bool getAddress) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var localIndex = 0;
            if (index != null)
            {
                localIndex = index.Value;
            }
            else
            {
                var variable = (VariableDefinition)context.CurrentInstruction.Operand;
                localIndex = variable.Index;
            }

            if (context.CurrentMethodConversion.Locals.Count <= localIndex)
            {
                var message = $"Requested to load local #{localIndex} but only " +
                              $"{context.CurrentMethodConversion.Locals.Count} are defined";
                throw new InvalidOperationException(message);
            }

            var local = context.CurrentMethodConversion.Locals[localIndex];
            var localInfo = context.ConversionCatalog.Find(local.TypeName);
            var pointerDepth = 0;
            if (local.IsReference || localInfo.IlName.IsPointer())
            {
                pointerDepth = 1;
            }
            else if (localInfo.IsReferenceType)
            {
                pointerDepth = 1; // Reference type local variables are pointers in C
            }
            
            var expression = new VariableValueExpression(
                new Variable(
                    localInfo,
                    Utils.LocalName(context.CurrentDotNetMethod.Definition, localIndex),
                    pointerDepth));

            CBaseExpression newExpression = getAddress
                ? new AdjustPointerDepthExpression(expression, 1)
                : expression;

            context.ExpressionStack.Push(newExpression);

            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class LdObjHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            HandleContext context)
        {
            var items = context.ExpressionStack.Pop(1);
            var objectAddress = items[0];

            context.ExpressionStack.Push(new AdjustPointerDepthExpression(objectAddress, 0));

            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class LdStrHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var stringType = context.ConversionCatalog.Find(new IlTypeName(typeof(string).FullName!));
            var stringValue = EscapeString((string)context.CurrentInstruction.Operand);
            var expression = new LiteralValueExpression($"\"{stringValue}\"", stringType, 0);
            context.ExpressionStack.Push(expression);

            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }

        private static string EscapeString(string input)
        {
            return input
                .Replace("\\", "\\\\") // slashes must be escaped first
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\t", "\\t")
                .Replace("\"", "\\\"");
        }
    }
}