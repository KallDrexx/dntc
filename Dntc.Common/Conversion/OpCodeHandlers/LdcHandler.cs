using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class LdcHandler : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Ldc_I4, CreateldcI4Fn(null) },
        { Code.Ldc_I4_S, CreateldcI4Fn(null) },
        { Code.Ldc_I4_0, CreateldcI4Fn(0) },
        { Code.Ldc_I4_1, CreateldcI4Fn(1) },
        { Code.Ldc_I4_2, CreateldcI4Fn(2) },
        { Code.Ldc_I4_3, CreateldcI4Fn(3) },
        { Code.Ldc_I4_4, CreateldcI4Fn(4) },
        { Code.Ldc_I4_5, CreateldcI4Fn(5) },
        { Code.Ldc_I4_6, CreateldcI4Fn(6) },
        { Code.Ldc_I4_7, CreateldcI4Fn(7) },
        { Code.Ldc_I4_8, CreateldcI4Fn(8) },
        { Code.Ldc_I4_M1, CreateldcI4Fn(-1) },
    };

    private static OpCodeHandlerFn CreateldcI4Fn(int? hardCodedNumber)
    {
        if (hardCodedNumber == null)
        {
            return context =>
            {
                var number = (sbyte)context.Operand;
                return HandleLdcI4(number, context);
            };
        }

        return context => HandleLdcI4(hardCodedNumber.Value, context);
    }

    private static ValueTask HandleLdcI4(int number, OpCodeHandlingContext context)
    {
        context.EvaluationStack.Push(new EvaluationStackItem(number.ToString()));
        return new ValueTask();
    }
}