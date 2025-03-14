#include <assert.h>
#include <stdio.h>
#include "generated/fn_pointer_types.h"
#include "generated/ScratchpadCSharp.h"
#include "generated/dntc_utils.h"
#include "native_test.h"

#define ARRAY_ITEM_COUNT (10)

int main(void) {
    dntc_utils_init_static_constructors();

    ScratchpadCSharp_SimpleFunctions_Vector3 first = {.X = 1, .Y = 2, .Z = 3};
    ScratchpadCSharp_SimpleFunctions_Vector3 second = {.X = 4, .Y = 5, .Z = 6};
    ScratchpadCSharp_SimpleFunctions_Vector3 result = ScratchpadCSharp_SimpleFunctions_StructOpOverload(first, second);

    assert(result.X == 5.0);
    assert(result.Y == 7.0);
    assert(result.Z == 9.0);

    float someFloat = 10;
    ScratchpadCSharp_SimpleFunctions_Vector3 ctorVec = ScratchpadCSharp_SimpleFunctions_ConstructorTest(8, 9, 10);

    assert(ctorVec.X == 8.0);
    assert(ctorVec.Y == 9.0);
    assert(ctorVec.Z == 10.0);

    uint16_t *items = malloc(sizeof(uint16_t) * ARRAY_ITEM_COUNT);
    SystemUInt16Array array = {.length = ARRAY_ITEM_COUNT, .items = items };
    uint16_t arraySum = ScratchpadCSharp_SimpleFunctions_ArrayTest(array);

    assert(array.items[3] == 3);
    assert(array.items[8] == 8);
    assert(arraySum == 45);

    ScratchpadCSharp_SimpleFunctions_RefTest(&first, &someFloat, 5);

    assert(first.X == 6);
    assert(first.Y == 7);
    assert(first.Z == 8);
    assert(someFloat == 15);

    float dotResult = ScratchpadCSharp_SimpleFunctions_StructInstanceTest(first, second);

    assert(dotResult == 107);

    int32_t swapX = 15;
    int32_t swapY = 20;
    int32_t swapResult = ScratchpadCSharp_SimpleFunctions_SwapTest(swapX, swapY);

    assert(swapResult == 15);

    float sqrtTest = ScratchpadCSharp_SimpleFunctions_SquareRootTest(25);
    assert(sqrtTest == 5.0);

    float localSwapTest = ScratchpadCSharp_SimpleFunctions_LocalSwapTest(5, 10);
    assert(localSwapTest == 5);

    int32_t ternaryTest1 = ScratchpadCSharp_SimpleFunctions_TernaryTest(5, 6);
    int32_t ternaryTest2 = ScratchpadCSharp_SimpleFunctions_TernaryTest(8, 5);
    assert(ternaryTest1 == 4);
    assert(ternaryTest2 == 6);

    int32_t genericStaticTest = ScratchpadCSharp_GenericTests_GetStaticNumber(5);
    int32_t genericAddedTest = ScratchpadCSharp_GenericTests_GetAddedNumber(8, 10);
    assert(genericStaticTest == 5);
    assert(genericAddedTest == 18);

    uint32_t nativeMethodTest = ScratchpadCSharp_AttributeTests_GetNumberMethod();
    assert(nativeMethodTest == 42);

    ScratchpadCSharp_SimpleFunctions_SomeStaticInt = 5;
    uint32_t staticTest1 = ScratchpadCSharp_SimpleFunctions_IncrementStaticInt();
    assert(staticTest1 == 6);

    staticTest1 = ScratchpadCSharp_SimpleFunctions_IncrementStaticInt();
    assert(staticTest1 == 7);

    ScratchpadCSharp_SimpleFunctions_Vector3 staticVector = ScratchpadCSharp_SimpleFunctions_GetStaticVector();
    assert(staticVector.X == 10);
    assert(staticVector.Y == 11);
    assert(staticVector.Z == 12);

    uint32_t staticNumber1 = ScratchpadCSharp_AttributeTests_GetStaticNumberField();
    assert(staticNumber1 == 55);

    ScratchpadCSharp_AttributeTests_SetStaticNumberField(23);
    assert(static_number == 23);

    uint32_t nonHeaderField = ScratchpadCSharp_AttributeTests_GetNonHeaderStructValue();
    assert(nonHeaderField == 1020);

    int32_t renamedFunctionValue = some_named_function();
    assert(renamedFunctionValue == 94);

    int32_t declareTest = ScratchpadCSharp_AttributeTests_ReferToCustomDeclaredMethod();
    assert(declareTest == 929);

    int32_t inlineTest = ScratchpadCSharp_AttributeTests_InlineTest();
    assert(inlineTest == 42);

    struct NativeType nativeType = {.Value = 65};
    int32_t nativeTypeValue1 = ScratchpadCSharp_AttributeTests_GetNativeTypeValue(nativeType);
    assert(nativeTypeValue1 == 65);

    int32_t nativeTypeValue2 = ScratchpadCSharp_AttributeTests_GetNativeTypeValueRef(&nativeType);
    assert(nativeTypeValue2 == 65);

    assert(ScratchpadCSharp_AttributeTests_UnreferencedGlobalField == 123);

    int32_t genericDepNum = ScratchpadCSharp_GenericTests_GetGenericNumberFromDep(25);
    assert(genericDepNum == 25);

    ScratchpadCSharp_StringTests_LogStaticString();
    ScratchpadCSharp_AttributeTests_TestNativeGeneric();
    ScratchpadCSharp_AttributeTests_TestNativeGenericInDep();

    printf("Statically Sized String: %s\n", ScratchpadCSharp_AttributeTests_StaticallySizedString);

    int32_t *tempItems = malloc(sizeof(int32_t) * ARRAY_ITEM_COUNT);
    for (int x = 0; x < ARRAY_ITEM_COUNT; x++)
    {
        tempItems[x] = ARRAY_ITEM_COUNT - x;
    }
    
    ScratchpadCSharp_AttributeTests_StaticallySizedTest test = {
        .NumberArray = {1,2,3,4,5,6,7,8,9,10},
        .NumberArray2 = {10,20,30,40,50,60,70,80,90,100},
    };
    int32_t arrayFirstNumber = ScratchpadCSharp_AttributeTests_GetFirstNumber(test);
    assert(arrayFirstNumber == 1);

    int32_t arrayFirstNumber2 = ScratchpadCSharp_AttributeTests_GetFirstNumberNoBoundsCheck(test);
    assert(arrayFirstNumber2 == 10);

    // Verify the macros are working by utilizing them, they'll fail to compile if they don't get
    // their custom declarations used.
    custom_declared_global = 5;
    ScratchpadCSharp_AttributeTests_CustomDeclaredFieldStruct declaredTest = {0};
    declaredTest.field = 25;

    int32_t customDeclaredFieldTest = ScratchpadCSharp_AttributeTests_GetCustomDeclaredField(declaredTest);
    assert(customDeclaredFieldTest == 25);

    int32_t* newInt = ScratchpadCSharp_GenericTests_GenericPointerTest();
    assert(*newInt == 25);

    bool pointerCheckResult = ScratchpadCSharp_GenericTests_PointerNullCheck();
    assert(pointerCheckResult == false);

    int32_t pointerCheck2 = ScratchpadCSharp_GenericTests_PointerNullCheck2();
    assert(pointerCheck2 == 2);

    FnPtr_Returns_Int32 retrievedFnPointer = ScratchpadCSharp_SimpleFunctions_GetFunctionPointer();
    int32_t fnPointerResult = ScratchpadCSharp_SimpleFunctions_RunFunctionPointer(retrievedFnPointer);
    assert(fnPointerResult == 1);

    ScratchpadCSharp_SimpleFunctions_SetOtherAssemblyFieldValue(63);
    int32_t otherAssemblyFieldValue = ScratchpadCSharp_SimpleFunctions_GetOtherAssemblyFieldValue();
    assert(otherAssemblyFieldValue == 63);

    int32_t nativePointerTest = ScratchpadCSharp_AttributeTests_CallNativePointer();
    assert(nativePointerTest == 6);

    int32_t customFunctionValue = ScratchpadCSharp_AttributeTests_AddTwo(12);
    assert(customFunctionValue == 14);

    int32_t customFuncIntValue = ScratchpadCSharp_AttributeTests_AddOneInt(14);
    assert(customFuncIntValue == 15);

    uint32_t customFuncUintValue = ScratchpadCSharp_AttributeTests_AddOneUint(15);
    assert(customFuncUintValue == 16);

    int32_t nativeArrayNoBoundsCheckValue = ScratchpadCSharp_AttributeTests_GetFirstNativeArrayNoBoundsCheckValue();
    assert(nativeArrayNoBoundsCheckValue == 10);

    ScratchpadCSharp_EnumTests_SetEnumValue(5);
    int32_t enumValue = ScratchpadCSharp_EnumTests_GetTestEnumValue();
    assert(enumValue == 5);

    printf("Tests passed!\n");
    return 0;
}
