using Dntc.Common;
using Dntc.Common.Conversion;
using Dntc.Common.Conversion.Mutators;
using Mono.Cecil;

namespace ScratchpadCSharp.Plugin;

public class NonInitializedGlobalMutator : IFieldConversionMutator
{
    public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>();

    public void Mutate(FieldConversionInfo conversionInfo, FieldDefinition fieldDefinition)
    {
        if (fieldDefinition.Name.EndsWith("GlobalWithNoInitialValue"))
        {
            conversionInfo.HasNoInitialValue = true;
        }
    }
}