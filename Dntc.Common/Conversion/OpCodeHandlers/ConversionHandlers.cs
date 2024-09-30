using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class ConversionHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Conv_I, context => PushCast("int", context) },
        { Code.Conv_I4, context => PushCast("int32_t", context) },
        { Code.Conv_I8, context => PushCast("int64_t", context) },
        { Code.Conv_R4, context => PushCast("float", context) },
        { Code.Conv_R8, context => PushCast("double", context) },
        { Code.Conv_R_Un, context => PushCast("float", context) },
        { Code.Conv_U, context => PushCast("uint", context) },
        { Code.Conv_U4, context => PushCast("uint32_t", context) },
        { Code.Conv_U8, context => PushCast("uint64_t", context) },
        
        // MSIL docs say the following should be extended to i32, but I'm not sure if that's
        // needed in a straight C conversion.
        { Code.Conv_I1, context => PushCast("int8_t", context) }, 
        { Code.Conv_I2, context => PushCast("int16_t", context) }, 
        { Code.Conv_U1, context => PushCast("uint8_t", context) },
        { Code.Conv_U2, context => PushCast("uint16_t", context) },
    };

    private static ValueTask PushCast(string castString, OpCodeHandlingContext context)
    {
        var items = context.EvaluationStack.PopCount(1);
        var newItem = new EvaluationStackItem($"(({castString}){items[0].Dereferenced})", false);
        context.EvaluationStack.Push(newItem);

        return new ValueTask();
    }
}