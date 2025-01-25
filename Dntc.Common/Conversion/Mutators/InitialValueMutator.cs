using Dntc.Attributes;
using Dntc.Common.Definitions;
using Dntc.Common.Syntax.Expressions;

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

    public void Mutate(FieldConversionInfo conversionInfo, DotNetDefinedField field)
    {
        var attribute = field.Definition
            .CustomAttributes
            .FirstOrDefault(x => x.AttributeType.FullName == typeof(InitialGlobalValueAttribute).FullName);

        if (attribute == null || attribute.ConstructorArguments.Count < 1)
        {
            return;
        }

        var returnType = _conversionCatalog.Find(field.IlType);
        var expression = new LiteralValueExpression(attribute.ConstructorArguments[0].Value.ToString()!, returnType);
        conversionInfo.InitialValue = expression;
    }
}