using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
using Dntc.Common.Definitions.Definers;

namespace Dntc.Common;

public class TranspilerContext
{
    public DefinitionGenerationPipeline Definers { get; }
    public ConversionInfoCreator ConversionInfoCreator { get; }
    public DefinitionCatalog DefinitionCatalog { get; }
    public ConversionCatalog ConversionCatalog { get; }

    public TranspilerContext()
    {
        Definers = new DefinitionGenerationPipeline();
        ConversionInfoCreator = new ConversionInfoCreator();
        DefinitionCatalog = new DefinitionCatalog(Definers);
        ConversionCatalog = new ConversionCatalog(DefinitionCatalog, ConversionInfoCreator);
    }
}