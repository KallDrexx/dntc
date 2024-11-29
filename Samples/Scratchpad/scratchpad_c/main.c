#include <assert.h>
#include <stdio.h>
#include "generated/fn_pointer_types.h"
#include "generated/ScratchpadCSharp.h"
#include "generated/dntc_utils.h"

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
    ScratchpadCSharp_SimpleFunctions_ArrayTest(array);

    assert(array.items[3] == 3);
    assert(array.items[8] == 8);

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

    int32_t renamedFunctionValue = some_named_function();
    assert(renamedFunctionValue == 94);

    int32_t declareTest = ScratchpadCSharp_AttributeTests_ReferToCustomDeclaredMethod();
    assert(declareTest == 929);

    printf("Tests passed!\n");
    return 0;
}
