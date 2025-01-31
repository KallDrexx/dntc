using Dntc.Attributes;
using ScratchpadCSharp.Dependency;

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
    [CustomFieldName("NonHeaderGlobal")]
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

    [NonPointerString]
    public static string? TestGlobalString;

    [NativeFunctionCall("printf", "<stdio.h>")]
    public static void Printf<T>(string template, T input1)
    {
        
    }

    public static void TestNativeGeneric()
    {
        Printf("generic test %d\n", 1234);
    }

    public static void TestNativeGenericInDep()
    {
        GenericUtils.Printf("generic dep test %d\n", 5678);
    }

    public struct CustomFieldNameStruct
    {
        [CustomFieldName("some_value")]
        public int Value;
    }

    public static int GetCustomFieldStructValue(CustomFieldNameStruct obj)
    {
        return obj.Value;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    [InitialGlobalValue("\"abcdefg\"")]
    [StaticallySizedArray(8)]
    public static string StaticallySizedString;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public struct StaticallySizedTest
    {
        [StaticallySizedArray(10)]
        public int[] NumberArray;
    }

    public static int GetFirstNumber(StaticallySizedTest value)
    {
        return value.NumberArray[0];
    }

    [CustomDeclaration("INT_FIELD(custom_declared_global)", "custom_declared_global", "../native_test2.h")]
    [InitialGlobalValue("675")]
    public static int CustomDeclaredGlobalTest;

    public struct CustomDeclaredFieldStruct
    {
        [CustomDeclaration("INT_FIELD(field)", "field", "../native_test2.h")]
        public int SomeField;
    }

    public static int GetCustomDeclaredField(CustomDeclaredFieldStruct fieldStruct)
    {
        return fieldStruct.SomeField;
    }
}