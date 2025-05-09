using Dntc.Common.Conversion;
using Dntc.Common.Definitions.CustomDefinedTypes;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.CustomDefinedMethods;

public class DynamicCastInterfaceDefinedMethod() : CustomDefinedMethod(MethodId,
    ReferenceTypeBaseDefinedType.TypeName,
    new IlNamespace("DntC.Platform.Utils"),
    new HeaderName("dntc.h"),
    null,
    new CFunctionName("dynamic_cast_interface"),
    [
        new Parameter(ReferenceTypeBaseDefinedType.TypeName, "instance", true),
        new Parameter(NativeDefinedType.StandardTypes[typeof(uint)].IlName, "interface", false),
    ], false)
{
    public static readonly IlMethodId MethodId = new("[Util: DynamicCastInterface]");

    protected override IReadOnlyList<IlTypeName> GetReferencedTypesInternal()
    {
        return [TypeInfoDefinedType.TypeName];
    }

    public override CustomCodeStatementSet? GetCustomDeclaration()
    {
        const string content = @"
static ReferenceType_Base* dynamic_cast_interface(ReferenceType_Base* instance, uint32_t interface) {
    if (instance && instance->type_info) {
        TypeInfo* type_info = instance->type_info;
        for (size_t i = 0; i < type_info->interface_count; ++i) {
            if (type_info->implemented_interfaces[i] == interface) {
                // Calculate the address of the interface implementation using the offset
                return (ReferenceType_Base*)((char*)instance + type_info->interface_offsets[i]);
            }
        }
    }
    return NULL; // Interface is not implemented
}
";
        return new CustomCodeStatementSet(content);
    }

    public override CustomCodeStatementSet? GetCustomImplementation(ConversionCatalog catalog)
    {
        return null;
    }
}