using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

public class DefaultFieldDefiner : IDotNetFieldDefiner
{
    public DefinedField Define(FieldDefinition field)
    {
        return new DotNetDefinedField(field);
    }
}