using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DotNetDefinedField : DefinedField
{
    public FieldDefinition Definition { get; }
    public IlTypeName DeclaringType { get; }
    
    public DotNetDefinedField(FieldDefinition field) 
        : base(new IlFieldId(field.FullName), new IlTypeName(field.FieldType.FullName), field.IsStatic)
    {
        Definition = field;
        DeclaringType = new IlTypeName(field.DeclaringType.FullName);
    }
}