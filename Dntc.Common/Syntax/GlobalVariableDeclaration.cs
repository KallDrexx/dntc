using Dntc.Common.Conversion;
using Dntc.Common.Definitions;

namespace Dntc.Common.Syntax;

public record GlobalVariableDeclaration(
    GlobalConversionInfo Global, 
    TypeConversionInfo Type,
    bool IsExtern)
{
    public async Task WriteAsync(StreamWriter writer)
    {
        await writer.WriteLineAsync($"{(IsExtern ? "extern" : "")} {Type.NameInC} {Global.NameInC};");
    }
}