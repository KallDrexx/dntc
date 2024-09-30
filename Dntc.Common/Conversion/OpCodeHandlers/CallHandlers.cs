using System.Text;
using Dntc.Common.Definitions;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class CallHandlers : IOpCodeFnFactory
{
    private record CallArgument(EvaluationStackItem Value, bool isParameterAReference);
    
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Call, HandleCall },
        { Code.Calli, HandleCallI },
        { Code.Newobj, HandleNewObj },
    };

    private static async ValueTask HandleCall(OpCodeHandlingContext context)
    {
        IlMethodId methodId;
        TypeReference returnType;
        switch (context.Operand)
        {
            case MethodDefinition definition:
                methodId = new IlMethodId(definition.FullName);
                returnType = definition.ReturnType;
                break;
            
            case MethodReference reference:
                methodId = new IlMethodId(reference.FullName);
                returnType = reference.ReturnType;
                break;
            
            default:
                throw new NotSupportedException(context.Operand.GetType().FullName);
        }

        var conversionInfo = context.ConversionCatalog.Find(methodId);
        var methodInfo = context.DefinitionCatalog.Get(methodId);
        if (methodInfo == null)
        {
            var message = $"Could not find definition for method '{methodId.Value}'";
            throw new InvalidOperationException(message);
        }

        var arguments = context.EvaluationStack
            .PopCount(methodInfo.Parameters.Count)
            .Reverse() // Items are passed into the method in the reverse order they were popped in
            .Select((x, index) => new CallArgument(x, methodInfo.Parameters[index].IsReference))
            .ToArray();
        
        await ExecuteCallHandle(context, conversionInfo.NameInC, arguments, ReturnsVoid(returnType));
    }

    private static async ValueTask HandleCallI(OpCodeHandlingContext context)
    {
        var callSite = (CallSite)context.Operand;
        
        // Top of the stack contains the function name to call, followed by the arguments in reverse calling order
        var allItems = context.EvaluationStack.PopCount(callSite.Parameters.Count + 1);
        var name = new CFunctionName(allItems[0].RawText);
        var argumentsInCallingOrder = allItems.Skip(1).Reverse().ToArray();
        var arguments = argumentsInCallingOrder
            .Select((x, index) => new CallArgument(x, callSite.Parameters[index].ParameterType.IsByReference))
            .ToArray();

        await ExecuteCallHandle(context, name, arguments, ReturnsVoid(callSite.ReturnType));
    }

    private static ValueTask HandleNewObj(OpCodeHandlingContext context)
    {
        throw new NotImplementedException();
    }

    private static async Task ExecuteCallHandle(
        OpCodeHandlingContext context, 
        CFunctionName functionName,
        IReadOnlyList<CallArgument> callArguments,
        bool returnsVoid)
    {
        var functionCallString = new StringBuilder();
        functionCallString.Append($"{functionName}(");
        for (var x = 0; x < callArguments.Count; x++)
        {
            var callArgument = callArguments[x];
            
            if (x > 0)
            {
                functionCallString.Append(", ");
            }

            var argumentString = callArgument.isParameterAReference
                ? callArgument.Value.TextReference
                : callArgument.Value.TextDerefed;

            functionCallString.Append(argumentString);
        }

        functionCallString.Append(')');
        
        // If this function call returns void, then nothing will be put on the stack, and therefore
        // we need to write it out now, as nothing will intentionally pop it later. If this doesn't
        // return void, then we need to store the call to the evaluation stack for when it gets
        // read/stored/popped later.
        if (returnsVoid)
        {
            await context.Writer.WriteLineAsync($"\t{functionCallString};");
        }
        else
        {
            context.EvaluationStack.Push(new EvaluationStackItem(functionCallString.ToString(), false));
        }
    }

    private static bool ReturnsVoid(TypeReference type) => type.Name == "void";
}