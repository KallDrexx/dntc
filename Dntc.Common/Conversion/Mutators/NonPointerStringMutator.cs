using Dntc.Attributes;
using Dntc.Common.Definitions;

namespace Dntc.Common.Conversion.Mutators;

public class NonPointerStringMutator : IFieldConversionMutator
{
    public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>();

    public void Mutate(FieldConversionInfo conversionInfo, DotNetDefinedField field)
    {
        var hasAttribute = field.Definition
            .CustomAttributes
            .Any(x => x.AttributeType.FullName == typeof(NonPointerStringAttribute).FullName);

        if (!hasAttribute || field.IlType.Value != typeof(string).FullName)
        {
            return;
        }

        conversionInfo.IsNonPointerString = true;
    }
}