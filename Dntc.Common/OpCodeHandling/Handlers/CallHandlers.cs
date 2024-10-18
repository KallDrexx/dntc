using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class CallHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Call, new CallHandler() },
        { Code.Calli, new CallIHandler() },
        { Code.Newobj, new NewObjHandler() },
        { Code.Callvirt, new CallVirtHandler() },
    };
    
    private class CallHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var methodReference = (MethodReference)currentInstruction.Operand;
            var methodId = new IlMethodId(methodReference.FullName);
            var conversionInfo = conversionCatalog.Find(methodId);
            var voidType = conversionCatalog.Find(new IlTypeName(typeof(void).FullName!));
            var returnType = conversionCatalog.Find(new IlTypeName(methodReference.ReturnType.FullName));

            var arguments = expressionStack.Pop(conversionInfo.Parameters.Count)
                .Reverse() // Items are passed into the method in the reverse order they are popped
                .ToArray();

            var fnExpression = new LiteralValueExpression(conversionInfo.NameInC.Value, voidType);
            var expression = new MethodCallExpression(fnExpression, arguments, returnType);
            
            if (ReturnsVoid(methodReference.ReturnType))
            {
                var statement = new VoidExpressionStatementSet(expression);
                return new OpCodeHandlingResult(statement);
            }
            
            expressionStack.Push(expression);
            return new OpCodeHandlingResult(null);
        }
    }
    
    private class CallIHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var callSite = (CallSite)currentInstruction.Operand;
            
            // Top of the stack contains the function name to call, followed by the arguments in 
            // reverse calling order
            var allItems = expressionStack.Pop(callSite.Parameters.Count + 1);
            var fnPointerExpression = allItems[0];
            var argumentsInCallingOrder = allItems.Skip(1).Reverse().ToArray();
            var returnType = conversionCatalog.Find(new IlTypeName(callSite.ReturnType.FullName));

            var expression = new MethodCallExpression(fnPointerExpression, argumentsInCallingOrder, returnType);
            if (ReturnsVoid(callSite.ReturnType))
            {
                var statement = new VoidExpressionStatementSet(expression);
                return new OpCodeHandlingResult(statement);
            }
            
            expressionStack.Push(expression);
            return new OpCodeHandlingResult(null);
        }
    }
    
    private class NewObjHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var constructor = (MethodReference)currentInstruction.Operand;
            if (!constructor.DeclaringType.IsValueType)
            {
                var message = $"Cannot call `newobj` on {constructor.DeclaringType.FullName} as it " +
                              $"is a reference type and only value types are currently supported";
                throw new InvalidOperationException(message);
            }

            var constructorId = new IlMethodId(constructor.FullName);
            var constructorInfo = conversionCatalog.Find(constructorId);
            var objType = conversionCatalog.Find(new IlTypeName(constructor.DeclaringType.FullName));
            var variable = new Variable(objType, $"__temp_{currentInstruction.Offset:x4}", false);
            var voidType = conversionCatalog.Find(new IlTypeName(typeof(void).FullName!));

            var argumentsInCallingOrder = expressionStack.Pop(constructorInfo.Parameters.Count - 1)
                .Reverse()
                .ToList();

            // Add a pointer to the variable
            var variableExpression = new VariableValueExpression(variable);
            argumentsInCallingOrder.Insert(0, new AddressOfValueExpression(variableExpression));

            var initStatement = new LocalDeclarationStatementSet(variable);
            var fnExpression = new LiteralValueExpression(constructorInfo.NameInC.Value, voidType);
            var methodCall = new MethodCallExpression(fnExpression, argumentsInCallingOrder, voidType);
            var methodCallStatement = new VoidExpressionStatementSet(methodCall);

            expressionStack.Push(variableExpression);

            return new OpCodeHandlingResult(new CompoundStatementSet([initStatement, methodCallStatement]));
        }
    }
    
    private class CallVirtHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var methodToCall = VirtualCallConverter.Convert(currentInstruction, currentMethod);
            
        }
    }

    private static bool ReturnsVoid(TypeReference type) => type.FullName == typeof(void).FullName;
}