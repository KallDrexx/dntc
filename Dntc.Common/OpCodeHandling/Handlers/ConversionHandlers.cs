using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Expressions;
using Mono.Cecil.Cil;

namespace Dntc.Common.OpCodeHandling.Handlers;

public class ConversionHandlers : IOpCodeHandlerCollection
{
    public IReadOnlyDictionary<Code, IOpCodeHandler> Handlers { get; } = new Dictionary<Code, IOpCodeHandler>
    {
        { Code.Conv_I, new ConversionHandler(typeof(int)) }, // Probably need some way to support native int
        { Code.Conv_I4, new ConversionHandler(typeof(int)) },
        { Code.Conv_I8, new ConversionHandler(typeof(long)) },
        { Code.Conv_R4, new ConversionHandler(typeof(float)) },
        { Code.Conv_R8, new ConversionHandler(typeof(double)) },
        { Code.Conv_R_Un, new ConversionHandler(typeof(float)) },
        { Code.Conv_U, new ConversionHandler(typeof(uint)) },
        { Code.Conv_U4, new ConversionHandler(typeof(uint)) },
        { Code.Conv_U8, new ConversionHandler(typeof(ulong)) },

        // MSIL docs say the following should be extended to i32, but I'm not sure if that's
        // needed in a straight C conversion.
        { Code.Conv_I1, new ConversionHandler(typeof(sbyte)) },
        { Code.Conv_I2, new ConversionHandler(typeof(short)) },
        { Code.Conv_U1, new ConversionHandler(typeof(byte)) },
        { Code.Conv_U2, new ConversionHandler(typeof(ushort)) },
    };

    private class ConversionHandler(Type castToType) : IOpCodeHandler
    {
        public OpCodeHandlingResult Handle(HandleContext context)
        {
            var typeConversion = context.ConversionCatalog.Find(new IlTypeName(castToType.FullName!));
            var items = context.ExpressionStack.Pop(1);
            var item = new AdjustPointerDepthExpression(items[0], 0);
            var expession = new CastExpression(item, typeConversion);
            context.ExpressionStack.Push(expession);

            return new OpCodeHandlingResult(null);
        }

        public OpCodeAnalysisResult Analyze(AnalyzeContext context)
        {
            return new OpCodeAnalysisResult
            {
                ReferencedTypes = new HashSet<IlTypeName>([new IlTypeName(castToType.FullName!)]),
            };
        }
    }
}