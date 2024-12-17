#include <stdint.h>

typedef struct {
	int32_t Value;
} ScratchpadCSharp_SingleFileTests_SomeStruct;


int32_t main(void) {
	ScratchpadCSharp_SingleFileTests_SomeStruct __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SingleFileTests_SomeStruct){0});
	((&__local_0)->Value) = 33;
	return (__local_0.Value);
}

