#include "custom_file_test.h"
#include "ScratchpadCSharp.h"
#include <stdint.h>

ScratchpadCSharp_AttributeTests_CustomFileTestStruct ScratchpadCSharp_AttributeTests_TestStructField = {0};

int32_t ScratchpadCSharp_AttributeTests_CustomFileTestMethod(void) {
	int32_t __return_value = {0};
	(*(&((&ScratchpadCSharp_AttributeTests_TestStructField)->Value))) = ((*(&((&ScratchpadCSharp_AttributeTests_TestStructField)->Value))) + 1);
	__return_value = ((&ScratchpadCSharp_AttributeTests_TestStructField)->Value);
	return __return_value;
}
