using Dntc.Common.Definitions.ReferenceTypeSupport.ReferenceCounting;
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

        if (!definition.IsValueType)
        {
            // If this is a class reference type, we need to add the parent classes to its inheritance chain
            if (definition.BaseType != null &&
                definition.BaseType.FullName != typeof(object).FullName &&
                definition.BaseType.FullName != typeof(ValueType).FullName &&
                definition.BaseType.FullName != typeof(Enum).FullName)
            {
                var baseTypeName = new IlTypeName(definition.BaseType.FullName);

                OtherReferencedTypes = [baseTypeName];
            }
            else
            {
                // It has no parent, so we need to ensure it references the reference counter
                InstanceFields = new List<Field>(
                [
                    new Field(ReferenceCountConstants.CounterIlTypeName, ReferenceCountConstants.CounterIlFieldId)
                ])
                    .Concat(InstanceFields)
                    .ToArray();
            }
        }

        Methods = definition.Methods
            .Select(x => new IlMethodId(x.FullName))
            .ToArray();
    }

    private static Field ConvertToField(FieldDefinition fieldDefinition)
    {
        var type = new IlTypeName(fieldDefinition.FieldType.FullName);

        return new Field(type, new IlFieldId(fieldDefinition.FullName));
    }
}