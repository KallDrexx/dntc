#include <stdint.h>
#include "ScratchpadCSharp.h"
#include "fn_pointer_types.h"
#include <stdbool.h>
#include <math.h>
#include "dotnet_arrays.h"
#include <stdlib.h>
#include "../native_test.h"
#include "custom_file_test.h"
#include "../macros.h"
#include <stdio.h>
#include <string.h>
#include "ScratchpadCSharp_Dependency.h"

typedef struct {
	int32_t Value;
} ScratchpadCSharp_AttributeTests_NonHeaderStruct;

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_AStaticVector = {0};
int32_t ScratchpadCSharp_SimpleFunctions_SomeStaticInt = {0};
ScratchpadCSharp_AttributeTests_NonHeaderStruct NonHeaderGlobal = {0};
int32_t ScratchpadCSharp_AttributeTests_UnreferencedGlobalField __attribute__ ((aligned (16))) = 123;
char ScratchpadCSharp_AttributeTests_TestGlobalString[] = {0};
int32_t ScratchpadCSharp_PluginTests_PluginGlobal __attribute__ ((aligned (8))) = {0};
char ScratchpadCSharp_AttributeTests_StaticallySizedString[8] = "abcdefg";
INT_FIELD(custom_declared_global) = 675;
int32_t ScratchpadCSharp_SimpleFunctions_GlobalWithNoInitialValue;

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

float ScratchpadCSharp_SimpleFunctions_Vector3_Dot(ScratchpadCSharp_SimpleFunctions_Vector3 *__this, ScratchpadCSharp_SimpleFunctions_Vector3 other) {
	return ((((__this->X) * (other.X)) + ((__this->Y) * (other.Y))) + ((__this->Z) * (other.Z)));
}

float ScratchpadCSharp_SimpleFunctions_StructInstanceTest(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second) {
	return ScratchpadCSharp_SimpleFunctions_Vector3_Dot((&first), second);
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_Vector3_op_Addition(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SimpleFunctions_Vector3){0});
	((&__local_0)->X) = ((first.X) + (second.X));
	((&__local_0)->Y) = ((first.Y) + (second.Y));
	((&__local_0)->Z) = ((first.Z) + (second.Z));
	return __local_0;
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_StructOpOverload(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second) {
	return ScratchpadCSharp_SimpleFunctions_Vector3_op_Addition(first, second);
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

uint16_t ScratchpadCSharp_SimpleFunctions_ArrayTest(SystemUInt16Array test) {
	uint16_t __local_0 = {0};
	int32_t __local_1 = {0};
	int32_t __local_2 = {0};
	__local_1 = 0;
	goto IL_000d;

IL_0004:
	if ((test.length) <= __local_1) {
		printf("Attempted to access to test[%d], but only %u items are in the array", __local_1, (test.length));
		abort();
	}
	(test.items)[__local_1] = ((uint16_t)__local_1);
	__local_1 = (__local_1 + 1);

IL_000d:
	if ((__local_1 < ((int32_t)(test.length)))) {
		goto IL_0004;
	}
	__local_0 = 0;
	__local_2 = 0;
	goto IL_0024;

IL_0019:
	if ((test.length) <= __local_2) {
		printf("Attempted to access to test[%d], but only %u items are in the array", __local_2, (test.length));
		abort();
	}
	__local_0 = ((uint16_t)(__local_0 + (test.items)[__local_2]));
	__local_2 = (__local_2 + 1);

IL_0024:
	if ((__local_2 < ((int32_t)(test.length)))) {
		goto IL_0019;
	}
	return __local_0;
}

void ScratchpadCSharp_SimpleFunctions_Vector3__ctor(ScratchpadCSharp_SimpleFunctions_Vector3 *__this, float x, float y, float z) {
	(__this->X) = x;
	(__this->Y) = y;
	(__this->Z) = z;
	return;
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_ConstructorTest(float x, float y, float z) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __temp_0003 = {0};
	ScratchpadCSharp_SimpleFunctions_Vector3__ctor((&__temp_0003), x, y, z);
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

void ScratchpadCSharp_GenericTests_AddNumberGetter__ctor(ScratchpadCSharp_GenericTests_AddNumberGetter *__this, int32_t first, int32_t second) {
	(__this->_first_P) = first;
	(__this->_second_P) = second;
	return;
}

int32_t ScratchpadCSharp_GenericTests_AddNumberGetter_GetNumber(ScratchpadCSharp_GenericTests_AddNumberGetter *__this) {
	return ((__this->_first_P) + (__this->_second_P));
}

int32_t ScratchpadCSharp_GenericTests_Run_ScratchpadCSharp_GenericTests_AddNumberGetter(ScratchpadCSharp_GenericTests_AddNumberGetter getter) {
	return ScratchpadCSharp_GenericTests_AddNumberGetter_GetNumber((&getter));
}

int32_t ScratchpadCSharp_GenericTests_GetAddedNumber(int32_t x, int32_t y) {
	ScratchpadCSharp_GenericTests_AddNumberGetter __temp_0002 = {0};
	ScratchpadCSharp_GenericTests_AddNumberGetter__ctor((&__temp_0002), x, y);
	return ScratchpadCSharp_GenericTests_Run_ScratchpadCSharp_GenericTests_AddNumberGetter(__temp_0002);
}

void ScratchpadCSharp_GenericTests_StaticNumberGetter__ctor(ScratchpadCSharp_GenericTests_StaticNumberGetter *__this, int32_t number) {
	(__this->_number_P) = number;
	return;
}

int32_t ScratchpadCSharp_GenericTests_StaticNumberGetter_GetNumber(ScratchpadCSharp_GenericTests_StaticNumberGetter *__this) {
	return (__this->_number_P);
}

int32_t ScratchpadCSharp_GenericTests_Run_ScratchpadCSharp_GenericTests_StaticNumberGetter(ScratchpadCSharp_GenericTests_StaticNumberGetter getter) {
	return ScratchpadCSharp_GenericTests_StaticNumberGetter_GetNumber((&getter));
}

int32_t ScratchpadCSharp_GenericTests_GetStaticNumber(int32_t x) {
	ScratchpadCSharp_GenericTests_StaticNumberGetter __temp_0001 = {0};
	ScratchpadCSharp_GenericTests_StaticNumberGetter__ctor((&__temp_0001), x);
	return ScratchpadCSharp_GenericTests_Run_ScratchpadCSharp_GenericTests_StaticNumberGetter(__temp_0001);
}

uint32_t ScratchpadCSharp_AttributeTests_GetNumberMethod(void) {
	return get_number();
}

void ScratchpadCSharp_SimpleFunctions__cctor(void) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __temp_000f = {0};
	ScratchpadCSharp_SimpleFunctions_Vector3__ctor((&__temp_000f), 10, 11, 12);
	ScratchpadCSharp_SimpleFunctions_AStaticVector = __temp_000f;
	return;
}

int32_t ScratchpadCSharp_SimpleFunctions_IncrementStaticInt(void) {
	ScratchpadCSharp_SimpleFunctions_SomeStaticInt = (ScratchpadCSharp_SimpleFunctions_SomeStaticInt + 1);
	return ScratchpadCSharp_SimpleFunctions_SomeStaticInt;
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_GetStaticVector(void) {
	return ScratchpadCSharp_SimpleFunctions_AStaticVector;
}

uint32_t ScratchpadCSharp_AttributeTests_GetStaticNumberField(void) {
	return static_number;
}

void ScratchpadCSharp_AttributeTests_SetStaticNumberField(uint32_t num) {
	static_number = num;
	return;
}

int32_t some_named_function(void) {
	return 94;
}

void ScratchpadCSharp_AttributeTests__cctor(void) {
	ScratchpadCSharp_AttributeTests_NonHeaderStruct __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_AttributeTests_NonHeaderStruct){0});
	((&__local_0)->Value) = 1020;
	NonHeaderGlobal = __local_0;
	return;
}

int32_t ScratchpadCSharp_AttributeTests_CustomFileReferenceTestMethod(void) {
	return ((&ScratchpadCSharp_AttributeTests_TestStructField)->Value);
}

DECLARE_TEST(custom_declared_method) {
	return 929;
}

int32_t ScratchpadCSharp_AttributeTests_ReferToCustomDeclaredMethod(void) {
	return custom_declared_method();
}

int32_t ScratchpadCSharp_AttributeTests_GetNonHeaderStructValue(void) {
	return ((&NonHeaderGlobal)->Value);
}

__attribute__ ((always_inline))
int32_t ScratchpadCSharp_AttributeTests_InlineTest(void) {
	return 42;
}

int32_t ScratchpadCSharp_AttributeTests_GetNativeTypeValue(struct NativeType obj) {
	return (obj.Value);
}

int32_t ScratchpadCSharp_AttributeTests_GetNativeTypeValueRef(struct NativeType *obj) {
	return (obj->Value);
}

void ScratchpadCSharp_AttributeTests_TestNativeGeneric(void) {
	printf("generic test %d\n", 1234);
	return;
}

void ScratchpadCSharp_StringTests_LogStaticString(void) {
	printf("abcdefg\n");
	return;
}

int32_t ScratchpadCSharp_GenericTests_GetGenericNumberFromDep(int32_t x) {
	return ScratchpadCSharp_Dependency_GenericUtils_GenericReturnValue_System_Int32(x);
}

void ScratchpadCSharp_AttributeTests_TestNativeGenericInDep(void) {
	printf("generic dep test %d\n", 5678);
	return;
}

int32_t ScratchpadCSharp_AttributeTests_GetCustomFieldStructValue(ScratchpadCSharp_AttributeTests_CustomFieldNameStruct obj) {
	return (obj.some_value);
}

int32_t ScratchpadCSharp_AttributeTests_GetFirstNumber(ScratchpadCSharp_AttributeTests_StaticallySizedTest value) {
	if (10 <= 1) {
		printf("Attempted to access to (value.NumberArray)[%d], but only %u items are in the array", 1, 10);
		abort();
	}
	(value.NumberArray)[1] = 25;
	if (10 <= 0) {
		printf("Attempted to access to (value.NumberArray)[%d], but only %u items are in the array", 0, 10);
		abort();
	}
	return (value.NumberArray)[0];
}

int32_t ScratchpadCSharp_AttributeTests_GetCustomDeclaredField(ScratchpadCSharp_AttributeTests_CustomDeclaredFieldStruct fieldStruct) {
	return (fieldStruct.field);
}

int32_t* ScratchpadCSharp_GenericTests_GenericPointerTest(void) {
	int32_t* __temp_000f = {0};
	__temp_000f = generic_pointer_return_type_test(sizeof(int32_t));
	(*__temp_000f) = 25;
	return __temp_000f;
}

int32_t ScratchpadCSharp_GenericTests_RefArgTest(int32_t value) {
	int32_t __local_0 = {0};
	__local_0 = 0;
	generic_ref_test((&__local_0), value);
	return __local_0;
}

void ScratchpadCSharp_GenericTests_PointerAssignmentTest(int32_t *input) {
	int32_t* __local_0 = {0};
	int32_t* __local_1 = {0};
	int32_t* __temp_000f = {0};
	__temp_000f = generic_pointer_return_type_test(sizeof(int32_t));
	input = __temp_000f;
	int32_t* __temp_0025 = {0};
	__temp_0025 = generic_pointer_return_type_test(sizeof(int32_t));
	__local_0 = __temp_0025;
	int32_t* __temp_003a = {0};
	__temp_003a = generic_pointer_return_type_test(sizeof(int32_t));
	__local_1 = __temp_003a;
	(*input) = ((*__local_0) + (*__local_1));
	return;
}

bool ScratchpadCSharp_GenericTests_PointerNullCheck(void) {
	ScratchpadCSharp_GenericTests_SimpleStruct* __temp_000f = {0};
	__temp_000f = generic_pointer_return_type_test(sizeof(ScratchpadCSharp_GenericTests_SimpleStruct));
	return (__temp_000f == ((uint32_t)0));
}

int32_t ScratchpadCSharp_GenericTests_PointerNullCheck2(void) {
	ScratchpadCSharp_GenericTests_SimpleStruct* __temp_000f = {0};
	__temp_000f = generic_pointer_return_type_test(sizeof(ScratchpadCSharp_GenericTests_SimpleStruct));
	if ((__temp_000f == ((uint32_t)0))) {
		goto IL_001a;
	}
	return 2;

IL_001a:
	return 1;
}

int32_t ScratchpadCSharp_SimpleFunctions_ReturnOne(void) {
	return 1;
}

FnPtr_Returns_Int32 ScratchpadCSharp_SimpleFunctions_GetFunctionPointer(void) {
	return (&ScratchpadCSharp_SimpleFunctions_ReturnOne);
}

int32_t ScratchpadCSharp_SimpleFunctions_RunFunctionPointer(FnPtr_Returns_Int32 function) {
	return function();
}

void ScratchpadCSharp_SimpleFunctions_SetOtherAssemblyFieldValue(int32_t value) {
	ScratchpadCSharp_Dependency_Misc_FieldInAnotherAssembly = value;
	return;
}

int32_t ScratchpadCSharp_SimpleFunctions_GetOtherAssemblyFieldValue(void) {
	return ScratchpadCSharp_Dependency_Misc_FieldInAnotherAssembly;
}

