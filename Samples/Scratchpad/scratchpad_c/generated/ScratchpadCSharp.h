#ifndef SCRATCHPADCSHARP_H_H
#define SCRATCHPADCSHARP_H_H


#include <math.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include "dotnet_arrays.h"
#include "fn_pointer_types.h"
#include "ScratchpadCSharp.h"

typedef struct {
	float X;
	float Y;
	float Z;
} ScratchpadCSharp_SimpleFunctions_Vector3;

typedef struct {
	float X;
	float Y;
} ScratchpadCSharp_SimpleFunctions_Vector2;

typedef struct {
	ScratchpadCSharp_SimpleFunctions_Vector2 First;
	ScratchpadCSharp_SimpleFunctions_Vector2 Second;
	ScratchpadCSharp_SimpleFunctions_Vector2 Third;
} ScratchpadCSharp_SimpleFunctions_Triangle;


int32_t ScratchpadCSharp_SimpleFunctions_BitwiseOps(int32_t a);
int32_t ScratchpadCSharp_SimpleFunctions_FnPointerTest(FnPtr_Int32_Int32_Returns_Int32 fn, int32_t x, int32_t y);
int32_t ScratchpadCSharp_SimpleFunctions_IfTest(int32_t input);
int32_t ScratchpadCSharp_SimpleFunctions_IntAdd(int32_t a, int32_t b);
float ScratchpadCSharp_SimpleFunctions_MathOps(int32_t input);
int32_t ScratchpadCSharp_SimpleFunctions_StaticFunctionCall(int32_t a);
ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_StructAddTest(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second);
float ScratchpadCSharp_SimpleFunctions__Vector3_Dot(ScratchpadCSharp_SimpleFunctions_Vector3 *__this, ScratchpadCSharp_SimpleFunctions_Vector3 other);
float ScratchpadCSharp_SimpleFunctions_StructInstanceTest(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second);
ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions__Vector3_op_Addition(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second);
ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_StructOpOverload(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second);
ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_StructTest(float x, float y, float z);
ScratchpadCSharp_SimpleFunctions_Vector2 ScratchpadCSharp_SimpleFunctions_Vector2Add(ScratchpadCSharp_SimpleFunctions_Vector2 first, ScratchpadCSharp_SimpleFunctions_Vector2 second);
ScratchpadCSharp_SimpleFunctions_Triangle ScratchpadCSharp_SimpleFunctions_TriangleAdd(ScratchpadCSharp_SimpleFunctions_Triangle a, ScratchpadCSharp_SimpleFunctions_Triangle b);
ScratchpadCSharp_SimpleFunctions_Triangle ScratchpadCSharp_SimpleFunctions_TriangleBuilder(float x0, float y0, float x1, float y1, float x2, float y2);
float ScratchpadCSharp_SimpleFunctions_SquareRootTest(float value);
void ScratchpadCSharp_SimpleFunctions_ArrayTest(SystemUInt16Array test);
void ScratchpadCSharp_SimpleFunctions__Vector3__ctor(ScratchpadCSharp_SimpleFunctions_Vector3 *__this, float x, float y, float z);
ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_ConstructorTest(float x, float y, float z);
void ScratchpadCSharp_SimpleFunctions_RefTest(ScratchpadCSharp_SimpleFunctions_Vector3 *vector, float *floatToAdd, float amount);
int32_t ScratchpadCSharp_SimpleFunctions_SwapTest(int32_t x, int32_t y);
float ScratchpadCSharp_SimpleFunctions_LocalSwapTest(float x0, float x1);
int32_t ScratchpadCSharp_SimpleFunctions_TernaryTest(int32_t a, int32_t b);

#endif // SCRATCHPADCSHARP_H_H
