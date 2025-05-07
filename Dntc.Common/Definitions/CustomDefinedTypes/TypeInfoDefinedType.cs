using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.CustomDefinedTypes;

public class TypeInfoDefinedType() : CustomDefinedType(TypeName,
    new HeaderName("dntc.h"),
    null,
    new CTypeName("TypeInfo"),
    [],
    [])
{
    public static readonly IlTypeName TypeName = new IlTypeName("[Platform: TypeInfo]");

    public override CustomCodeStatementSet? GetCustomTypeDeclaration(ConversionCatalog catalog)
    {
        const string content = @"
typedef struct TypeInfo {
    uint32_t* implemented_interfaces; // Array of implemented interfaces
	size_t* interface_offsets; // Array of offsets for each interface
	size_t interface_count; // Count of implemented interfaces
} TypeInfo;
";
        
        return new CustomCodeStatementSet(content);
    }
}