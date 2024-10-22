using Dntc.Attributes;

namespace ScratchpadCSharp;

public static class AttributeTests
{
    public static uint GetNumberMethod() => GetNumber();

    [NativeFunctionCallOnTranspile("get_number", "../native_test.h")]
    private static uint GetNumber()
    {
        return 99;
    }
}