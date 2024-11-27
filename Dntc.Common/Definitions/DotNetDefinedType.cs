using Dntc.Common.OpCodeHandling;
using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DotNetDefinedType : DefinedType
{
    public TypeDefinition Definition { get; }
    
    public ModuleDefinition DefinedModule { get; }
    public IlNamespace Namespace { get; }

    public DotNetDefinedType(TypeDefinition definition)
    {
        Definition = definition;
        DefinedModule = definition.Module;
        IlName = new IlTypeName(definition.FullName);
        
        // Nested types don't have a namespace on them, so we need to go to the root
        var rootDeclaringType = definition;
        while (rootDeclaringType.DeclaringType != null)
        {
            rootDeclaringType = rootDeclaringType.DeclaringType;
        }
        Namespace = new IlNamespace(rootDeclaringType.Namespace);

        InstanceFields = definition.Fields
            .Where(x => !x.IsStatic)
            .Select(ConvertToField)
            .ToArray();

        Methods = definition.Methods
            .Select(x => new IlMethodId(x.FullName))
            .ToArray();
    }

    private static Field ConvertToField(FieldDefinition fieldDefinition)
    {
        var type = new IlTypeName(fieldDefinition.FieldType.FullName);
        var name = fieldDefinition.Name;

        return new Field(type, name);
    }
}