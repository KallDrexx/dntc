using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DotNetDefinedType : DefinedType
{
    private readonly TypeDefinition _definition;
    
    public ModuleDefinition DefinedModule { get; }

    public DotNetDefinedType(TypeDefinition definition)
    {
        _definition = definition;
        DefinedModule = definition.Module;
        ClrName = new ClrTypeName(definition.FullName);

        Fields = definition.Fields
            .Select(x => new Field(new ClrTypeName(x.FieldType.FullName), x.Name))
            .ToArray();

        Methods = definition.Methods
            .Select(x => new ClrMethodId(x.FullName))
            .ToArray();
    }
}