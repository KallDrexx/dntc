using Dntc.Attributes;
using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

public class NativeTypeDefiner : IDotNetTypeDefiner
{
    public DefinedType? Define(TypeDefinition type)
    {
        var attribute = type.CustomAttributes
            .SingleOrDefault(x => x.AttributeType.FullName == typeof(NativeTypeAttribute).FullName);

        if (attribute == null)
        {
            return null;
        }
        
        if (attribute.ConstructorArguments.Count < 1)
        {
            var message = $"NativeTypeAttribute on '{type.FullName}' expected to have at least " +
                          $"1 constructor arguments but none were found";

            throw new InvalidOperationException(message);
        }

        var hasHeaderSpecified = attribute.ConstructorArguments.Count > 1 &&
                                 attribute.ConstructorArguments[1].Value != null;

        var header = hasHeaderSpecified
            ? new HeaderName(attribute.ConstructorArguments[1].Value.ToString()!)
            : (HeaderName?) null;

        var referenceName = attribute.ConstructorArguments[0].Value.ToString()!;

        var instanceFields = type.Fields
            .Select(x => new DefinedType.Field(new IlTypeName(x.FieldType.FullName), new IlFieldId(x.FullName)))
            .ToArray();
        
        return new NativeDefinedType(new IlTypeName(type.FullName), header, new CTypeName(referenceName), instanceFields);
    }
}