using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class LdcHandlers : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Ldc_I4, CreateLdcI4Fn(null) },
        { Code.Ldc_I4_S, CreateLdcI4Fn(null) },
        { Code.Ldc_I4_0, CreateLdcI4Fn(0) },
        { Code.Ldc_I4_1, CreateLdcI4Fn(1) },
        { Code.Ldc_I4_2, CreateLdcI4Fn(2) },
        { Code.Ldc_I4_3, CreateLdcI4Fn(3) },
        { Code.Ldc_I4_4, CreateLdcI4Fn(4) },
        { Code.Ldc_I4_5, CreateLdcI4Fn(5) },
        { Code.Ldc_I4_6, CreateLdcI4Fn(6) },
        { Code.Ldc_I4_7, CreateLdcI4Fn(7) },
        { Code.Ldc_I4_8, CreateLdcI4Fn(8) },
        { Code.Ldc_I4_M1, CreateLdcI4Fn(-1) },
        
        { Code.Ldc_R4, HandleLdcReal },
        { Code.Ldc_R8, HandleLdcReal },
    };

    private static OpCodeHandlerFn CreateLdcI4Fn(int? hardCodedNumber)
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
        context.EvaluationStack.Push(new EvaluationStackItem(number.ToString(), false));
        return new ValueTask();
    }

    private static ValueTask HandleLdcReal(OpCodeHandlingContext context)
    {
        context.EvaluationStack.Push(new EvaluationStackItem(context.Operand.ToString()!, false));
        return new ValueTask();
    }
}