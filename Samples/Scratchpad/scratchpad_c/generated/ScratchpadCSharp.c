#include <math.h>
#include <stdint.h>
#include "dotnet_arrays.h"
#include "fn_pointer_types.h"
#include "ScratchpadCSharp.h"

int32_t ScratchpadCSharp_SimpleFunctions_BitwiseOps(int32_t a) {
	return ((((a >> 1) | (a & 15)) << 2) ^ 255);
}

int32_t ScratchpadCSharp_SimpleFunctions_FnPointerTest(FnPtr_Int32_Int32_Returns_Int32 fn, int32_t x, int32_t y) {
	FnPtr_Int32_Int32_Returns_Int32 __local_0 = {0};
	__local_0 = fn;
	return __local_0(x, y);
}

int32_t ScratchpadCSharp_SimpleFunctions_IfTest(int32_t input) {
	if ((input >= 20)) {
		goto IL_0007;
	}
	return -1;

IL_0007:
	if ((input < 50)) {
		goto IL_000f;
	}
	return 100;

IL_000f:
	return input;
}

int32_t ScratchpadCSharp_SimpleFunctions_IntAdd(int32_t a, int32_t b) {
	return (a + b);
}

float ScratchpadCSharp_SimpleFunctions_MathOps(int32_t input) {
	return (((((float)input) + ((3.5 * ((float)input)) / 3)) - 2) + 10);
}

int32_t ScratchpadCSharp_SimpleFunctions_StaticFunctionCall(int32_t a) {
	ScratchpadCSharp_SimpleFunctions_IntAdd(a, 5);
	return ScratchpadCSharp_SimpleFunctions_IntAdd(a, 20);
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_StructAddTest(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SimpleFunctions_Vector3){0});
	((&__local_0)->X) = ((first.X) + (second.X));
	((&__local_0)->Y) = ((first.Y) + (second.Y));
	((&__local_0)->Z) = ((first.Z) + (second.Z));
	return __local_0;
}

float ScratchpadCSharp_SimpleFunctions__Vector3_Dot(ScratchpadCSharp_SimpleFunctions_Vector3 *__this, ScratchpadCSharp_SimpleFunctions_Vector3 other) {
	return ((((__this->X) * (other.X)) + ((__this->Y) * (other.Y))) + ((__this->Z) * (other.Z)));
}

float ScratchpadCSharp_SimpleFunctions_StructInstanceTest(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second) {
	return ScratchpadCSharp_SimpleFunctions__Vector3_Dot((&first), second);
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions__Vector3_op_Addition(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SimpleFunctions_Vector3){0});
	((&__local_0)->X) = ((first.X) + (second.X));
	((&__local_0)->Y) = ((first.Y) + (second.Y));
	((&__local_0)->Z) = ((first.Z) + (second.Z));
	return __local_0;
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_StructOpOverload(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second) {
	return ScratchpadCSharp_SimpleFunctions__Vector3_op_Addition(first, second);
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_StructTest(float x, float y, float z) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SimpleFunctions_Vector3){0});
	((&__local_0)->X) = x;
	((&__local_0)->Y) = y;
	((&__local_0)->Z) = z;
	return __local_0;
}

ScratchpadCSharp_SimpleFunctions_Vector2 ScratchpadCSharp_SimpleFunctions_Vector2Add(ScratchpadCSharp_SimpleFunctions_Vector2 first, ScratchpadCSharp_SimpleFunctions_Vector2 second) {
	ScratchpadCSharp_SimpleFunctions_Vector2 __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SimpleFunctions_Vector2){0});
	((&__local_0)->X) = ((first.X) + (second.X));
	((&__local_0)->Y) = ((first.Y) + (second.Y));
	return __local_0;
}

ScratchpadCSharp_SimpleFunctions_Triangle ScratchpadCSharp_SimpleFunctions_TriangleAdd(ScratchpadCSharp_SimpleFunctions_Triangle a, ScratchpadCSharp_SimpleFunctions_Triangle b) {
	ScratchpadCSharp_SimpleFunctions_Triangle __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SimpleFunctions_Triangle){0});
	((&__local_0)->First) = ScratchpadCSharp_SimpleFunctions_Vector2Add((a.First), (b.First));
	((&__local_0)->Second) = ScratchpadCSharp_SimpleFunctions_Vector2Add((a.Second), (b.Second));
	((&__local_0)->Third) = ScratchpadCSharp_SimpleFunctions_Vector2Add((a.Third), (b.Third));
	return __local_0;
}

ScratchpadCSharp_SimpleFunctions_Triangle ScratchpadCSharp_SimpleFunctions_TriangleBuilder(float x0, float y0, float x1, float y1, float x2, float y2) {
	ScratchpadCSharp_SimpleFunctions_Triangle __local_0 = {0};
	ScratchpadCSharp_SimpleFunctions_Vector2 __local_1 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SimpleFunctions_Triangle){0});
	(*(&__local_1)) = ((ScratchpadCSharp_SimpleFunctions_Vector2){0});
	((&__local_1)->X) = x0;
	((&__local_1)->Y) = y0;
	((&__local_0)->First) = __local_1;
	(*(&__local_1)) = ((ScratchpadCSharp_SimpleFunctions_Vector2){0});
	((&__local_1)->X) = x1;
	((&__local_1)->Y) = y1;
	((&__local_0)->Second) = __local_1;
	(*(&__local_1)) = ((ScratchpadCSharp_SimpleFunctions_Vector2){0});
	((&__local_1)->X) = x2;
	((&__local_1)->Y) = y2;
	((&__local_0)->Third) = __local_1;
	return __local_0;
}

float ScratchpadCSharp_SimpleFunctions_SquareRootTest(float value) {
	return ((float)sqrt(((double)value)));
}

void ScratchpadCSharp_SimpleFunctions_ArrayTest(SystemUInt16Array test) {
	int32_t __local_0 = {0};
	__local_0 = 0;
	goto IL_000d;

IL_0004:
	if ((test.length) <= __local_0) {
		printf("ATtempted to write to test[%ld], but only %zu items are in the array", __local_0, (test.length));
		abort();
	}
	(test.items)[__local_0] = ((uint16_t)__local_0);
	__local_0 = (__local_0 + 1);

IL_000d:
	if ((__local_0 < ((int32_t)(test.length)))) {
		goto IL_0004;
	}
	return;
}

void ScratchpadCSharp_SimpleFunctions__Vector3__ctor(ScratchpadCSharp_SimpleFunctions_Vector3 *__this, float x, float y, float z) {
	(__this->X) = x;
	(__this->Y) = y;
	(__this->Z) = z;
	return;
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_ConstructorTest(float x, float y, float z) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __temp_0003 = {0};
	ScratchpadCSharp_SimpleFunctions__Vector3__ctor((&__temp_0003), x, y, z);
	return __temp_0003;
}

void ScratchpadCSharp_SimpleFunctions_RefTest(ScratchpadCSharp_SimpleFunctions_Vector3 *vector, float *floatToAdd, float amount) {
	(*floatToAdd) = ((*floatToAdd) + amount);
	(*(&(vector->X))) = ((*(&(vector->X))) + amount);
	(*(&(vector->Y))) = ((*(&(vector->Y))) + amount);
	(*(&(vector->Z))) = ((*(&(vector->Z))) + amount);
	return;
}

int32_t ScratchpadCSharp_SimpleFunctions_SwapTest(int32_t x, int32_t y) {
	int32_t __temp_0002 = {0};
	__temp_0002 = x;
	x = y;
	y = __temp_0002;
	return y;
}

float ScratchpadCSharp_SimpleFunctions_LocalSwapTest(float x0, float x1) {
	ScratchpadCSharp_SimpleFunctions_Vector2 __local_0 = {0};
	ScratchpadCSharp_SimpleFunctions_Vector2 __local_1 = {0};
	ScratchpadCSharp_SimpleFunctions_Vector2 __local_2 = {0};
	(*(&__local_2)) = ((ScratchpadCSharp_SimpleFunctions_Vector2){0});
	((&__local_2)->X) = x0;
	((&__local_2)->Y) = 0;
	__local_0 = __local_2;
	(*(&__local_2)) = ((ScratchpadCSharp_SimpleFunctions_Vector2){0});
	((&__local_2)->X) = x1;
	((&__local_2)->Y) = 0;
	__local_1 = __local_0;
	__local_0 = __local_2;
	return (__local_1.X);
}

int32_t ScratchpadCSharp_SimpleFunctions_TernaryTest(int32_t a, int32_t b) {
	int32_t __checkpoint_for_il000c = {0};
	if ((a < b)) {
		goto IL_0009;
	}
	__checkpoint_for_il000c = (a - b);
	goto IL_000c;

IL_0009:
	__checkpoint_for_il000c = (b - a);

IL_000c:
	return (__checkpoint_for_il000c + 3);
}

