#ifndef DNTC_H_H
#define DNTC_H_H


#include <stdint.h>
#include <stdlib.h>

typedef struct DntcReferenceTypeBase {
void (*PrepForFree)(void* object);
	int32_t activeReferenceCount;
} DntcReferenceTypeBase;



void DntcReferenceTypeBase_Gc_Untrack(DntcReferenceTypeBase **referenceType);
void DntcReferenceTypeBase_Gc_Track(DntcReferenceTypeBase *referenceType);

#endif // DNTC_H_H
