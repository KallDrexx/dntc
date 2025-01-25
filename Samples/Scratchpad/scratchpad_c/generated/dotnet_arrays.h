#ifndef DOTNET_ARRAYS_H_H
#define DOTNET_ARRAYS_H_H


#include <stddef.h>
#include <stdint.h>


typedef struct {
    size_t length;
    uint16_t *items;
} SystemUInt16Array;

typedef struct {
    size_t length;
    int32_t *items;
} SystemInt32Array;



#endif // DOTNET_ARRAYS_H_H
