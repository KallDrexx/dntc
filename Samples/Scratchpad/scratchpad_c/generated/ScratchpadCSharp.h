#ifndef SCRATCHPADCSHARP_H_H
#define SCRATCHPADCSHARP_H_H


#include <math.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include "../native_test.h"
#include "custom_file_test.h"
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

typedef struct {
	int32_t _first_P;
	int32_t _second_P;
} ScratchpadCSharp_GenericTests_AddNumberGetter;

typedef struct {
	int32_t _number_P;
} ScratchpadCSharp_GenericTests_StaticNumberGetter;

typedef struct {
	char __dummy; // Placeholder for empty type
} ScratchpadCSharp_SimpleFunctions;

typedef struct {
	char __dummy; // Placeholder for empty type
} ScratchpadCSharp_AttributeTests;


extern ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_AStaticVector;
extern int32_t ScratchpadCSharp_SimpleFunctions_SomeStaticInt;

int32_t ScratchpadCSharp_SimpleFunctions_BitwiseOps(int32_t a);
int32_t ScratchpadCSharp_SimpleFunctions_FnPointerTest(FnPtr_Int32_Int32_Returns_Int32 fn, int32_t x, int32_t y);
int32_t ScratchpadCSharp_SimpleFunctions_IfTest(int32_t input);
int32_t ScratchpadCSharp_SimpleFunctions_IntAdd(int32_t a, int32_t b);
float ScratchpadCSharp_SimpleFunctions_MathOps(int32_t input);
int32_t ScratchpadCSharp_SimpleFunctions_StaticFunctionCall(int32_t a);
ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_StructAddTest(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second);
float ScratchpadCSharp_SimpleFunctions_Vector3_Dot(ScratchpadCSharp_SimpleFunctions_Vector3 *__this, ScratchpadCSharp_SimpleFunctions_Vector3 other);
float ScratchpadCSharp_SimpleFunctions_StructInstanceTest(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second);
ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_Vector3_op_Addition(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second);
ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_StructOpOverload(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second);
ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_StructTest(float x, float y, float z);
ScratchpadCSharp_SimpleFunctions_Vector2 ScratchpadCSharp_SimpleFunctions_Vector2Add(ScratchpadCSharp_SimpleFunctions_Vector2 first, ScratchpadCSharp_SimpleFunctions_Vector2 second);
ScratchpadCSharp_SimpleFunctions_Triangle ScratchpadCSharp_SimpleFunctions_TriangleAdd(ScratchpadCSharp_SimpleFunctions_Triangle a, ScratchpadCSharp_SimpleFunctions_Triangle b);
ScratchpadCSharp_SimpleFunctions_Triangle ScratchpadCSharp_SimpleFunctions_TriangleBuilder(float x0, float y0, float x1, float y1, float x2, float y2);
float ScratchpadCSharp_SimpleFunctions_SquareRootTest(float value);
void ScratchpadCSharp_SimpleFunctions_ArrayTest(SystemUInt16Array test);
void ScratchpadCSharp_SimpleFunctions_Vector3__ctor(ScratchpadCSharp_SimpleFunctions_Vector3 *__this, float x, float y, float z);
ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_ConstructorTest(float x, float y, float z);
void ScratchpadCSharp_SimpleFunctions_RefTest(ScratchpadCSharp_SimpleFunctions_Vector3 *vector, float *floatToAdd, float amount);
int32_t ScratchpadCSharp_SimpleFunctions_SwapTest(int32_t x, int32_t y);
float ScratchpadCSharp_SimpleFunctions_LocalSwapTest(float x0, float x1);
int32_t ScratchpadCSharp_SimpleFunctions_TernaryTest(int32_t a, int32_t b);
void ScratchpadCSharp_GenericTests_AddNumberGetter__ctor(ScratchpadCSharp_GenericTests_AddNumberGetter *__this, int32_t first, int32_t second);
int32_t ScratchpadCSharp_GenericTests_AddNumberGetter_GetNumber(ScratchpadCSharp_GenericTests_AddNumberGetter *__this);
int32_t ScratchpadCSharp_GenericTests_Run_ScratchpadCSharp_GenericTests_AddNumberGetter(ScratchpadCSharp_GenericTests_AddNumberGetter getter);
int32_t ScratchpadCSharp_GenericTests_GetAddedNumber(int32_t x, int32_t y);
void ScratchpadCSharp_GenericTests_StaticNumberGetter__ctor(ScratchpadCSharp_GenericTests_StaticNumberGetter *__this, int32_t number);
int32_t ScratchpadCSharp_GenericTests_StaticNumberGetter_GetNumber(ScratchpadCSharp_GenericTests_StaticNumberGetter *__this);
int32_t ScratchpadCSharp_GenericTests_Run_ScratchpadCSharp_GenericTests_StaticNumberGetter(ScratchpadCSharp_GenericTests_StaticNumberGetter getter);
int32_t ScratchpadCSharp_GenericTests_GetStaticNumber(int32_t x);
uint32_t ScratchpadCSharp_AttributeTests_GetNumberMethod();
void ScratchpadCSharp_SimpleFunctions__cctor();
int32_t ScratchpadCSharp_SimpleFunctions_IncrementStaticInt();
ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_GetStaticVector();
uint32_t ScratchpadCSharp_AttributeTests_GetStaticNumberField();
void ScratchpadCSharp_AttributeTests_SetStaticNumberField(uint32_t num);
int32_t some_named_function();

#endif // SCRATCHPADCSHARP_H_H
