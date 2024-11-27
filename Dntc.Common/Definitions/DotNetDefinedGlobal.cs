using Mono.Cecil;

namespace Dntc.Common.Definitions;

public class DotNetDefinedGlobal : DefinedGlobal
{
    public FieldDefinition Definition { get; }
    public IlTypeName DeclaringType { get; }
    
    public DotNetDefinedGlobal(FieldDefinition field) 
        : base(new IlFieldId(field.FullName), new IlTypeName(field.FieldType.FullName))
    {
        if (!field.IsStatic)
        {
            var message = $"Cannot define {field.FullName} as a global as it's not static.";
            throw new InvalidOperationException(message);
        }

        Definition = field;
        DeclaringType = new IlTypeName(field.DeclaringType.FullName);
    }
}