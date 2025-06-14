using Dntc.Common.Conversion;
using Dntc.Common.Syntax.Statements;

namespace Dntc.Common.Definitions.CustomDefinedMethods;

public class FloatMinDefinedMethod : CustomDefinedMethod
{
    public FloatMinDefinedMethod()
    : base(
        new IlMethodId("System.Single System.Math::Min(System.Single,System.Single)"),
        new IlTypeName("System.Single"),
        new IlNamespace("System"),
        new HeaderName("dotnet_math.h"),
        null,
        new CFunctionName("dn_min_float"),
        [
            new Parameter(new IlTypeName("System.Single"), "first", false, false),
            new Parameter(new IlTypeName("System.Single"), "second", false, false),
        ], false)
    {
    }

    public override CustomCodeStatementSet? GetCustomDeclaration(ConversionCatalog catalog)
    {
        const string content = @"
static float dn_min_float(float first, float second){{
    if (first <= second) return first;
    return second;
}}";
        return new CustomCodeStatementSet(content);
    }

    public override CustomCodeStatementSet? GetCustomImplementation(ConversionCatalog catalog)
    {
        return null;
    }
}