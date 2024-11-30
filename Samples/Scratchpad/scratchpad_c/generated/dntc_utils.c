#include "dntc_utils.h"
#include "ScratchpadCSharp.h"



void dntc_utils_init_static_constructors(void) {
	ScratchpadCSharp_SimpleFunctions__cctor();
	ScratchpadCSharp_AttributeTests__cctor();
}

