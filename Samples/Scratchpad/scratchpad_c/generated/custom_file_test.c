#include "ScratchpadCSharp.h"
#include <stdint.h>
#include "custom_file_test.h"


ScratchpadCSharp_AttributeTests_CustomFileTestStruct ScratchpadCSharp_AttributeTests_TestStructField = {0};

int32_t ScratchpadCSharp_AttributeTests_CustomFileTestMethod(void) {
	(*(&((&ScratchpadCSharp_AttributeTests_TestStructField)->Value))) = ((*(&((&ScratchpadCSharp_AttributeTests_TestStructField)->Value))) + 1);
	return ((&ScratchpadCSharp_AttributeTests_TestStructField)->Value);
}

