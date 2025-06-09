using Dntc.Attributes;
using Dntc.Common.Syntax.Expressions;
using Mono.Cecil;

namespace Dntc.Common.Conversion.Mutators;

public class InitialValueMutator : IFieldConversionMutator
{
    // This causes an annoying almost circular dependency. I need to think through a better
    // graph for this.
    private readonly ConversionCatalog _conversionCatalog;

    public InitialValueMutator(ConversionCatalog conversionCatalog)
    {
        _conversionCatalog = conversionCatalog;
    }

    public IReadOnlySet<IlTypeName> RequiredTypes => new HashSet<IlTypeName>();

    public void Mutate(FieldConversionInfo conversionInfo, FieldDefinition field)
    {
        var attribute = field
            .CustomAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == typeof(InitialGlobalValueAttribute).FullName);

        if (attribute == null || attribute.ConstructorArguments.Count < 1)
        {
            return;
        }

        var returnType = _conversionCatalog.Find(conversionInfo.FieldTypeConversionInfo.IlName);
        var expression = new LiteralValueExpression(attribute.ConstructorArguments[0].Value.ToString()!, returnType, 0);
        conversionInfo.InitialValue = expression;
    }
}