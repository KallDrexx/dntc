namespace Dntc.Common.Syntax;

public record HeaderGuard(HeaderName HeaderName)
{
    public async Task WriteStart(StreamWriter writer)
    {
        await writer.WriteLineAsync($"#ifndef {GuardName}");
        await writer.WriteLineAsync($"#define {GuardName}");
        await writer.WriteLineAsync();
    }

    public async Task WriteEnd(StreamWriter writer)
    {
        await writer.WriteLineAsync($"#endif // {GuardName}");
    }

    private string GuardName => HeaderName.Value.ToUpper().Replace('.', '_') + "_H";
}