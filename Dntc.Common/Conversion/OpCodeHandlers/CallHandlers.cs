using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class CallHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Call, HandleCall },
        { Code.Calli, HandleCallI },
    };

    private static async ValueTask HandleCall(OpCodeHandlingContext context)
    {
        var methodDefinition = (MethodDefinition)context.Operand;
        var conversionInfo = context.ConversionCatalog.Find(new IlMethodId(methodDefinition.FullName));
        var methodInfo = context.DefinitionCatalog.Get(new IlMethodId(methodDefinition.FullName));
        if (methodInfo == null)
        {
            var message = $"Could not find definition for method '{methodDefinition.FullName}'";
            throw new InvalidOperationException(message);
        }
        
        var arguments = context.EvaluationStack.PopCount(methodInfo.Parameters.Count);
        
        // Items are passed into the method in the reverse order they were popped in
        arguments = arguments.Reverse().ToArray();
        
        await ExecuteCallHandle(context, conversionInfo.NameInC, arguments, ReturnsVoid(methodDefinition.ReturnType));
    }

    private static async ValueTask HandleCallI(OpCodeHandlingContext context)
    {
        var callSite = (CallSite)context.Operand;
        
        // Top of the stack contains the function name to call, followed by the arguments in reverse calling order
        var allItems = context.EvaluationStack.PopCount(callSite.Parameters.Count + 1);
        var name = new CFunctionName(allItems[0].Text);
        var argumentsInCallingOrder = allItems.Skip(1).Reverse().ToArray();

        await ExecuteCallHandle(context, name, argumentsInCallingOrder, ReturnsVoid(callSite.ReturnType));
    }

    private static async Task ExecuteCallHandle(
        OpCodeHandlingContext context, 
        CFunctionName functionName,
        IReadOnlyList<EvaluationStackItem> argumentsInCallingOrder,
        bool returnsVoid)
    {
        var functionCallString = new StringBuilder();
        functionCallString.Append($"{functionName}(");
        for (var x = 0; x < argumentsInCallingOrder.Count; x++)
        {
            if (x > 0)
            {
                functionCallString.Append(", ");
            }

            functionCallString.Append(argumentsInCallingOrder[x].Text);
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
            context.EvaluationStack.Push(new EvaluationStackItem(functionCallString.ToString()));
        }
    }

    private static bool ReturnsVoid(TypeReference type) => type.Name == "void";
}