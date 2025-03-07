using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

public class DefaultFieldDefiner : IDotNetFieldDefiner
{
    public DefinedField Define(FieldDefinition field, DefinedType fieldType)
    {
        return new DotNetDefinedField(field, fieldType);
    }
}