using Dntc.Common.Definitions;
using Dntc.Common.Definitions.CustomDefinedMethods;
using Dntc.Common.Definitions.ReferenceTypeSupport;
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
        new IlMethodId("System.Void System.Object::.ctor()")
            
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
        IlTypeName returnTypeName,
        bool isVirtualCall = false)
    {
        var conversionInfo = context.ConversionCatalog.Find(methodId);
        var returnType = conversionInfo.ReturnTypeInfo;

        // Arguments (including the instance if this isn't a static call) are pushed onto the stack in the order
        // they are called, which means they are popped in the reverse order needed. So we need to revers the
        // order of popped items before building a method call expression
        var arguments = context.ExpressionStack.Pop(conversionInfo.Parameters.Count)
            .Reverse() 
            .ToArray();

        var methodCallExpression = new MethodCallExpression(methodId, context.ConversionCatalog, arguments)
        {
            IsVirtualCall = isVirtualCall,
        };

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
            var tempVariable = new Variable(returnType, name, returnType.IsPointer ? 1 : 0);
            var tempVariableExpression = new VariableValueExpression(tempVariable);

            var localDeclaration = new LocalDeclarationStatementSet(tempVariable);
            var assignment = new AssignmentStatementSet(tempVariableExpression, methodCallExpression);

            var statements = new List<CStatementSet>([localDeclaration, assignment]);

            context.ExpressionStack.Push(tempVariableExpression);
            return new OpCodeHandlingResult(new CompoundStatementSet(statements));
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
                if (methodReference.HasThis)
                {
                    // + 1 to include the parameter.
                    context.ExpressionStack.Pop(methodReference.Parameters.Count + 1); 
                    // specifically, because System_Object::ctor is excluded.
                    // otherwise it will try to add return __this at the end.
                }
                
                return new OpCodeHandlingResult(null);
            }

            return CallMethodReference(context, methodId, new IlTypeName(methodReference.ReturnType.FullName));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            if (GetCallTarget(context.CurrentInstruction) is { } callTarget)
            {
                return new OpCodeAnalysisResult
                {
                    CalledMethods = [ callTarget ]
                };
            }

            return new OpCodeAnalysisResult();
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

            var expression = new MethodCallExpression(
                fnPointerExpression,
                context.CurrentMethodConversion.Parameters,
                argumentsInCallingOrder,
                returnType,
                context.ConversionCatalog);

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
            var variable = new Variable(
                objType,
                $"__temp_{context.CurrentInstruction.Offset:x4}",
                (!constructor.DeclaringType.IsValueType) ? 1 : 0);

            var argumentsInCallingOrder = context.ExpressionStack.Pop(constructorInfo.Parameters.Count - 1)
                .Reverse()
                .ToList();

            // Add a pointer to the variable
            var variableExpression = new VariableValueExpression(variable);
            argumentsInCallingOrder.Insert(0, new AdjustPointerDepthExpression(variableExpression, 1));

            var statements = new List<CStatementSet>();
            
            var initStatement = new LocalDeclarationStatementSet(variable);
            statements.Add(initStatement);

            if (!constructor.DeclaringType.IsValueType)
            {
                // We need to make sure the temp variable is untracked if it's being set in a loop
                statements.Add(new GcUntrackFunctionCallStatement(variableExpression, context.ConversionCatalog));

                var createMethodId = ReferenceTypeAllocationMethod.FormIlMethodId(constructor.DeclaringType.Resolve());
                var createFnCall = new MethodCallExpression(createMethodId, context.ConversionCatalog);
                var assignment = new AssignmentStatementSet(variableExpression, createFnCall);
                statements.Add(assignment);

                var incrementFnCall = new MethodCallExpression(
                    ReferenceTypeConstants.GcTrackMethodId,
                    context.ConversionCatalog,
                    variableExpression);

                statements.Add(new VoidExpressionStatementSet(incrementFnCall));
            }

            var methodCall = new MethodCallExpression(
                constructorId,
                context.ConversionCatalog,
                argumentsInCallingOrder.ToArray());

            var methodCallStatement = new VoidExpressionStatementSet(methodCall);
            
            statements.Add(methodCallStatement);

            context.ExpressionStack.Push(variableExpression);

            return new OpCodeHandlingResult(new CompoundStatementSet(statements));
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            var extraCalls = new List<InvokedMethod>();
            var constructor = (MethodReference)context.CurrentInstruction.Operand;

            if (!constructor.DeclaringType.IsValueType)
            {
                var createMethodId = ReferenceTypeAllocationMethod.FormIlMethodId(constructor.DeclaringType.Resolve());
                extraCalls.Add(new InvokedMethod(createMethodId));

                extraCalls.Add(new InvokedMethod(ReferenceTypeConstants.GcTrackMethodId));
            }
            
            if (GetCallTarget(context.CurrentInstruction) is { } callTarget)
            {
                extraCalls.Add(callTarget);
            }
            
            return new OpCodeAnalysisResult
            {
                CalledMethods = extraCalls
            };
        }
    }
    
    private class CallVirtHandler : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var methodToCall = VirtualCallConverter.Convert(context.CurrentInstruction, context.CurrentDotNetMethod);
            var targetMethodDefinition = context.DefinitionCatalog.Get(methodToCall);

            bool virtualCall = targetMethodDefinition is DotNetDefinedMethod dntDefinedMethod
                               && !dntDefinedMethod.Definition.DeclaringType.IsValueType
                               && dntDefinedMethod.Definition.IsVirtual;
            
            if (targetMethodDefinition == null)
            {
                var message = $"No known definition for the method '{methodToCall}'";
                throw new InvalidOperationException(message);
            }

            return CallMethodReference(context, methodToCall, targetMethodDefinition.ReturnType, virtualCall);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult
            {
                CalledMethods = [ new InvokedMethod(
                    VirtualCallConverter.Convert(
                        context.CurrentInstruction,
                        context.CurrentMethod)) ]
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
                conversionInfo.ReturnTypeInfo,
                0);

            var functionAddress = new AdjustPointerDepthExpression(functionNameExpression, 1);
            context.ExpressionStack.Push(functionAddress);

            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            if (GetCallTarget(context.CurrentInstruction) is { } callTarget)
            {
                return new OpCodeAnalysisResult
                {
                    // Even though the method isn't called, we need to analyze it as a
                    // called method so the dependency graph gets generated for it, so it gets
                    // transpiled and can have a pointer created from it.
                
                    CalledMethods = [ callTarget ],
                };
            }
            
            return new OpCodeAnalysisResult
            {
                CalledMethods = [ ]
            };
        }
    }
}