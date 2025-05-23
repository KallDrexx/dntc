using System.Text;
using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport;

/// <summary>
/// Base type that all reference types will inherit from. Can be used to attach fields and information
/// that all reference types should have (such as reference counting or GC information).
/// </summary>
public class ReferenceTypeBaseDefinedType : CustomDefinedType
{
    public ReferenceTypeBaseDefinedType()
        : base(
        ReferenceTypeConstants.ReferenceTypeBaseId,
        ReferenceTypeConstants.HeaderFileName,
        ReferenceTypeConstants.SourceFileName,
        ReferenceTypeConstants.ReferenceTypeBaseName,
        [],
        [])
    {
    }

    public void AddField(DefinedField field)
    {
        InstanceFields = InstanceFields
            .Concat([new Field(field.IlType, field.IlName)])
            .ToArray();
    }

    public override CustomCodeStatementSet? GetCustomTypeDeclaration(ConversionCatalog catalog)
    {
        var code = new StringBuilder();
        code.AppendLine($"typedef struct {NativeName} {{");
        code.AppendLine($"\tvoid (*{ReferenceTypeConstants.PrepForFreeFnPointerName})(void* object);");

        foreach (var field in InstanceFields)
        {
            var typeInfo = catalog.Find(field.Type);
            var fieldInfo = catalog.Find(field.Id);

            code.AppendLine($"\t{typeInfo.NameInC} {fieldInfo.NameInC};");
        }

        code.AppendLine($"}} {NativeName};");
        return new CustomCodeStatementSet(code.ToString());
    }
}