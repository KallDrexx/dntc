#ifndef NATIVE_TEST_H
#define NATIVE_TEST_H

#include <stddef.h>
#include <stdint.h>

extern uint32_t static_number;

uint32_t get_number();
void* generic_pointer_return_type_test(size_t size);

struct NativeType
{
    int32_t Value;
};

#endif //NATIVE_TEST_H
