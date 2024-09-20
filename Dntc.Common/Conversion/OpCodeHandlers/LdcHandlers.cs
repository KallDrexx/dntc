using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class LdcHandlers : IOpCodeFnFactory
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
        
        { Code.Ldc_R4, HandleLdcR4 },
    };

    private static OpCodeHandlerFn CreateldcI4Fn(int? hardCodedNumber)
    {
        if (hardCodedNumber == null)
        {
            return context =>
            {
                switch (context.Operand)
                {
                    case sbyte sbyteValue:
                        return HandleLdcI4(sbyteValue, context);
                    
                    case byte byteValue:
                        return HandleLdcI4(byteValue, context);
                    
                    case short shortValue:
                        return HandleLdcI4(shortValue, context);
                    
                    case ushort ushortValue:
                        return HandleLdcI4(ushortValue, context);
                    
                    case int intValue:
                        return HandleLdcI4(intValue, context);
                    
                    default:
                        throw new NotSupportedException(context.Operand.GetType().FullName);
                }
            };
        }

        return context => HandleLdcI4(hardCodedNumber.Value, context);
    }

    private static ValueTask HandleLdcI4(int number, OpCodeHandlingContext context)
    {
        context.EvaluationStack.Push(new EvaluationStackItem(number.ToString()));
        return new ValueTask();
    }

    private static ValueTask HandleLdcR4(OpCodeHandlingContext context)
    {
        context.EvaluationStack.Push(new EvaluationStackItem(context.Operand.ToString()));
        return new ValueTask();
    }
}