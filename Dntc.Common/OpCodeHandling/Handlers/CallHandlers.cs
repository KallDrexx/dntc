using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
using Dntc.Common.Syntax.Expressions;
using Dntc.Common.Syntax.Statements;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class CallHandlers : IOpCodeHandlerCollection
{
    /// <summary>
    /// Methods that should not be executed and be bypassed.
    /// </summary>
    private static readonly HashSet<IlMethodId> IgnoredMethods =
        [
            // GetTypeFromHandle comes from a `typeof()` expression and usually follows a
            // `ldtoken` op code. Since the `ldtoken` will translate the call directly to the
            // type, we don't need this intermediary step.
            new IlMethodId("System.Type System.Type::GetTypeFromHandle(System.RuntimeTypeHandle)"),
           // new IlMethodId("System.Void System.Object::.ctor()")
            
        ];

    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Call, new CallHandler() },
        { Code.Calli, new CallIHandler() },
        { Code.Newobj, new NewObjHandler() },
        { Code.Callvirt, new CallVirtHandler() },
        { Code.Ldftn, new LdFtnHandler() },
    };

    private static bool ReturnsVoid(TypeReference type) => type.FullName == typeof(void).FullName;
    private static bool ReturnsVoid(IlTypeName type) => type.Value == typeof(void).FullName;

    private static InvokedMethod? GetCallTarget(Instruction instruction)
    {
        switch (instruction.Operand)
        {
            case GenericInstanceMethod generic:
                var arguments = generic.GenericArguments.Select(x => new IlTypeName(x.FullName)).ToArray();
                var methodId = Utils.NormalizeGenericMethodId(generic.FullName, generic.ElementMethod.GenericParameters);
                var originalMethodId = Utils.NormalizeGenericMethodId(
                    generic.ElementMethod.FullName,
                    generic.ElementMethod.GenericParameters);

                return new GenericInvokedMethod(methodId, originalMethodId, arguments);
                
            case MethodReference reference:
                var referenceMethodId = new IlMethodId(reference.FullName);
                return IgnoredMethods.Contains(referenceMethodId)
                    ? null
                    : new InvokedMethod(referenceMethodId);
            
            default:
                return null;
        }
    }
    
    private static OpCodeHandlingResult CallMethodReference(
        HandleContext context, 
        IlMethodId methodId, 
        IlTypeName returnTypeName, bool isVirtualCall = false)
    {
        var conversionInfo = context.ConversionCatalog.Find(methodId);
        var voidType = context.ConversionCatalog.Find(new IlTypeName(typeof(void).FullName!));
        var returnType = conversionInfo.ReturnTypeInfo;

        // Arguments (including the instance if this isn't a static call) are pushed onto the stack in the order
        // they are called, which means they are popped in the reverse order needed. So we need to revers the
        // order of popped items before building a method call expression
        var arguments = context.ExpressionStack.Pop(conversionInfo.Parameters.Count)
            .Reverse() 
            .ToArray();

        var fnExpression = new LiteralValueExpression(conversionInfo.NameInC.Value, voidType);
        var methodCallExpression = new MethodCallExpression(fnExpression, conversionInfo.Parameters, arguments, returnType, isVirtualCall);

        if (ReturnsVoid(returnTypeName))
        {
            var statement = new VoidExpressionStatementSet(methodCallExpression);
            return new OpCodeHandlingResult(statement);
        }

        // If this function returns a pointer, we need to store that pointer into a local variable and
        // put that variable on the stack instead of the method call itself. Otherwise, you end up
        // with code like `(*func()) = 25` and other illegal expressions.
        if (returnType.IsPointer)
        {
            var name = $"__temp_{context.CurrentInstruction.Offset:x4}";
            var tempVariable = new Variable(returnType, name, returnType.IsPointer);
            var tempVariableExpression = new VariableValueExpression(tempVariable);

            var localDeclaration = new LocalDeclarationStatementSet(tempVariable);
            var assignment = new AssignmentStatementSet(tempVariableExpression, methodCallExpression);

            context.ExpressionStack.Push(tempVariableExpression);
            return new OpCodeHandlingResult(new CompoundStatementSet([localDeclaration, assignment]));
        }

        // Otherwise we can just inline the method call.
        context.ExpressionStack.Push(methodCallExpression);
        return new OpCodeHandlingResult(null);
    }

    private class CallHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var methodReference = (MethodReference)context.CurrentInstruction.Operand;
            var methodId = methodReference is GenericInstanceMethod generic && generic.HasGenericArguments
                ? Utils.NormalizeGenericMethodId(generic.FullName, generic.ElementMethod.GenericParameters)
                : new IlMethodId(methodReference.FullName);

            if (IgnoredMethods.Contains(methodId))
            {
                context.ExpressionStack.Pop(1);
                // we are ignoring this method so we must pop arguments.
                
                return new OpCodeHandlingResult(null);
            }

            return CallMethodReference(context, methodId, new IlTypeName(methodReference.ReturnType.FullName));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult
            {
                CalledMethod = GetCallTarget(context.CurrentInstruction),
            };
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
            
            var expression = new MethodCallExpression(fnPointerExpression, context.CurrentMethodConversion.Parameters, argumentsInCallingOrder, returnType);
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
            var constructorId = new IlMethodId(constructor.FullName);
            var constructorInfo = context.ConversionCatalog.Find(constructorId);
            var objType = context.ConversionCatalog.Find(new IlTypeName(constructor.DeclaringType.FullName));
            var variable = new Variable(objType, $"__temp_{context.CurrentInstruction.Offset:x4}", !constructor.DeclaringType.IsValueType);
            var voidType = context.ConversionCatalog.Find(new IlTypeName(typeof(void).FullName!));

            var argumentsInCallingOrder = context.ExpressionStack.Pop(constructorInfo.Parameters.Count - 1)
                .Reverse()
                .ToList();

            // Add a pointer to the variable
            var variableExpression = new VariableValueExpression(variable);
            argumentsInCallingOrder.Insert(0, new AddressOfValueExpression(variableExpression));

            var statements = new List<CStatementSet>();
            
            var initStatement = new LocalDeclarationStatementSet(variable);
            statements.Add(initStatement);

            if (!constructor.DeclaringType.IsValueType)
            {
                var allocateStatement = new AllocatingStatementSet(variable);
                statements.Add(allocateStatement);
            }

            var fnExpression = new LiteralValueExpression(constructorInfo.NameInC.Value, voidType);
            var methodCall = new MethodCallExpression(fnExpression, constructorInfo.Parameters, argumentsInCallingOrder, voidType);
            var methodCallStatement = new VoidExpressionStatementSet(methodCall);
            
            statements.Add(methodCallStatement);

            context.ExpressionStack.Push(variableExpression);

            return new OpCodeHandlingResult(new CompoundStatementSet(statements));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult
            {
                CalledMethod = GetCallTarget(context.CurrentInstruction),
            };
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

            return CallMethodReference(context, methodToCall, targetMethodDefinition.ReturnType, true);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult
            {
                CalledMethod = new InvokedMethod(
                    VirtualCallConverter.Convert(
                        context.CurrentInstruction,
                        context.CurrentMethod))
            };
        }
    }

    private class LdFtnHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var target = GetCallTarget(context.CurrentInstruction);
            if (target == null)
            {
                var message = $"No call target could be created for ldftn instruction (operand {context.CurrentInstruction.Operand}";
                throw new InvalidOperationException(message);
            }

            var conversionInfo = context.ConversionCatalog.Find(target.MethodId);
            var functionNameExpression = new LiteralValueExpression(
                conversionInfo.NameInC.Value,
                conversionInfo.ReturnTypeInfo);

            var functionAddress = new AddressOfValueExpression(functionNameExpression);
            context.ExpressionStack.Push(functionAddress);

            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult
            {
                // Even though the method isn't called, we need to analyze it as a
                // called method so the dependency graph gets generated for it, so it gets
                // transpiled and can have a pointer created from it.
                CalledMethod = GetCallTarget(context.CurrentInstruction),
            };
        }
    }
}