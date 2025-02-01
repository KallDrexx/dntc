#include "native_test.h"

#include <stdlib.h>

uint32_t static_number = 55;

uint32_t get_number() {
    return 42;
}

void* generic_pointer_return_type_test(const size_t size) {
    return calloc(1, size);
}
