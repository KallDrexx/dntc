#include <stdint.h>
#include "ScratchpadCSharp_ReferenceTypes.h"
#include <stdlib.h>
#include "dntc.h"
#include <stdbool.h>


void ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass__PrepForFree(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass* object) {
	// No cleanup necessary
}

void ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase__PrepForFree(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase* object) {
	// No cleanup necessary
}

void ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent__PrepForFree(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* object) {
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&(object->InnerClassInstance));

	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase__PrepForFree(&(object->base));
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase_Sum(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase *__this, int32_t a, int32_t b) {
	int32_t __return_value = {0};
	__return_value = (a + b);
	return __return_value;
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent_Sum(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *__this, int32_t a, int32_t b) {
	int32_t __return_value = {0};
	int32_t result = {0};
	result = ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase_Sum((ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase*)__this, a, b);
	__return_value = ((__this->FieldValue) + result);
	return __return_value;
}

ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent__Create(void) {
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* result = {0};
	result = calloc(1, sizeof(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent));
	(((DntcReferenceTypeBase*)result)->PrepForFree) = (void (*)(void*))ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent__PrepForFree;
	((ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase*)result)->ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase_Sum = (int32_t (*)(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase*, int32_t, int32_t))ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent_Sum;
	return result;
}

void ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase__ctor(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase *__this) {
	return;
}

ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass* ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass__Create(void) {
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass* result = {0};
	result = calloc(1, sizeof(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass));
	(((DntcReferenceTypeBase*)result)->PrepForFree) = (void (*)(void*))ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass__PrepForFree;
	return result;
}

void ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass__ctor(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass *__this) {
	return;
}

void ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent__ctor(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *__this, int32_t value) {
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass* __temp_000e = {0};
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase__ctor((ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase*)__this);
	(__this->FieldValue) = value;
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&__temp_000e);
	__temp_000e = ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass__Create();
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)__temp_000e);
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass__ctor(__temp_000e);
	(__temp_000e->TestValue) = value;
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&(__this->InnerClassInstance));
	(__this->InnerClassInstance) = __temp_000e;
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)(__this->InnerClassInstance));
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&__temp_000e);
	return;
}

ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_CreateParent(int32_t value) {
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* __return_value = {0};
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* __temp_0001 = {0};
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&__temp_0001);
	__temp_0001 = ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent__Create();
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)__temp_0001);
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent__ctor(__temp_0001, value);
	__return_value = __temp_0001;
	return __return_value;
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_GetParentValue(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *parent) {
	int32_t __return_value = {0};
	int32_t sum = {0};
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)parent);
	sum = ((ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase*)parent)->ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase_Sum((ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase*)parent, 1, 2);
	__return_value = ((parent->FieldValue) + sum);
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&parent);
	return __return_value;
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Test(void) {
	int32_t __return_value = {0};
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* __temp_0002 = {0};
	__temp_0002 = ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_CreateParent(10);
	__return_value = ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_GetParentValue(__temp_0002);
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&__temp_0002);
	return __return_value;
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent_get_FieldValueViaProperty(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *__this) {
	int32_t __return_value = {0};
	__return_value = (__this->FieldValue);
	return __return_value;
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_GetParentValueFromProperty(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *parent) {
	int32_t __return_value = {0};
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)parent);
	__return_value = ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent_get_FieldValueViaProperty(parent);
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&parent);
	return __return_value;
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_TestBaseFieldValue(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *parent, int32_t value) {
	int32_t __return_value = {0};
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)parent);
	((parent->base).BaseField) = value;
	__return_value = (((parent->base).BaseField) + 5);
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&parent);
	return __return_value;
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase_GetBaseFieldValue(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase *__this) {
	int32_t __return_value = {0};
	__return_value = (__this->BaseField);
	return __return_value;
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_TestBaseMethodCall(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *parent) {
	int32_t __return_value = {0};
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)parent);
	__return_value = ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase_GetBaseFieldValue((ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase*)parent);
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&parent);
	return __return_value;
}

ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray* ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray__Create(void) {
	ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray* result = {0};
	result = calloc(1, sizeof(ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray));
	(((DntcReferenceTypeBase*)result)->PrepForFree) = (void (*)(void*))ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray__PrepForFree;
	return result;
}

void ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray__PrepForFree(ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray* object) {
	free((object->items));
}

ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray* ScratchpadCSharp_ReferenceTypes_ArrayTests_CreateSizedArrayTest(void) {
	ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray* __return_value = {0};
	ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray* __temp_0001 = {0};
	__temp_0001 = ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray__Create();
	(__temp_0001->items) = calloc(5, sizeof(ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayStruct));
	(__temp_0001->length) = 5;
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)__temp_0001);
	__return_value = __temp_0001;
	return __return_value;
}

ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray* ScratchpadCSharp_ReferenceTypes_ArrayTests_CreateModifiedArrayTest(void) {
	ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray* __return_value = {0};
	ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray* array = {0};
	int32_t x = {0};
	ScratchpadCSharpReferenceTypesArrayTestsArrayStructArray* __temp_0000 = {0};
	__temp_0000 = ScratchpadCSharp_ReferenceTypes_ArrayTests_CreateSizedArrayTest();
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&array);
	array = __temp_0000;
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)array);
	x = 0;
	goto ScratchpadCSharp_ReferenceTypes_ArrayTests_CreateModifiedArrayTest_IL_001b;

ScratchpadCSharp_ReferenceTypes_ArrayTests_CreateModifiedArrayTest_IL_000a:
	if ((array->length) <= x) {
		printf("Attempted to access to array[%d], but only %u items are in the array", x, (array->length));
		abort();
	}
	((array->items)[x].Value) = x;
	x = (x + 1);

ScratchpadCSharp_ReferenceTypes_ArrayTests_CreateModifiedArrayTest_IL_001b:
	if ((x < ((int32_t)(array->length)))) {
		goto ScratchpadCSharp_ReferenceTypes_ArrayTests_CreateModifiedArrayTest_IL_000a;
	}
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)array);
	__return_value = array;
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&array);
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&__temp_0000);
	return __return_value;
}

void ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayClass__PrepForFree(ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayClass* object) {
	// No cleanup necessary
}

ScratchpadCSharpReferenceTypesArrayTestsArrayClassArray* ScratchpadCSharpReferenceTypesArrayTestsArrayClassArray__Create(void) {
	ScratchpadCSharpReferenceTypesArrayTestsArrayClassArray* result = {0};
	result = calloc(1, sizeof(ScratchpadCSharpReferenceTypesArrayTestsArrayClassArray));
	(((DntcReferenceTypeBase*)result)->PrepForFree) = (void (*)(void*))ScratchpadCSharpReferenceTypesArrayTestsArrayClassArray__PrepForFree;
	return result;
}

void ScratchpadCSharpReferenceTypesArrayTestsArrayClassArray__PrepForFree(ScratchpadCSharpReferenceTypesArrayTestsArrayClassArray* object) {
	free((object->items));
}

ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayClass* ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayClass__Create(void) {
	ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayClass* result = {0};
	result = calloc(1, sizeof(ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayClass));
	(((DntcReferenceTypeBase*)result)->PrepForFree) = (void (*)(void*))ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayClass__PrepForFree;
	return result;
}

void ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayClass__ctor(ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayClass *__this) {
	return;
}

ScratchpadCSharpReferenceTypesArrayTestsArrayClassArray* ScratchpadCSharp_ReferenceTypes_ArrayTests_CreateReferenceTypeArray(int32_t count) {
	ScratchpadCSharpReferenceTypesArrayTestsArrayClassArray* __return_value = {0};
	ScratchpadCSharpReferenceTypesArrayTestsArrayClassArray* array = {0};
	int32_t x = {0};
	ScratchpadCSharpReferenceTypesArrayTestsArrayClassArray* __temp_0001 = {0};
	ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayClass* __temp_000d = {0};
	__temp_0001 = ScratchpadCSharpReferenceTypesArrayTestsArrayClassArray__Create();
	(__temp_0001->items) = calloc(count, sizeof(ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayClass));
	(__temp_0001->length) = count;
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)__temp_0001);
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&array);
	array = __temp_0001;
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)array);
	x = 0;
	goto ScratchpadCSharp_ReferenceTypes_ArrayTests_CreateReferenceTypeArray_IL_001e;

ScratchpadCSharp_ReferenceTypes_ArrayTests_CreateReferenceTypeArray_IL_000b:
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&__temp_000d);
	__temp_000d = ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayClass__Create();
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)__temp_000d);
	ScratchpadCSharp_ReferenceTypes_ArrayTests_ArrayClass__ctor(__temp_000d);
	(__temp_000d->Value) = x;
	if ((array->length) <= x) {
		printf("Attempted to access to array[%d], but only %u items are in the array", x, (array->length));
		abort();
	}
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)__temp_000d);
	(array->items)[x] = __temp_000d;
	x = (x + 1);

ScratchpadCSharp_ReferenceTypes_ArrayTests_CreateReferenceTypeArray_IL_001e:
	if ((x < count)) {
		goto ScratchpadCSharp_ReferenceTypes_ArrayTests_CreateReferenceTypeArray_IL_000b;
	}
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)array);
	__return_value = array;
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&array);
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&__temp_0001);
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&__temp_000d);
	return __return_value;
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_StArgTest(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass *inner, bool refresh) {
	int32_t __return_value = {0};
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass* __temp_0003 = {0};
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)inner);
	if (!refresh) {
		goto ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_StArgTest_IL_0015;
	}
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&__temp_0003);
	__temp_0003 = ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass__Create();
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)__temp_0003);
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass__ctor(__temp_0003);
	(__temp_0003->TestValue) = 200;
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&inner);
	inner = __temp_0003;
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)inner);

ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_StArgTest_IL_0015:
	__return_value = (inner->TestValue);
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&__temp_0003);
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&inner);
	return __return_value;
}
