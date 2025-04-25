#include <stdint.h>
#include <stdio.h>
#include <stdbool.h>
#include "HelloWorld.h"


int32_t main(void) {

#line 23 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 23:23,14:23
	int32_t i = {0};
#line 23 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 23:23,14:23
	i = 0;	goto main_IL_0013;
#line 25 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 25:25,13:50
main_IL_0004:	printf("%u Hello, World!\n", i);
#line 23 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 23:23,35:38
	i = (i + 1);
#line 23 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 23:23,25:33
main_IL_0013:	if ((i < 1000)) {		goto main_IL_0004;	}
#line 28 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 28:28,9:18
	return 0;}
