#include <stdint.h>
#include "ScratchpadCSharp_ReferenceTypes.h"


ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent__Create(void) {
    ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* result = (ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent*) malloc(sizeof(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent));
	memset(result, 0, sizeof(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent));
	return result;
}

void ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent__ctor(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *__this, int32_t value) {
	(__this->FieldValue) = value;
	return;
}

ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_CreateParent(int32_t value) {
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent* __temp_0001 = {0};
	__temp_0001 = ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent__Create();
	ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent__ctor(__temp_0001, value);
	return __temp_0001;
}

int32_t ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_GetParentValue(ScratchpadCSharp_ReferenceTypes_BasicClassSupportTests_Parent *parent) {
	return (parent->FieldValue);
}
