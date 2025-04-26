using Dntc.Attributes;

namespace ScratchpadCSharp;

public static class StringTests
{
    [NativeFunctionCall("printf", "<string.h>")]
    public static void LogSingleString(string value)
    {
    }

    public static void LogStaticString()
    {
        LogSingleString("abcdefg\n");
    }
}