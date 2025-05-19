using Dntc.Common.Conversion;
using Dntc.Common.Definitions;
using Dntc.Common.Definitions.Definers;
using Dntc.Common.Definitions.ReferenceTypeSupport;

namespace Dntc.Common;

public class TranspilerContext
{
    public DefinitionGenerationPipeline Definers { get; }
    public ConversionInfoCreator ConversionInfoCreator { get; }
    public DefinitionCatalog DefinitionCatalog { get; }
    public ConversionCatalog ConversionCatalog { get; }

    public TranspilerContext(IMemoryManagementActions memoryManagement)
    {
        Definers = new DefinitionGenerationPipeline(memoryManagement);
        ConversionInfoCreator = new ConversionInfoCreator();
        DefinitionCatalog = new DefinitionCatalog(Definers);
        ConversionCatalog = new ConversionCatalog(DefinitionCatalog, ConversionInfoCreator);
    }
}