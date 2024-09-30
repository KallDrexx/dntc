using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class StLocOpHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Stloc, CreateFn(null) },
        { Code.Stloc_0, CreateFn(0) },
        { Code.Stloc_1, CreateFn(1) },
        { Code.Stloc_2, CreateFn(2) },
        { Code.Stloc_3, CreateFn(3) },
    };

    private static OpCodeHandlerFn CreateFn(int? hardCodedIndex)
    {
        if (hardCodedIndex == null)
        {
            return context =>
            {
                if (context.Operand is not int index)
                {
                    var message = $"Expected stloc operand of int, instead was '{context.Operand?.GetType().FullName}'";
                    throw new ArgumentException(message);
                }

                return HandleStore(index, context);
            };
        }

        return context => HandleStore(hardCodedIndex.Value, context);
    }
    
    private static async ValueTask HandleStore(int localIndex, OpCodeHandlingContext context)
    {
        var items = context.EvaluationStack.PopCount(1);
        
        var local = context.Variables.Locals[localIndex];
        await context.Writer.WriteAsync($"\t{local.Name} = ");

        var text = (local.IsPointer, items[0].IsPointer) switch
        {
            (true, true) => items[0].RawText,
            (true, false) => items[0].ReferenceTo,
            (false, true) => items[0].Dereferenced,
            (false, false) => items[0].RawText,
        };

        await context.Writer.WriteLineAsync($"{text};");
    }
}