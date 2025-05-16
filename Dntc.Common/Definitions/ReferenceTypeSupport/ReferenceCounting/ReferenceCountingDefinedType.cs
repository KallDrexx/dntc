using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport.ReferenceCounting;

/// <summary>
/// Type that tracks reference count information. Does not support cyclic references, nor is it thread safe.
/// </summary>
public class ReferenceCountingDefinedType : CustomDefinedType
{
    public ReferenceCountingDefinedType()
        : base(
            new IlTypeName(ReferenceCountConstants.CounterIlTypeName),
            new HeaderName(ReferenceCountConstants.HeaderFileName),
            new CSourceFileName(ReferenceCountConstants.SourceFileName),
            new CTypeName(ReferenceCountConstants.CounterTypeNativeName),
            [
                new IlTypeName(typeof(int).FullName!),
            ],
            [])
    {
    }

    public override CustomCodeStatementSet? GetCustomTypeDeclaration(ConversionCatalog catalog)
    {
        return new CustomCodeStatementSet($@"
typedef struct {NativeName} {{
    int {ReferenceCountConstants.CountFieldName};
}} {NativeName};
");
    }
}