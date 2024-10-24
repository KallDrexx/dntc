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
        
        // Nested types don't have a namespace on them, so we need to go to the root
        var rootDeclaringType = definition;
        while (rootDeclaringType.DeclaringType != null)
        {
            rootDeclaringType = rootDeclaringType.DeclaringType;
        }
        Namespace = new IlNamespace(rootDeclaringType.Namespace);

        Fields = definition.Fields
            .Select(x => new Field(new IlTypeName(x.FieldType.FullName), x.Name, x.IsStatic))
            .ToArray();

        Methods = definition.Methods
            .Select(x => new IlMethodId(x.FullName))
            .ToArray();
    }
}