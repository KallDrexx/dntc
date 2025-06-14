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

typedef struct ScratchpadCSharp_AttributeTests_NonHeaderStruct {
	int32_t Value;
} ScratchpadCSharp_AttributeTests_NonHeaderStruct;

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_AStaticVector = {0};
int32_t ScratchpadCSharp_SimpleFunctions_SomeStaticInt = {0};
ScratchpadCSharp_AttributeTests_NonHeaderStruct NonHeaderGlobal = {0};
int32_t ScratchpadCSharp_EnumTests_GlobalEnumValue = {0};
uint16_t ScratchpadCSharp_SimpleFunctions_StaticallySizedUshortArray[5] = {1,2,3,4,5};
uint16_t ScratchpadCSharp_SimpleFunctions_OrderOfOperationsTestArray[5] = {1,2,3,4,5};
uint8_t ScratchpadCSharp_SimpleFunctions_OrderOfOperationsIndex = {0};
ScratchpadCSharp_SimpleFunctions_PlusPlusOrderTestStruct ScratchpadCSharp_SimpleFunctions_PlusPlusOrderStruct = {0};
ScratchpadCSharp_AttributeTests_Vector2 ScratchpadCSharp_AttributeTests_StaticallySizedVector2Array[10] = {0};
int32_t ScratchpadCSharp_AttributeTests_UnreferencedGlobalField __attribute__ ((aligned (16))) = 123;
char ScratchpadCSharp_AttributeTests_TestGlobalString[] = {0};
int32_t ScratchpadCSharp_PluginTests_PluginGlobal __attribute__ ((aligned (8))) = {0};
char ScratchpadCSharp_AttributeTests_StaticallySizedString[8] = "abcdefg";
INT_FIELD(custom_declared_global) = 675;
int32_t ScratchpadCSharp_SimpleFunctions_GlobalWithNoInitialValue;

int32_t ScratchpadCSharp_SimpleFunctions_BitwiseOps(int32_t a) {
	int32_t __return_value = {0};
	__return_value = ((((a >> 1) | (a & 15)) << 2) ^ 255);
	return __return_value;
}

int32_t ScratchpadCSharp_SimpleFunctions_FnPointerTest(FnPtr_Int32_Int32_Returns_Int32 fn, int32_t x, int32_t y) {
	int32_t __return_value = {0};
	FnPtr_Int32_Int32_Returns_Int32 __local_0 = {0};
	__local_0 = fn;
	__return_value = __local_0(x, y);
	return __return_value;
}

int32_t ScratchpadCSharp_SimpleFunctions_IfTest(int32_t input) {
	int32_t __return_value = {0};
	if ((input >= 20)) {
		goto ScratchpadCSharp_SimpleFunctions_IfTest_IL_0007;
	}
	__return_value = -1;
	return __return_value;

ScratchpadCSharp_SimpleFunctions_IfTest_IL_0007:
	if ((input < 50)) {
		goto ScratchpadCSharp_SimpleFunctions_IfTest_IL_000f;
	}
	__return_value = 100;
	return __return_value;

ScratchpadCSharp_SimpleFunctions_IfTest_IL_000f:
	__return_value = input;
	return __return_value;
}

int32_t ScratchpadCSharp_SimpleFunctions_IntAdd(int32_t a, int32_t b) {
	int32_t __return_value = {0};
	__return_value = (a + b);
	return __return_value;
}

float ScratchpadCSharp_SimpleFunctions_MathOps(int32_t input) {
	float __return_value = {0};
	__return_value = (((((float)input) + ((3.5 * ((float)input)) / 3)) - 2) + 10);
	return __return_value;
}

int32_t ScratchpadCSharp_SimpleFunctions_StaticFunctionCall(int32_t a) {
	int32_t __return_value = {0};
	ScratchpadCSharp_SimpleFunctions_IntAdd(a, 5);
	__return_value = ScratchpadCSharp_SimpleFunctions_IntAdd(a, 20);
	return __return_value;
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_StructAddTest(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __return_value = {0};
	ScratchpadCSharp_SimpleFunctions_Vector3 __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SimpleFunctions_Vector3){0});
	((&__local_0)->X) = ((first.X) + (second.X));
	((&__local_0)->Y) = ((first.Y) + (second.Y));
	((&__local_0)->Z) = ((first.Z) + (second.Z));
	__return_value = __local_0;
	return __return_value;
}

float ScratchpadCSharp_SimpleFunctions_Vector3_Dot(ScratchpadCSharp_SimpleFunctions_Vector3 *__this, ScratchpadCSharp_SimpleFunctions_Vector3 other) {
	float __return_value = {0};
	__return_value = ((((__this->X) * (other.X)) + ((__this->Y) * (other.Y))) + ((__this->Z) * (other.Z)));
	return __return_value;
}

float ScratchpadCSharp_SimpleFunctions_StructInstanceTest(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second) {
	float __return_value = {0};
	__return_value = ScratchpadCSharp_SimpleFunctions_Vector3_Dot((&first), second);
	return __return_value;
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_Vector3_op_Addition(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __return_value = {0};
	ScratchpadCSharp_SimpleFunctions_Vector3 __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SimpleFunctions_Vector3){0});
	((&__local_0)->X) = ((first.X) + (second.X));
	((&__local_0)->Y) = ((first.Y) + (second.Y));
	((&__local_0)->Z) = ((first.Z) + (second.Z));
	__return_value = __local_0;
	return __return_value;
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_StructOpOverload(ScratchpadCSharp_SimpleFunctions_Vector3 first, ScratchpadCSharp_SimpleFunctions_Vector3 second) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __return_value = {0};
	__return_value = ScratchpadCSharp_SimpleFunctions_Vector3_op_Addition(first, second);
	return __return_value;
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_StructTest(float x, float y, float z) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __return_value = {0};
	ScratchpadCSharp_SimpleFunctions_Vector3 __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SimpleFunctions_Vector3){0});
	((&__local_0)->X) = x;
	((&__local_0)->Y) = y;
	((&__local_0)->Z) = z;
	__return_value = __local_0;
	return __return_value;
}

ScratchpadCSharp_SimpleFunctions_Vector2 ScratchpadCSharp_SimpleFunctions_Vector2Add(ScratchpadCSharp_SimpleFunctions_Vector2 first, ScratchpadCSharp_SimpleFunctions_Vector2 second) {
	ScratchpadCSharp_SimpleFunctions_Vector2 __return_value = {0};
	ScratchpadCSharp_SimpleFunctions_Vector2 __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SimpleFunctions_Vector2){0});
	((&__local_0)->X) = ((first.X) + (second.X));
	((&__local_0)->Y) = ((first.Y) + (second.Y));
	__return_value = __local_0;
	return __return_value;
}

ScratchpadCSharp_SimpleFunctions_Triangle ScratchpadCSharp_SimpleFunctions_TriangleAdd(ScratchpadCSharp_SimpleFunctions_Triangle a, ScratchpadCSharp_SimpleFunctions_Triangle b) {
	ScratchpadCSharp_SimpleFunctions_Triangle __return_value = {0};
	ScratchpadCSharp_SimpleFunctions_Triangle __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_SimpleFunctions_Triangle){0});
	((&__local_0)->First) = ScratchpadCSharp_SimpleFunctions_Vector2Add((a.First), (b.First));
	((&__local_0)->Second) = ScratchpadCSharp_SimpleFunctions_Vector2Add((a.Second), (b.Second));
	((&__local_0)->Third) = ScratchpadCSharp_SimpleFunctions_Vector2Add((a.Third), (b.Third));
	__return_value = __local_0;
	return __return_value;
}

ScratchpadCSharp_SimpleFunctions_Triangle ScratchpadCSharp_SimpleFunctions_TriangleBuilder(float x0, float y0, float x1, float y1, float x2, float y2) {
	ScratchpadCSharp_SimpleFunctions_Triangle __return_value = {0};
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
	__return_value = __local_0;
	return __return_value;
}

float ScratchpadCSharp_SimpleFunctions_SquareRootTest(float value) {
	float __return_value = {0};
	__return_value = ((float)sqrt(((double)value)));
	return __return_value;
}

uint16_t ScratchpadCSharp_SimpleFunctions_ArrayTest(SystemUInt16Array test) {
	uint16_t __return_value = {0};
	uint16_t sum = {0};
	int32_t x = {0};
	x = 0;
	goto ScratchpadCSharp_SimpleFunctions_ArrayTest_IL_000d;

ScratchpadCSharp_SimpleFunctions_ArrayTest_IL_0004:
	if ((test.length) <= x) {
		printf("Attempted to access to test[%d], but only %u items are in the array", x, (test.length));
		abort();
	}
	(test.items)[x] = ((uint16_t)x);
	x = (x + 1);

ScratchpadCSharp_SimpleFunctions_ArrayTest_IL_000d:
	if ((x < ((int32_t)(test.length)))) {
		goto ScratchpadCSharp_SimpleFunctions_ArrayTest_IL_0004;
	}
	sum = 0;
	x = 0;
	goto ScratchpadCSharp_SimpleFunctions_ArrayTest_IL_0024;

ScratchpadCSharp_SimpleFunctions_ArrayTest_IL_0019:
	if ((test.length) <= x) {
		printf("Attempted to access to test[%d], but only %u items are in the array", x, (test.length));
		abort();
	}
	sum = ((uint16_t)(sum + (test.items)[x]));
	x = (x + 1);

ScratchpadCSharp_SimpleFunctions_ArrayTest_IL_0024:
	if ((x < ((int32_t)(test.length)))) {
		goto ScratchpadCSharp_SimpleFunctions_ArrayTest_IL_0019;
	}
	__return_value = sum;
	return __return_value;
}

void ScratchpadCSharp_SimpleFunctions_Vector3__ctor(ScratchpadCSharp_SimpleFunctions_Vector3 *__this, float x, float y, float z) {
	(__this->X) = x;
	(__this->Y) = y;
	(__this->Z) = z;
	return;
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_ConstructorTest(float x, float y, float z) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __return_value = {0};
	ScratchpadCSharp_SimpleFunctions_Vector3 __temp_0003 = {0};
	ScratchpadCSharp_SimpleFunctions_Vector3__ctor((&__temp_0003), x, y, z);
	__return_value = __temp_0003;
	return __return_value;
}

void ScratchpadCSharp_SimpleFunctions_RefTest(ScratchpadCSharp_SimpleFunctions_Vector3 *vector, float *floatToAdd, float amount) {
	(*floatToAdd) = ((*floatToAdd) + amount);
	(*(&(vector->X))) = ((*(&(vector->X))) + amount);
	(*(&(vector->Y))) = ((*(&(vector->Y))) + amount);
	(*(&(vector->Z))) = ((*(&(vector->Z))) + amount);
	return;
}

int32_t ScratchpadCSharp_SimpleFunctions_SwapTest(int32_t x, int32_t y) {
	int32_t __return_value = {0};
	int32_t __temp_0002 = {0};
	__temp_0002 = x;
	x = y;
	y = __temp_0002;
	__return_value = y;
	return __return_value;
}

float ScratchpadCSharp_SimpleFunctions_LocalSwapTest(float x0, float x1) {
	float __return_value = {0};
	ScratchpadCSharp_SimpleFunctions_Vector2 first = {0};
	ScratchpadCSharp_SimpleFunctions_Vector2 temp = {0};
	ScratchpadCSharp_SimpleFunctions_Vector2 __local_2 = {0};
	(*(&__local_2)) = ((ScratchpadCSharp_SimpleFunctions_Vector2){0});
	((&__local_2)->X) = x0;
	((&__local_2)->Y) = 0;
	first = __local_2;
	(*(&__local_2)) = ((ScratchpadCSharp_SimpleFunctions_Vector2){0});
	((&__local_2)->X) = x1;
	((&__local_2)->Y) = 0;
	temp = first;
	first = __local_2;
	__return_value = (temp.X);
	return __return_value;
}

int32_t ScratchpadCSharp_SimpleFunctions_TernaryTest(int32_t a, int32_t b) {
	int32_t __checkpoint_for_il000c = {0};
	int32_t __return_value = {0};
	if ((a < b)) {
		goto ScratchpadCSharp_SimpleFunctions_TernaryTest_IL_0009;
	}
	__checkpoint_for_il000c = (a - b);
	goto ScratchpadCSharp_SimpleFunctions_TernaryTest_IL_000c;

ScratchpadCSharp_SimpleFunctions_TernaryTest_IL_0009:
	__checkpoint_for_il000c = (b - a);

ScratchpadCSharp_SimpleFunctions_TernaryTest_IL_000c:
	__return_value = (__checkpoint_for_il000c + 3);
	return __return_value;
}

void ScratchpadCSharp_GenericTests_AddNumberGetter__ctor(ScratchpadCSharp_GenericTests_AddNumberGetter *__this, int32_t first, int32_t second) {
	(__this->_first_P) = first;
	(__this->_second_P) = second;
	return;
}

int32_t ScratchpadCSharp_GenericTests_AddNumberGetter_GetNumber(ScratchpadCSharp_GenericTests_AddNumberGetter *__this) {
	int32_t __return_value = {0};
	__return_value = ((__this->_first_P) + (__this->_second_P));
	return __return_value;
}

int32_t ScratchpadCSharp_GenericTests_Run_ScratchpadCSharp_GenericTests_AddNumberGetter(ScratchpadCSharp_GenericTests_AddNumberGetter getter) {
	int32_t __return_value = {0};
	__return_value = ScratchpadCSharp_GenericTests_AddNumberGetter_GetNumber((&getter));
	return __return_value;
}

int32_t ScratchpadCSharp_GenericTests_GetAddedNumber(int32_t x, int32_t y) {
	int32_t __return_value = {0};
	ScratchpadCSharp_GenericTests_AddNumberGetter __temp_0002 = {0};
	ScratchpadCSharp_GenericTests_AddNumberGetter__ctor((&__temp_0002), x, y);
	__return_value = ScratchpadCSharp_GenericTests_Run_ScratchpadCSharp_GenericTests_AddNumberGetter(__temp_0002);
	return __return_value;
}

void ScratchpadCSharp_GenericTests_StaticNumberGetter__ctor(ScratchpadCSharp_GenericTests_StaticNumberGetter *__this, int32_t number) {
	(__this->_number_P) = number;
	return;
}

int32_t ScratchpadCSharp_GenericTests_StaticNumberGetter_GetNumber(ScratchpadCSharp_GenericTests_StaticNumberGetter *__this) {
	int32_t __return_value = {0};
	__return_value = (__this->_number_P);
	return __return_value;
}

int32_t ScratchpadCSharp_GenericTests_Run_ScratchpadCSharp_GenericTests_StaticNumberGetter(ScratchpadCSharp_GenericTests_StaticNumberGetter getter) {
	int32_t __return_value = {0};
	__return_value = ScratchpadCSharp_GenericTests_StaticNumberGetter_GetNumber((&getter));
	return __return_value;
}

int32_t ScratchpadCSharp_GenericTests_GetStaticNumber(int32_t x) {
	int32_t __return_value = {0};
	ScratchpadCSharp_GenericTests_StaticNumberGetter __temp_0001 = {0};
	ScratchpadCSharp_GenericTests_StaticNumberGetter__ctor((&__temp_0001), x);
	__return_value = ScratchpadCSharp_GenericTests_Run_ScratchpadCSharp_GenericTests_StaticNumberGetter(__temp_0001);
	return __return_value;
}

uint32_t ScratchpadCSharp_AttributeTests_GetNumberMethod(void) {
	uint32_t __return_value = {0};
	__return_value = get_number();
	return __return_value;
}

void ScratchpadCSharp_SimpleFunctions__cctor(void) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __temp_000f = {0};
	ScratchpadCSharp_SimpleFunctions_Vector3__ctor((&__temp_000f), 10, 11, 12);
	ScratchpadCSharp_SimpleFunctions_AStaticVector = __temp_000f;
	return;
}

int32_t ScratchpadCSharp_SimpleFunctions_IncrementStaticInt(void) {
	int32_t __return_value = {0};
	ScratchpadCSharp_SimpleFunctions_SomeStaticInt = (ScratchpadCSharp_SimpleFunctions_SomeStaticInt + 1);
	__return_value = ScratchpadCSharp_SimpleFunctions_SomeStaticInt;
	return __return_value;
}

ScratchpadCSharp_SimpleFunctions_Vector3 ScratchpadCSharp_SimpleFunctions_GetStaticVector(void) {
	ScratchpadCSharp_SimpleFunctions_Vector3 __return_value = {0};
	__return_value = ScratchpadCSharp_SimpleFunctions_AStaticVector;
	return __return_value;
}

uint32_t ScratchpadCSharp_AttributeTests_GetStaticNumberField(void) {
	uint32_t __return_value = {0};
	__return_value = static_number;
	return __return_value;
}

void ScratchpadCSharp_AttributeTests_SetStaticNumberField(uint32_t num) {
	static_number = num;
	return;
}

int32_t some_named_function(void) {
	int32_t __return_value = {0};
	__return_value = 94;
	return __return_value;
}

void ScratchpadCSharp_AttributeTests__cctor(void) {
	ScratchpadCSharp_AttributeTests_NonHeaderStruct __local_0 = {0};
	(*(&__local_0)) = ((ScratchpadCSharp_AttributeTests_NonHeaderStruct){0});
	((&__local_0)->Value) = 1020;
	NonHeaderGlobal = __local_0;
	return;
}

int32_t ScratchpadCSharp_AttributeTests_CustomFileReferenceTestMethod(void) {
	int32_t __return_value = {0};
	__return_value = ((&ScratchpadCSharp_AttributeTests_TestStructField)->Value);
	return __return_value;
}

DECLARE_TEST(custom_declared_method) {
	int32_t __return_value = {0};
	__return_value = 929;
	return __return_value;
}

int32_t ScratchpadCSharp_AttributeTests_ReferToCustomDeclaredMethod(void) {
	int32_t __return_value = {0};
	__return_value = custom_declared_method();
	return __return_value;
}

int32_t ScratchpadCSharp_AttributeTests_GetNonHeaderStructValue(void) {
	int32_t __return_value = {0};
	__return_value = ((&NonHeaderGlobal)->Value);
	return __return_value;
}

__attribute__ ((always_inline))
int32_t ScratchpadCSharp_AttributeTests_InlineTest(void) {
	int32_t __return_value = {0};
	__return_value = 42;
	return __return_value;
}

int32_t ScratchpadCSharp_AttributeTests_GetNativeTypeValue(struct NativeType obj) {
	int32_t __return_value = {0};
	__return_value = (obj.Value);
	return __return_value;
}

int32_t ScratchpadCSharp_AttributeTests_GetNativeTypeValueRef(struct NativeType *obj) {
	int32_t __return_value = {0};
	__return_value = (obj->Value);
	return __return_value;
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
	int32_t __return_value = {0};
	__return_value = ScratchpadCSharp_Dependency_GenericUtils_GenericReturnValue_System_Int32(x);
	return __return_value;
}

void ScratchpadCSharp_AttributeTests_TestNativeGenericInDep(void) {
	printf("generic dep test %d\n", 5678);
	return;
}

int32_t ScratchpadCSharp_AttributeTests_GetCustomFieldStructValue(ScratchpadCSharp_AttributeTests_CustomFieldNameStruct obj) {
	int32_t __return_value = {0};
	__return_value = (obj.some_value);
	return __return_value;
}

int32_t ScratchpadCSharp_AttributeTests_GetFirstNumber(ScratchpadCSharp_AttributeTests_StaticallySizedTest value) {
	int32_t __return_value = {0};
	if (10 <= 1) {
		printf("Attempted to access to (value.NumberArray)[%d], but only %u items are in the array", 1, 10);
		abort();
	}
	(value.NumberArray)[1] = 25;
	if (10 <= 0) {
		printf("Attempted to access to (value.NumberArray)[%d], but only %u items are in the array", 0, 10);
		abort();
	}
	__return_value = (value.NumberArray)[0];
	return __return_value;
}

int32_t ScratchpadCSharp_AttributeTests_GetFirstNumberNoBoundsCheck(ScratchpadCSharp_AttributeTests_StaticallySizedTest value) {
	int32_t __return_value = {0};
	(value.NumberArray2)[1] = 25;
	__return_value = (value.NumberArray2)[0];
	return __return_value;
}

int32_t ScratchpadCSharp_AttributeTests_GetCustomDeclaredField(ScratchpadCSharp_AttributeTests_CustomDeclaredFieldStruct fieldStruct) {
	int32_t __return_value = {0};
	__return_value = (fieldStruct.field);
	return __return_value;
}

int32_t* ScratchpadCSharp_GenericTests_GenericPointerTest(void) {
	int32_t* __return_value = {0};
	int32_t* __temp_000f = {0};
	__temp_000f = generic_pointer_return_type_test(sizeof(int32_t));
	(*__temp_000f) = 25;
	__return_value = __temp_000f;
	return __return_value;
}

int32_t ScratchpadCSharp_GenericTests_RefArgTest(int32_t value) {
	int32_t __return_value = {0};
	int32_t test = {0};
	test = 0;
	generic_ref_test((&test), value);
	__return_value = test;
	return __return_value;
}

void ScratchpadCSharp_GenericTests_PointerAssignmentTest(int32_t *input) {
	int32_t* first = {0};
	int32_t* second = {0};
	int32_t* __temp_000f = {0};
	int32_t* __temp_0025 = {0};
	int32_t* __temp_003a = {0};
	__temp_000f = generic_pointer_return_type_test(sizeof(int32_t));
	input = __temp_000f;
	__temp_0025 = generic_pointer_return_type_test(sizeof(int32_t));
	first = __temp_0025;
	__temp_003a = generic_pointer_return_type_test(sizeof(int32_t));
	second = __temp_003a;
	(*input) = ((*first) + (*second));
	return;
}

bool ScratchpadCSharp_GenericTests_PointerNullCheck(void) {
	bool __return_value = {0};
	ScratchpadCSharp_GenericTests_SimpleStruct* __temp_000f = {0};
	__temp_000f = generic_pointer_return_type_test(sizeof(ScratchpadCSharp_GenericTests_SimpleStruct));
	__return_value = (__temp_000f == ((uint32_t)0));
	return __return_value;
}

int32_t ScratchpadCSharp_GenericTests_PointerNullCheck2(void) {
	int32_t __return_value = {0};
	ScratchpadCSharp_GenericTests_SimpleStruct* __temp_000f = {0};
	__temp_000f = generic_pointer_return_type_test(sizeof(ScratchpadCSharp_GenericTests_SimpleStruct));
	if ((__temp_000f == ((uint32_t)0))) {
		goto ScratchpadCSharp_GenericTests_PointerNullCheck2_IL_001a;
	}
	__return_value = 2;
	return __return_value;

ScratchpadCSharp_GenericTests_PointerNullCheck2_IL_001a:
	__return_value = 1;
	return __return_value;
}

int32_t ScratchpadCSharp_SimpleFunctions_ReturnOne(void) {
	int32_t __return_value = {0};
	__return_value = 1;
	return __return_value;
}

FnPtr_Returns_Int32 ScratchpadCSharp_SimpleFunctions_GetFunctionPointer(void) {
	FnPtr_Returns_Int32 __return_value = {0};
	__return_value = (&ScratchpadCSharp_SimpleFunctions_ReturnOne);
	return __return_value;
}

int32_t ScratchpadCSharp_SimpleFunctions_RunFunctionPointer(FnPtr_Returns_Int32 function) {
	int32_t __return_value = {0};
	__return_value = function();
	return __return_value;
}

void ScratchpadCSharp_SimpleFunctions_SetOtherAssemblyFieldValue(int32_t value) {
	ScratchpadCSharp_Dependency_Misc_FieldInAnotherAssembly = value;
	return;
}

int32_t ScratchpadCSharp_SimpleFunctions_GetOtherAssemblyFieldValue(void) {
	int32_t __return_value = {0};
	__return_value = ScratchpadCSharp_Dependency_Misc_FieldInAnotherAssembly;
	return __return_value;
}

int32_t ScratchpadCSharp_AttributeTests_NonNativeGetNumber(int32_t first, int32_t second, int32_t third) {
	int32_t __return_value = {0};
	__return_value = ((first + second) + third);
	return __return_value;
}

int32_t ScratchpadCSharp_AttributeTests_CallNativePointer(void) {
	int32_t __return_value = {0};
	__return_value = void_pointer_test((&ScratchpadCSharp_AttributeTests_NonNativeGetNumber));
	return __return_value;
}

int32_t addOneFunc(int32_t a) {
return a + 1;}

int32_t ScratchpadCSharp_AttributeTests_AddTwo(int32_t input) {
	int32_t __return_value = {0};
	__return_value = addOneFunc(addOneMacro(input));
	return __return_value;
}

int32_t ScratchpadCSharp_AttributeTests_AddOneInt(int32_t input) {
	int32_t __return_value = {0};
	__return_value = addOneGenericMacro(input);
	return __return_value;
}

uint32_t ScratchpadCSharp_AttributeTests_AddOneUint(uint32_t input) {
	uint32_t __return_value = {0};
	__return_value = addOneGenericMacro(input);
	return __return_value;
}

int32_t ScratchpadCSharp_AttributeTests_GetFirstNativeArrayNoBoundsCheckValue(void) {
	int32_t __return_value = {0};
	__return_value = native_number_array[0];
	return __return_value;
}

int32_t ScratchpadCSharp_EnumTests_GetTestEnumValue(void) {
	int32_t __return_value = {0};
	__return_value = ScratchpadCSharp_EnumTests_GlobalEnumValue;
	return __return_value;
}

void ScratchpadCSharp_EnumTests_SetEnumValue(int32_t value) {
	ScratchpadCSharp_EnumTests_GlobalEnumValue = value;
	return;
}

uint16_t ScratchpadCSharp_EnumTests_GetUShortEnumValue(void) {
	uint16_t __return_value = {0};
	__return_value = 3;
	return __return_value;
}

void ScratchpadCSharp_SimpleFunctions_LdIndRefTest(SystemUInt16Array *array, int32_t index, uint16_t value) {
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)array);
	if ((array->length) <= index) {
		printf("Attempted to access to array[%d], but only %u items are in the array", index, (array->length));
		abort();
	}
	(array->items)[index] = value;
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&array);
	return;
}

int32_t ScratchpadCSharp_SimpleFunctions_NegOpCodeTest(int32_t value) {
	int32_t __return_value = {0};
	__return_value = -value;
	return __return_value;
}

int32_t ScratchpadCSharp_SimpleFunctions_LocalDeclarationOrderingTest(int32_t a, int32_t b) {
	int32_t __return_value = {0};
	int32_t __temp_0007 = {0};
	b = (a + b);
	__temp_0007 = a;
	a = b;
	b = __temp_0007;
	__return_value = a;
	return __return_value;
}

void ScratchpadCSharp_SimpleFunctions_ArrayItemIncrementValidation(void) {
	ScratchpadCSharp_SimpleFunctions_StaticallySizedUshortArray[1] = ((uint16_t)(ScratchpadCSharp_SimpleFunctions_StaticallySizedUshortArray[1] + 10));
	return;
}

uint16_t ScratchpadCSharp_SimpleFunctions_PlusPlusOrderOfOperationsValidation(void) {
	uint16_t __return_value = {0};
	uint8_t __temp_000e = {0};
	__temp_000e = ScratchpadCSharp_SimpleFunctions_OrderOfOperationsIndex;
	ScratchpadCSharp_SimpleFunctions_OrderOfOperationsIndex = ((uint8_t)(ScratchpadCSharp_SimpleFunctions_OrderOfOperationsIndex + 1));
	__return_value = ScratchpadCSharp_SimpleFunctions_OrderOfOperationsTestArray[__temp_000e];
	return __return_value;
}

uint16_t ScratchpadCSharp_SimpleFunctions_PlusPlusStructOrderOfOperationsValidation(void) {
	uint16_t __return_value = {0};
	uint8_t __local_0 = {0};
	__local_0 = (*(&((&ScratchpadCSharp_SimpleFunctions_PlusPlusOrderStruct)->Index)));
	(*(&((&ScratchpadCSharp_SimpleFunctions_PlusPlusOrderStruct)->Index))) = ((uint8_t)(__local_0 + 1));
	__return_value = ScratchpadCSharp_SimpleFunctions_OrderOfOperationsTestArray[__local_0];
	return __return_value;
}

void ScratchpadCSharp_StringTests_LogString(char* input) {
	printf(input);
	return;
}

int32_t ScratchpadCSharp_AttributeTests_ValidateLdFldFromArray(void) {
	int32_t __return_value = {0};
	int32_t sum = {0};
	int32_t x = {0};
	sum = 0;
	x = 0;
	goto ScratchpadCSharp_AttributeTests_ValidateLdFldFromArray_IL_0030;

ScratchpadCSharp_AttributeTests_ValidateLdFldFromArray_IL_0006:
	if (10 <= x) {
		printf("Attempted to access to ScratchpadCSharp_AttributeTests_StaticallySizedVector2Array[%d], but only %u items are in the array", x, 10);
		abort();
	}
	sum = (sum + (ScratchpadCSharp_AttributeTests_StaticallySizedVector2Array[x].X));
	if (10 <= x) {
		printf("Attempted to access to ScratchpadCSharp_AttributeTests_StaticallySizedVector2Array[%d], but only %u items are in the array", x, 10);
		abort();
	}
	sum = (sum + (ScratchpadCSharp_AttributeTests_StaticallySizedVector2Array[x].Y));
	x = (x + 1);

ScratchpadCSharp_AttributeTests_ValidateLdFldFromArray_IL_0030:
	if ((x < ((int32_t)10))) {
		goto ScratchpadCSharp_AttributeTests_ValidateLdFldFromArray_IL_0006;
	}
	__return_value = sum;
	return __return_value;
}
