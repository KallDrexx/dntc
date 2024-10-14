#include <assert.h>
#include <stdio.h>
#include "generated/fn_pointer_types.h"
#include "generated/ScratchpadCSharp.h"

#define ARRAY_ITEM_COUNT (10)

int main(void) {
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
    assert(localSwapTest == 10);

    printf("Tests passed!\n");
    return 0;
}
