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

    [NativeType("struct NativeType", "../native_test.h")]
    public struct RefNativeTestType
    {
        public int Value;
    }

    public static int GetNativeTypeValueRef(ref RefNativeTestType obj)
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

    [InitialGlobalValue("\"abcdefg\"")]
    [StaticallySizedArray(8)]
    public static char[] StaticallySizedString;

    public struct StaticallySizedTest
    {
        [StaticallySizedArray(10)]
        public int[] NumberArray;
    }

    public static int GetFirstNumber(StaticallySizedTest value)
    {
        value.NumberArray[1] = 25;
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

    public static int NonNativeGetNumber(int first, int second, int third)
    {
        return first + second + third;
    }

    [NativeFunctionCall("void_pointer_test", "../native_test.h")]
    public static unsafe int NativeFunctionPointerTest(delegate*<int, int, int, int> pointer)
    {
        return 0;
    }

    public static unsafe int CallNativePointer()
    {
        return NativeFunctionPointerTest(&NonNativeGetNumber);
    }

    [CustomFunction("#define addOneMacro(a) ((a) + 1)", null, "addOneMacro")]
    public static int AddOneMacro(int a)
    {
        return 0;
    }

    [CustomFunction("#define addOneGenericMacro(a) ((a) + 1)", null, "addOneGenericMacro")]
    public static T? AddOneGenericMacro<T>(T a)
    {
        return default;
    }

    [CustomFunction("int32_t addOneFunc(int32_t a)", "return a + 1;", "addOneFunc")]
    public static int AddOneFunction(int a)
    {
        return 0;
    }

    public static int AddTwo(int input)
    {
        return AddOneFunction(AddOneMacro(input));
    }

    public static int AddOneInt(int input)
    {
        return AddOneGenericMacro(input);
    }

    public static uint AddOneUint(uint input)
    {
        return AddOneGenericMacro(input);
    }
}