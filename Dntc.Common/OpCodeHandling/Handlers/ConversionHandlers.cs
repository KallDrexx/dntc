using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class ConversionHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Conv_I, new ConversionHandler("int") },
        { Code.Conv_I4, new ConversionHandler("int32_t") },
        { Code.Conv_I8, new ConversionHandler("int64_t") },
        { Code.Conv_R4, new ConversionHandler("float") },
        { Code.Conv_R8, new ConversionHandler("double") },
        { Code.Conv_R_Un, new ConversionHandler("float") },
        { Code.Conv_U, new ConversionHandler("uint") },
        { Code.Conv_U4, new ConversionHandler("uint32_t") },
        { Code.Conv_U8, new ConversionHandler("uint64_t") },

        // MSIL docs say the following should be extended to i32, but I'm not sure if that's
        // needed in a straight C conversion.
        { Code.Conv_I1, new ConversionHandler("int8_t") },
        { Code.Conv_I2, new ConversionHandler("int16_t") },
        { Code.Conv_U1, new ConversionHandler("uint8_t") },
        { Code.Conv_U2, new ConversionHandler("uint16_t") },
    };
    
    private class ConversionHandler(string castTo) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(
            Instruction currentInstruction, 
            ExpressionStack expressionStack,
            MethodConversionInfo currentMethod, 
            ConversionCatalog conversionCatalog)
        {
            var items = expressionStack.Pop(1);
            var item = new DereferencedValueExpression(items[0]);
            var expession = new CastExpression(item, castTo);
            expressionStack.Push(expession);

            return new OpCodeHandlingResult(null);
        }
    }
}