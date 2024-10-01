using Dntc.Common.Conversion;

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
            new Parameter(new IlTypeName("System.Single"), "first", false),
            new Parameter(new IlTypeName("System.Single"), "second", false),
        ])
    {
    }

    public override async ValueTask WriteHeaderContentsAsync(StreamWriter writer)
    {
        await writer.WriteLineAsync("float dn_min_float(float first, float second) {");
        await writer.WriteLineAsync("\tif (first <= second) return first;");
        await writer.WriteLineAsync("\t return second;");
        await writer.WriteLineAsync("}");
    }

    public override ValueTask WriteSourceFileContentsAsync(StreamWriter writer)
    {
        // Header only for now
        return new ValueTask();
    }
}