#include <stdint.h>
#include "ScratchpadCSharp_ReferenceTypes.h"
#include <stdlib.h>
#include "dntc.h"


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
	return (a + b);
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent_Sum(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *__this, int32_t a, int32_t b) {
	int32_t result = {0};
	result = ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase_Sum((ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase*)__this, a, b);
	return ((__this->FieldValue) + result);
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
	__temp_000e = ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass__Create();
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)__temp_000e);
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_InnerClass__ctor(__temp_000e);
	(__temp_000e->TestValue) = value;
	if ((__this->InnerClassInstance) != NULL) {
		DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&(__this->InnerClassInstance));

	}
	(__this->InnerClassInstance) = __temp_000e;
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)(__this->InnerClassInstance));
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&__temp_000e);
	return;
}

ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_CreateParent(int32_t value) {
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* __temp_0001 = {0};
	__temp_0001 = ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent__Create();
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)__temp_0001);
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent__ctor(__temp_0001, value);
	return __temp_0001;
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_GetParentValue(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *parent) {
	int32_t sum = {0};
	sum = ((ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase*)parent)->ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase_Sum((ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase*)parent, 1, 2);
	return ((parent->FieldValue) + sum);
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Test(void) {
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* __temp_0002 = {0};
	__temp_0002 = ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_CreateParent(10);
	DntcReferenceTypeBase_Gc_Track((DntcReferenceTypeBase*)__temp_0002);
	DntcReferenceTypeBase_Gc_Untrack((DntcReferenceTypeBase**)&__temp_0002);
	return ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_GetParentValue(__temp_0002);
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent_get_FieldValueViaProperty(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *__this) {
	return (__this->FieldValue);
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_GetParentValueFromProperty(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *parent) {
	return ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent_get_FieldValueViaProperty(parent);
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_TestBaseFieldValue(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *parent, int32_t value) {
	((parent->base).BaseField) = value;
	return (((parent->base).BaseField) + 5);
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase_GetBaseFieldValue(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase *__this) {
	return (__this->BaseField);
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_TestBaseMethodCall(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *parent) {
	return ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase_GetBaseFieldValue((ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_ParentBase*)parent);
}
