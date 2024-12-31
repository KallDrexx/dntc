using Dntc.Attributes;
using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

/// <summary>
/// Returns a `NativeDefinedGlobal` based on the .net field.
/// </summary>
public class NativeGlobalAttributeDefiner : IDotNetGlobalDefiner
{
    public DefinedGlobal Define(FieldDefinition field)
    {
        var attribute = field.CustomAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == typeof(NativeGlobalAttribute).FullName);

        if (attribute == null)
        {
            var message = $"Field {field.FullName} did not have a `NativeGlobal` attribute on it";
            throw new InvalidOperationException(message);
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

        return new NativeDefinedGlobal(
            new IlFieldId(field.FullName),
            new IlTypeName(field.FieldType.FullName),
            new CGlobalName(attribute.ConstructorArguments[0].Value.ToString()!),
            header);
    }
}