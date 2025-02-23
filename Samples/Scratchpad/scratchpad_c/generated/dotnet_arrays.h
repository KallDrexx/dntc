#ifndef DOTNET_ARRAYS_H_H
#define DOTNET_ARRAYS_H_H


#include <stddef.h>
#include <stdint.h>
#include "ScratchpadCSharp.h"


typedef struct {
    int32_t length;
    uint16_t *items;
} SystemUInt16Array;

typedef struct {
    int32_t length;
    int32_t *items;
} SystemInt32Array;

typedef struct {
    int32_t length;
    ScratchpadCSharp_SimpleFunctions_Vector2 *items;
} ScratchpadCSharpSimpleFunctionsVector2Array;



#endif // DOTNET_ARRAYS_H_H
