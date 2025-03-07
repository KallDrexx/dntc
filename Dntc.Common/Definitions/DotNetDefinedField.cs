using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DotNetDefinedField : DefinedField
{
    public FieldDefinition Definition { get; }
    public IlTypeName DeclaringType { get; }
    
    public DotNetDefinedField(FieldDefinition field, DefinedType fieldType)
        : base(new IlFieldId(field.FullName), fieldType, field.IsStatic)
    {
        Definition = field;
        DeclaringType = new IlTypeName(field.DeclaringType.FullName);
    }
}