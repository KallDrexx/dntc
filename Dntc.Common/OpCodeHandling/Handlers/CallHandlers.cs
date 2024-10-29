using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
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

    private static bool ReturnsVoid(TypeReference type) => type.FullName == typeof(void).FullName;
    private static bool ReturnsVoid(IlTypeName type) => type.Value == typeof(void).FullName;

    private static InvokedMethod? GetCallTarget(Instruction instruction, DotNetDefinedMethod method)
    {
        return instruction.Operand switch
        {
            GenericInstanceMethod generic => 
                new GenericInvokedMethod(
                    new IlMethodId(generic.FullName),
                    new IlMethodId(generic.ElementMethod.FullName),
                    generic.GenericArguments.Select(x => new IlTypeName(x.FullName)).ToArray()),
            
            MethodReference reference => new InvokedMethod(new IlMethodId(reference.FullName)),
            _ => null
        };
    }
    
    private static OpCodeHandlingResult CallMethodReference(
        HandleContext context, 
        IlMethodId methodId, 
        IlTypeName returnTypeName)
    {
        var conversionInfo = context.ConversionCatalog.Find(methodId);
        var voidType = context.ConversionCatalog.Find(new IlTypeName(typeof(void).FullName!));
        var returnType = context.ConversionCatalog.Find(returnTypeName);

        // Arguments (including the instance if this isn't a static call) are pushed onto the stack in the order
        // they are called, which means they are popped in the reverse order needed. So we need to revers the
        // order of popped items before building a method call expression
        var arguments = context.ExpressionStack.Pop(conversionInfo.Parameters.Count)
            .Reverse() 
            .ToArray();

        var fnExpression = new LiteralValueExpression(conversionInfo.NameInC.Value, voidType);
        var expression = new MethodCallExpression(fnExpression, arguments, returnType);
        
        if (ReturnsVoid(returnTypeName))
        {
            var statement = new VoidExpressionStatementSet(expression);
            return new OpCodeHandlingResult(statement);
        }
        
        context.ExpressionStack.Push(expression);
        return new OpCodeHandlingResult(null);
    }
    
    private class CallHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var methodReference = (MethodReference)context.CurrentInstruction.Operand;
            var methodId = new IlMethodId(methodReference.FullName);
            return CallMethodReference(context, methodId, new IlTypeName(methodReference.ReturnType.FullName));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult(
                GetCallTarget(
                    context.CurrentInstruction,
                    context.CurrentMethod));
        }
    }
    
    private class CallIHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var callSite = (CallSite)context.CurrentInstruction.Operand;
            
            // Top of the stack contains the function name to call, followed by the arguments in 
            // reverse calling order
            var allItems = context.ExpressionStack.Pop(callSite.Parameters.Count + 1);
            var fnPointerExpression = allItems[0];
            var argumentsInCallingOrder = allItems.Skip(1).Reverse().ToArray();
            var returnType = context.ConversionCatalog.Find(new IlTypeName(callSite.ReturnType.FullName));

            var expression = new MethodCallExpression(fnPointerExpression, argumentsInCallingOrder, returnType);
            if (ReturnsVoid(callSite.ReturnType))
            {
                var statement = new VoidExpressionStatementSet(expression);
                return new OpCodeHandlingResult(statement);
            }
            
            context.ExpressionStack.Push(expression);
            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            // Since it's calling a function pointer, we don't have a concrete
            // call target

            return new OpCodeAnalysisResult();
        }
    }
    
    private class NewObjHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var constructor = (MethodReference)context.CurrentInstruction.Operand;
            if (!constructor.DeclaringType.IsValueType)
            {
                var message = $"Cannot call `newobj` on {constructor.DeclaringType.FullName} as it " +
                              $"is a reference type and only value types are currently supported";
                throw new InvalidOperationException(message);
            }

            var constructorId = new IlMethodId(constructor.FullName);
            var constructorInfo = context.ConversionCatalog.Find(constructorId);
            var objType = context.ConversionCatalog.Find(new IlTypeName(constructor.DeclaringType.FullName));
            var variable = new Variable(objType, $"__temp_{context.CurrentInstruction.Offset:x4}", false);
            var voidType = context.ConversionCatalog.Find(new IlTypeName(typeof(void).FullName!));

            var argumentsInCallingOrder = context.ExpressionStack.Pop(constructorInfo.Parameters.Count - 1)
                .Reverse()
                .ToList();

            // Add a pointer to the variable
            var variableExpression = new VariableValueExpression(variable);
            argumentsInCallingOrder.Insert(0, new AddressOfValueExpression(variableExpression));

            var initStatement = new LocalDeclarationStatementSet(variable);
            var fnExpression = new LiteralValueExpression(constructorInfo.NameInC.Value, voidType);
            var methodCall = new MethodCallExpression(fnExpression, argumentsInCallingOrder, voidType);
            var methodCallStatement = new VoidExpressionStatementSet(methodCall);

            context.ExpressionStack.Push(variableExpression);

            return new OpCodeHandlingResult(new CompoundStatementSet([initStatement, methodCallStatement]));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult(
                GetCallTarget(
                    context.CurrentInstruction,
                    context.CurrentMethod));
        }
    }
    
    private class CallVirtHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var methodToCall = VirtualCallConverter.Convert(context.CurrentInstruction, context.CurrentDotNetMethod);
            var targetMethodDefinition = context.DefinitionCatalog.Get(methodToCall);
            if (targetMethodDefinition == null)
            {
                var message = $"No known definition for the method '{methodToCall}'";
                throw new InvalidOperationException(message);
            }

            return CallMethodReference(context, methodToCall, targetMethodDefinition.ReturnType);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult(
                new InvokedMethod(
                    VirtualCallConverter.Convert(
                        context.CurrentInstruction, 
                        context.CurrentMethod)));
        }
    }
}