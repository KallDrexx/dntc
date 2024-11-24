#include <stdint.h>
#include "custom_file_test.h"
#include "ScratchpadCSharp.h"



int32_t ScratchpadCSharp_AttributeTests_CustomFileTestMethod() {
	(*(&((&ScratchpadCSharp_AttributeTests_TestStructField)->Value))) = ((*(&((&ScratchpadCSharp_AttributeTests_TestStructField)->Value))) + 1);
	return ((&ScratchpadCSharp_AttributeTests_TestStructField)->Value);
}

