using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.ReferenceTypeSupport.ReferenceCounting;

/// <summary>
/// Manages incrementing the reference count for a ref counter
/// </summary>
public class ReferenceCountIncrementMethod : CustomDefinedMethod
{
    public ReferenceCountIncrementMethod()
        : base(
            ReferenceCountConstants.IncrementMethodIlName,
            new IlTypeName(typeof(void).FullName!),
            ReferenceCountConstants.IlNamespace,
            ReferenceCountConstants.HeaderFileName,
            ReferenceCountConstants.SourceFileName,
            ReferenceCountConstants.IncrementMethodNativeName,
            [
                new Parameter(ReferenceCountConstants.CounterIlTypeName, "refCounter", true)
            ])
    {
    }

    public override CustomCodeStatementSet? GetCustomDeclaration()
    {
        return new CustomCodeStatementSet(
            $"void ${NativeName}({ReferenceCountConstants.CounterIlTypeName}* refCounter)");
    }

    public override CStatementSet? GetCustomImplementation(ConversionCatalog catalog)
    {
        return new CustomCodeStatementSet($@"
    refCounter->{ReferenceCountConstants.CurrentCountFieldName}++;
");
    }
}