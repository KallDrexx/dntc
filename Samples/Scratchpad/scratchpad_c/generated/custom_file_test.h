#ifndef CUSTOM_FILE_TEST_H_H
#define CUSTOM_FILE_TEST_H_H


#include <stdint.h>
#include "ScratchpadCSharp.h"

typedef struct {
	int32_t Value;
} ScratchpadCSharp_AttributeTests_CustomFileTestStruct;


extern ScratchpadCSharp_AttributeTests_CustomFileTestStruct ScratchpadCSharp_AttributeTests_TestStructField;

int32_t ScratchpadCSharp_AttributeTests_CustomFileTestMethod(void);

#endif // CUSTOM_FILE_TEST_H_H
