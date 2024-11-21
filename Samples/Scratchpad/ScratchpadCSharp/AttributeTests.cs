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

    [NativeGlobalOnTranspile("static_number", "../native_test.h")]
    public static uint StaticNumberField;
    
    public static uint GetStaticNumberField()
    {
        return StaticNumberField;
    }

    public static void SetStaticNumberField(uint num)
    {
        StaticNumberField = num;
    }

    [CustomNameOnTranspile("some_named_function")]
    public static int RenamedMethodTest()
    {
        return 94;
    }
}