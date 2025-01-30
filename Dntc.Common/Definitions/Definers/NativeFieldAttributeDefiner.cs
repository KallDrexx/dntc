using Dntc.Attributes;
using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Returns a `NativeDefinedGlobal` based on the .net field.
/// </summary>
public class NativeFieldAttributeDefiner : IDotNetFieldDefiner
{
    public DefinedField? Define(FieldDefinition field)
    {
        var attribute = field.CustomAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == typeof(NativeGlobalAttribute).FullName);

        if (attribute == null)
        {
            return null;
        }

        if (attribute.ConstructorArguments.Count < 1)
        {
            var message = $"NativeGlobalAttribute on '{field.FullName}' expected to have at least " +
                          $"1 constructor arguments but none were found";

            throw new InvalidOperationException(message);
        }

        var hasHeaderSpecified = attribute.ConstructorArguments.Count > 1 &&
                                 attribute.ConstructorArguments[1].Value != null;

        var header = hasHeaderSpecified
            ? new HeaderName(attribute.ConstructorArguments[1].Value.ToString()!)
            : (HeaderName?) null;

        return new NativeDefinedField(
            new IlFieldId(field.FullName),
            new IlTypeName(field.FieldType.FullName),
            new CFieldName(attribute.ConstructorArguments[0].Value.ToString()!),
            header);
    }
}