#include <stdint.h>
#include <stdbool.h>
#include <stdio.h>
#include "HelloWorld.h"


int32_t main(void) {

#line 22 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 22:22,5:6
	int32_t i = {0};	bool __local_1 = {0};	int32_t __local_2 = {0};
#line 23 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 23:23,14:23
	i = 0;	goto main_IL_0017;
#line 25 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 25:25,13:50
main_IL_0005:	printf("%u Hello, World!\n", i);
#line 23 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 23:23,35:38
	i = (i + 1);
#line 23 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 23:23,25:33
main_IL_0017:	__local_1 = (i < 1000);	if (__local_1) {		goto main_IL_0005;	}
#line 28 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 28:28,9:18
	__local_2 = 0;
#line 28 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 28:28,9:18
	goto main_IL_0027;
#line 29 "/Users/dan/repos/dntc/Samples/HelloWorld/Program.cs" // 29:29,5:6
main_IL_0027:	return __local_2;}
