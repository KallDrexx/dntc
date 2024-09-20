using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class CallHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Call, HandleCall },
    };

    private async static ValueTask HandleCall(OpCodeHandlingContext context)
    {
        var methodDefinition = (MethodDefinition)context.Operand;
        var conversionInfo = context.ConversionCatalog.Find(new IlMethodId(methodDefinition.FullName));
        var arguments = new EvaluationStackItem[methodDefinition.Parameters.Count];
        
        // TODO: account for the object itself when handling instance calls
        if (context.EvaluationStack.Count < arguments.Length)
        {
            var message = $"Call to method {methodDefinition.Name} cannot be performed as it requires " +
                          $"{arguments.Length} parameters but only {context.EvaluationStack.Count} are on the " +
                          $"evaluation stack";
            throw new InvalidOperationException(message);
        }

        // We need the items in the opposite order than they are in the stack, in order to pass them
        // into the function call correctly.
        for (var x = arguments.Length - 1; x >= 0; x--)
        {
            var item = context.EvaluationStack.Pop();
            arguments[x] = item;
        }

        var functionCallString = new StringBuilder();
        functionCallString.Append($"{conversionInfo.NameInC.Value}(");
        for (var x = 0; x < arguments.Length; x++)
        {
            if (x > 0)
            {
                functionCallString.Append(", ");
            }

            functionCallString.Append(arguments[x].Text);
        }

        functionCallString.Append(')');
        
        // If this function call returns void, then nothing will be put on the stack, and therefore
        // we need to write it out now, as nothing will intentionally pop it later. If this doesn't
        // return void, then we need to store the call to the evaluation stack for when it gets
        // read/stored/popped later.
        if (methodDefinition.ReturnType.Name == "void")
        {
            await context.Writer.WriteLineAsync($"\t{functionCallString};");
        }
        else
        {
            context.EvaluationStack.Push(new EvaluationStackItem(functionCallString.ToString()));
        }
    }
}