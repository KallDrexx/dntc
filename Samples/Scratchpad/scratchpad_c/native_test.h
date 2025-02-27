#ifndef NATIVE_TEST_H
#define NATIVE_TEST_H

#include <stddef.h>
#include <stdint.h>

extern uint32_t static_number;

uint32_t get_number();
void* generic_pointer_return_type_test(size_t size);
void generic_ref_test(int32_t *data, int32_t value);
int32_t void_pointer_test(void* ptr);

struct NativeType
{
    int32_t Value;
};

#endif //NATIVE_TEST_H
