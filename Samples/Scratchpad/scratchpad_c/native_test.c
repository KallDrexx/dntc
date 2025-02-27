#include "native_test.h"

#include <stdlib.h>

uint32_t static_number = 55;

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
