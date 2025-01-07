using Dntc.Attributes;

namespace ScratchpadCSharp;

public static class AttributeTests
{
    public static uint GetNumberMethod() => GetNumber();

    [NativeFunctionCall("get_number", "<stdlib.h>,../native_test.h")]
    private static uint GetNumber()
    {
        return 99;
    }

    [NativeGlobal("static_number", "../native_test.h")]
    public static uint StaticNumberField;
    
    public static uint GetStaticNumberField()
    {
        return StaticNumberField;
    }

    public static void SetStaticNumberField(uint num)
    {
        StaticNumberField = num;
    }

    [CustomFunctionName("some_named_function")]
    public static int RenamedMethodTest()
    {
        return 94;
    }

    [CustomFileName("custom_file_test.c", "custom_file_test.h")]
    public struct CustomFileTestStruct
    {
        public int Value;
    }

    [CustomFileName("custom_file_test.c", "custom_file_test.h")]
    public static CustomFileTestStruct TestStructField;

    [CustomFileName("custom_file_test.c", "custom_file_test.h")]
    public static int CustomFileTestMethod()
    {
        TestStructField.Value += 1;
        return TestStructField.Value;
    }

    public static int CustomFileReferenceTestMethod()
    {
        return TestStructField.Value;
    }

    [IgnoreInHeader]
    [CustomDeclaration("DECLARE_TEST(custom_declared_method)", "custom_declared_method", "../macros.h")]
    public static int CustomDeclaredMethod()
    {
        return 929;
    }

    public static int ReferToCustomDeclaredMethod()
    {
        return CustomDeclaredMethod();
    }
   
    [IgnoreInHeader]
    public struct NonHeaderStruct
    {
        public int Value;
    }

    [IgnoreInHeader]
    [CustomGlobalName("NonHeaderGlobal")]
    public static NonHeaderStruct NonHeaderStructGlobal = new() { Value = 1020 };

    public static int GetNonHeaderStructValue()
    {
        return NonHeaderStructGlobal.Value;
    }

    [WithCAttribute("__attribute__ ((always_inline))")]
    public static int InlineTest()
    {
        return 42;
    }

    [NativeType("struct NativeType", "../native_test.h")]
    public struct NativeTestType
    {
        public int Value;
    }

    public static int GetNativeTypeValue(NativeTestType obj)
    {
        return obj.Value;
    }

    public static int GetNativeTypeValueRef(ref NativeTestType obj)
    {
        return obj.Value;
    }

    [InitialGlobalValue("123")]
    [WithCAttribute("__attribute__ ((aligned (16)))")]
    public static int UnreferencedGlobalField;
}