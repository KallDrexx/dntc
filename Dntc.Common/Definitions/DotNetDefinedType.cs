using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DotNetDefinedType : DefinedType
{
    private readonly TypeDefinition _definition;
    
    public ModuleDefinition DefinedModule { get; }
    public IlNamespace Namespace { get; }

    public DotNetDefinedType(TypeDefinition definition)
    {
        _definition = definition;
        DefinedModule = definition.Module;
        IlName = new IlTypeName(definition.FullName);
        Namespace = new IlNamespace(definition.Namespace);

        Fields = definition.Fields
            .Select(x => new Field(new IlTypeName(x.FieldType.FullName), x.Name))
            .ToArray();

        Methods = definition.Methods
            .Select(x => new IlMethodId(x.FullName))
            .ToArray();
    }
}