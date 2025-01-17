using Dntc.Common;

namespace ScratchpadCSharp.Plugin;

public class TestPlugin : ITranspilerPlugin
{
    public bool BypassBuiltInNativeDefinitions => false;

    public void Customize(TranspilerContext context)
    {
        context.ConversionInfoCreator.AddGlobalMutator(new Aligned8GlobalMutator());
    }
}