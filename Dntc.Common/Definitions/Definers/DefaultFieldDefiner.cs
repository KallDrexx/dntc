using Mono.Cecil;

namespace Dntc.Common.Definitions.Definers;

public class DefaultFieldDefiner : IDotNetGlobalDefiner
{
    public DefinedGlobal Define(FieldDefinition field)
    {
        return new DotNetDefinedGlobal(field);
    }
}