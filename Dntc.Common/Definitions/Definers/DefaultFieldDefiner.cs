using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

public class DefaultFieldDefiner : IDotNetGlobalDefiner
{
    public DefinedField Define(FieldDefinition field)
    {
        return new DotNetDefinedField(field);
    }
}