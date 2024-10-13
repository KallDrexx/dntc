#include <stdio.h>
#include "generated/fn_pointer_types.h"
#include "generated/ScratchpadCSharp.h"

int main(void) {
    ScratchpadCSharp_SimpleFunctions_Vector3 first = {.X = 1, .Y = 2, .Z = 3};
    ScratchpadCSharp_SimpleFunctions_Vector3 second = {.X = 4, .Y = 5, .Z = 6};
    ScratchpadCSharp_SimpleFunctions_Vector3 result = ScratchpadCSharp_SimpleFunctions_StructOpOverload(first, second);
    float someFloat = 10;
    ScratchpadCSharp_SimpleFunctions_Vector3 ctorVec = ScratchpadCSharp_SimpleFunctions_ConstructorTest(8, 9, 10);

    SystemUInt16Array *array = malloc(sizeof(SystemUInt16Array) + sizeof(uint16_t) * 10);
    array->length = 10;
    ScratchpadCSharp_SimpleFunctions_ArrayTest((*array));
    ScratchpadCSharp_SimpleFunctions_RefTest(&first, &someFloat, 5);
    float dotResult = ScratchpadCSharp_SimpleFunctions_StructInstanceTest(first, second);

    printf("Hello, World! (%f, %f, %f)\n", result.X, result.Y, result.Z);
    printf("sqrt test: %f\n", ScratchpadCSharp_SimpleFunctions_SquareRootTest(25));
    printf("array test: %d %d\n", array->items[3], array->items[8]);
    printf("ref test: {%f, %f, %f) and %f\n", first.X, first.Y, first.Z, someFloat);
    printf("instance test: %f\n", dotResult);
    printf("Ctor vec: (%f, %f, %f)\n", ctorVec.X, ctorVec.Y, ctorVec.Z);
    return 0;
}
