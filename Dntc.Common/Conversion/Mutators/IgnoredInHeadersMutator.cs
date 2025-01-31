using Dntc.Attributes;
using Dntc.Common.Definitions;
using Mono.Cecil;

namespace Dntc.Common.Conversion.Mutators;

public class IgnoredInHeadersMutator : ITypeConversionMutator, IMethodConversionMutator, IFieldConversionMutator
{
    public void Mutate(TypeConversionInfo conversionInfo, DotNetDefinedType type)
    {
        var ignoredInHeader = type.Definition
            .CustomAttributes
            .Any(x => x.AttributeType.FullName == typeof(IgnoreInHeaderAttribute).FullName);

        if (!ignoredInHeader || conversionInfo.Header == null || conversionInfo.IsPredeclared)
        {
            return;
        }

        conversionInfo.SourceFileName = Utils.ToSourceFileName(conversionInfo.Header.Value);
        conversionInfo.Header = null;
    }

    public void Mutate(MethodConversionInfo conversionInfo, DotNetDefinedMethod method)
    {
        var ignoredInHeader = method.Definition
            .CustomAttributes
            .Any(x => x.AttributeType.FullName == typeof(IgnoreInHeaderAttribute).FullName);

        if (!ignoredInHeader || conversionInfo.Header == null || conversionInfo.IsPredeclared)
        {
            return;
        }

        conversionInfo.Header = null;
    }

    public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>();

    public void Mutate(FieldConversionInfo conversionInfo, FieldDefinition field)
    {
        var ignoredInHeader = field
            .CustomAttributes
            .Any(x => x.AttributeType.FullName == typeof(IgnoreInHeaderAttribute).FullName);

        if (!ignoredInHeader || conversionInfo.Header == null || conversionInfo.IsPredeclared)
        {
            return;
        }

        conversionInfo.Header = null;
    }
}