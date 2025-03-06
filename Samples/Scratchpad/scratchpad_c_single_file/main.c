#include <stdint.h>
#include "header_test2.h"
#include "header_test1.h"

typedef struct {
	int32_t Value;
} ScratchpadCSharp_SingleFileTests_SomeStruct;


#define addOneMacro(a) ((a) + 1)
int32_t main(void) {
	ScratchpadCSharp_SingleFileTests_SomeStruct __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SingleFileTests_SomeStruct){0});
	((&__local_0)->Value) = 33;
	do_stuff2();
	do_stuff();
	addOneMacro(10);
	return (__local_0.Value);
}
