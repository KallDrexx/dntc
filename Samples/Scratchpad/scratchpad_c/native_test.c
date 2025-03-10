#include "native_test.h"

#include <stdlib.h>

uint32_t static_number = 55;

int32_t native_number_array[25] = {10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34};

uint32_t get_number() {
    return 42;
}

void* generic_pointer_return_type_test(const size_t size) {
    return calloc(1, size);
}

void generic_ref_test(int32_t* data, int32_t value) {
    *data = value;
}

typedef int32_t (*get_int)(int, int, int);
int32_t void_pointer_test(void *ptr) {
    const get_int casted = ptr;

    return casted(1, 2, 3);
}
