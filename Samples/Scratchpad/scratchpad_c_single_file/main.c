#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include "header_test2.h"
#include "header_test1.h"

typedef struct ScratchpadCSharp_SingleFileTests_SomeStruct {
	int32_t Value;
} ScratchpadCSharp_SingleFileTests_SomeStruct;

int32_t ScratchpadCSharp_SingleFileTests_NumberArray[10] = {1,2,3,4,5,6,7,8,9,10};

int32_t main(void) {
	ScratchpadCSharp_SingleFileTests_SomeStruct __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SingleFileTests_SomeStruct){0});
	((&__local_0)->Value) = 33;
	do_stuff2();
	do_stuff();
	addOneMacro(10);
	if (10 <= 3) {
		printf("Attempted to access to ScratchpadCSharp_SingleFileTests_NumberArray[%d], but only %u items are in the array", 3, 10);
		abort();
	}
	ScratchpadCSharp_SingleFileTests_NumberArray[3];
	return (__local_0.Value);
}
