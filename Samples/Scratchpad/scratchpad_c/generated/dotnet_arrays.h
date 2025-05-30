#ifndef DOTNET_ARRAYS_H_H
#define DOTNET_ARRAYS_H_H


#include <stddef.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include "dntc.h"
#include "ScratchpadCSharp_ReferenceTypes.h"


typedef struct {
    DntcReferenceTypeBase __reference_type_base;
    int32_t length;
    uint16_t *items;
} SystemUInt16Array;

typedef struct {
    DntcReferenceTypeBase __reference_type_base;
    int32_t length;
    ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayStruct *items;
} ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray;


void _Late bound array name___PrepForFree(ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray* object);

#endif // DOTNET_ARRAYS_H_H
