using Dntc.Common.Definitions.CustomDefinedMethods;
using Dntc.Common.Definitions.CustomDefinedTypes;
using Dntc.Common.Definitions.ReferenceTypeSupport;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class ArrayHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Ldlen, new LdLenHandler() },
        { Code.Newarr, new NewArrHandler() },

        { Code.Ldelema, new LdElemHandler() },
        { Code.Ldelem_I, new LdElemHandler() },
        { Code.Ldelem_I1, new LdElemHandler() },
        { Code.Ldelem_I2, new LdElemHandler() },
        { Code.Ldelem_I4, new LdElemHandler() },
        { Code.Ldelem_I8, new LdElemHandler() },
        { Code.Ldelem_U1, new LdElemHandler() },
        { Code.Ldelem_U2, new LdElemHandler() },
        { Code.Ldelem_R4, new LdElemHandler() },
        { Code.Ldelem_R8, new LdElemHandler() },
        { Code.Ldelem_Any, new LdElemHandler() },
        { Code.Ldelem_Ref, new LdElemHandler() },

        { Code.Stelem_I, new StElemHandler() },
        { Code.Stelem_I1, new StElemHandler() },
        { Code.Stelem_I2, new StElemHandler() },
        { Code.Stelem_I4, new StElemHandler() },
        { Code.Stelem_I8, new StElemHandler() },
        { Code.Stelem_R4, new StElemHandler() },
        { Code.Stelem_R8, new StElemHandler() },
        { Code.Stelem_Any, new StElemHandler() },
        { Code.Stelem_Ref, new StElemHandler() },
    };
    
    private class StElemHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(3);
            var value = items[0];
            var index = items[1];
            var array = items[2];

            if (array.ResultingType.OriginalTypeDefinition is not ArrayDefinedType arrayDefinedType)
            {
                var message = $"stelem opcode received for an expression returning {array.ResultingType.IlName} " +
                              $"which was defined by {array.ResultingType.OriginalTypeDefinition.GetType().FullName} " +
                              $"which does not inherit from ArrayDefinedType";

                throw new InvalidOperationException(message);
            }
            
            var indexExpression = new AdjustPointerDepthExpression(index, 0);

            var itemType = value.ResultingType;
            var itemsExpression = arrayDefinedType.GetItemsAccessorExpression(array, context.ConversionCatalog);
            var arrayIndex = new ArrayIndexExpression(itemsExpression, indexExpression, itemType);
            
            var statements = new List<CStatementSet>();
            var lengthField = arrayDefinedType.GetArraySizeExpression(array, context.ConversionCatalog);
            if (lengthField != null)
            {
                var lengthCheck = arrayDefinedType.GetLengthCheckExpression(lengthField, array, indexExpression);
                statements.Add(lengthCheck);
            }

            var storeStatement = new ArrayStoreStatementSet(arrayIndex, value);

            if (value.ResultingType.IsReferenceType)
            {
                statements.Add(new GcTrackFunctionCallStatement(value, context.ConversionCatalog));
            }

            statements.Add(storeStatement);

            return new OpCodeHandlingResult(new CompoundStatementSet(statements));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class LdLenHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(1);
            var array = items[0];

            if (array.ResultingType.OriginalTypeDefinition is not ArrayDefinedType arrayDefinedType)
            {
                var message = $"LdLen opcode called for an expression returning {array.ResultingType.IlName} " +
                              "which was defined by {array.ResultingType.OriginalTypeDefinition.GetType().FullName} " +
                              "which does not inherit from ArrayDefinedType";

                throw new InvalidOperationException(message);
            }

            var lengthField = arrayDefinedType.GetArraySizeExpression(array, context.ConversionCatalog);
            if (lengthField == null)
            {
                var message = $"No known way to get the size of an array of type {array.ResultingType.IlName}";
                throw new InvalidOperationException(message);
            }

            context.ExpressionStack.Push(lengthField);
            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }

    private class LdElemHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var items = context.ExpressionStack.Pop(2);
            var index = items[0];
            var array = items[1];

            if (array.ResultingType.OriginalTypeDefinition is not ArrayDefinedType arrayDefinedType)
            {
                var message = $"ldelem opcode received for an expression returning {array.ResultingType.IlName} " +
                              $"which was defined by {array.ResultingType.OriginalTypeDefinition.GetType().FullName} " +
                              $"which does not inherit from ArrayDefinedType";

                throw new InvalidOperationException(message);
            }

            var elementType = context.ConversionCatalog.Find(arrayDefinedType.ElementType);
            var indexExpression = new AdjustPointerDepthExpression(index, 0);
            var itemsExpression = arrayDefinedType.GetItemsAccessorExpression(array, context.ConversionCatalog);
            var arrayIndex = new ArrayIndexExpression(itemsExpression, indexExpression, elementType);
            
            // Return the length check while adding the accessor expression to the stack
            context.ExpressionStack.Push(arrayIndex);

            var lengthField = arrayDefinedType.GetArraySizeExpression(array, context.ConversionCatalog);

            if (lengthField != null)
            {
                var lengthCheck = arrayDefinedType.GetLengthCheckExpression(lengthField, array, indexExpression);
                return new OpCodeHandlingResult(lengthCheck);
            }

            // Without a length check there are no statements to write out atm.
            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult();
        }
    }
    
    private class NewArrHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var arrayElementType = (TypeReference)context.CurrentInstruction.Operand;
            var arrayType = arrayElementType.MakeArrayType();
            var elementTypeInfo = context.ConversionCatalog.Find(new IlTypeName(arrayElementType.FullName));
            var arrayInfo = context.ConversionCatalog.Find(new IlTypeName(arrayType.FullName));
            if (arrayInfo.OriginalTypeDefinition is not ArrayDefinedType arrayDefinedType)
            {
                var message = $"Expected array to be defined as an ArrayDefinedType, but instead was " +
                              $"a '{arrayInfo.OriginalTypeDefinition.GetType().FullName}";
                throw new InvalidOperationException(message);
            }

            var items = context.ExpressionStack.Pop(1);
            var count = items[0];

            var name = $"__temp_{context.CurrentInstruction.Offset:x4}";
            var tempVariable = new Variable(arrayInfo, name, 1);
            var tempVariableExpression = new VariableValueExpression(tempVariable);

            var createFnCallExpression = new MethodCallExpression(
                ReferenceTypeAllocationMethod.FormIlMethodId(arrayType),
                context.ConversionCatalog);

            // Allocate items pointer.
            // TODO: This should be customizable for different types of arrays
            var itemAllocator = context.MemoryManagementActions.AllocateCall(
                arrayDefinedType.GetItemsAccessorExpression(tempVariableExpression, context.ConversionCatalog),
                new LiteralValueExpression(elementTypeInfo.NameInC.Value, elementTypeInfo, 0),
                context.ConversionCatalog,
                count);

            // Set the item size value
            var sizeExpression = arrayDefinedType.GetArraySizeExpression(
                tempVariableExpression,
                context.ConversionCatalog);

            if (sizeExpression == null)
            {
                var message = $"Array defined type {arrayDefinedType.GetType()} gave a null size expression";
                throw new InvalidOperationException(message);
            }

            var sizeAssignment = new AssignmentStatementSet(sizeExpression, count);

            // Make sure to ref count it
            var gcTrack = new GcTrackFunctionCallStatement(tempVariableExpression, context.ConversionCatalog);

            context.ExpressionStack.Push(new VariableValueExpression(tempVariable));

            return new OpCodeHandlingResult(new CompoundStatementSet([
                new LocalDeclarationStatementSet(tempVariable),
                new AssignmentStatementSet(tempVariableExpression, createFnCallExpression),
                itemAllocator,
                sizeAssignment,
                gcTrack,
            ]));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            var definition = ((TypeDefinition)context.CurrentInstruction.Operand).MakeArrayType();
            var allocationMethod = new ReferenceTypeAllocationMethod(context.MemoryManagementActions, definition);

            // Use the newarr opcode to know when we need to add the prep for free function for this array type
            var prepMethod = new PrepToFreeDefinedMethod(
                new HeapArrayDefinedType(definition),
                context.MemoryManagementActions);

            return new OpCodeAnalysisResult
            {
                CalledMethods = [new CustomInvokedMethod(allocationMethod), new CustomInvokedMethod(prepMethod)],
            };
        }
    }
}