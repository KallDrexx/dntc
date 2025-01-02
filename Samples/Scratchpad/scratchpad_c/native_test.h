#ifndef NATIVE_TEST_H
#define NATIVE_TEST_H

#include <stdint.h>

extern uint32_t static_number;

uint32_t get_number();

struct NativeType
{
    int32_t Value;
};

#endif //NATIVE_TEST_H
