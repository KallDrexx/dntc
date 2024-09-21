using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dntc.Common.Conversion.OpCodeHandlers;

internal class InitObjHandler : IOpCodeFnFactory
{
    public IReadOnlyDictionary<Code, OpCodeHandlerFn> Get() => new Dictionary<Code, OpCodeHandlerFn>
    {
        { Code.Initobj, Handle },
    };

    private static async ValueTask Handle(OpCodeHandlingContext context)
    {
        var typeDefinition = (TypeDefinition)context.Operand;
        var conversionInfo = context.ConversionCatalog.Find(new IlTypeName(typeDefinition.FullName));
        
        var items = context.EvaluationStack.PopCount(1);
        await context.Writer.WriteLineAsync($"\t{items[0].Text} = ({conversionInfo.NameInC.Value}){{0}};");
    }
}