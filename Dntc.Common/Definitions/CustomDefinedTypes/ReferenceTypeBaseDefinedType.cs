using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.CustomDefinedTypes;

public class ReferenceTypeBaseDefinedType() : CustomDefinedType(TypeName,
    new HeaderName("dntc.h"),
    null,
    new CTypeName("ReferenceType_Base"),
    [TypeInfoDefinedType.TypeName],
    [])
{
    public static readonly IlTypeName TypeName = new IlTypeName("[Platform: ReferenceTypeBase]");

    public override CustomCodeStatementSet? GetCustomTypeDeclaration(ConversionCatalog catalog)
    {
        const string content = @"
typedef struct ReferenceType_Base {
	TypeInfo* type_info;
} ReferenceType_Base;
";
        return new CustomCodeStatementSet(content);
    }
}