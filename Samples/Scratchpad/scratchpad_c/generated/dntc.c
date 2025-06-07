#include <stdlib.h>
#include "dntc.h"


void DntcReferenceTypeBase_Gc_Untrack(DntcReferenceTypeBase **referenceType) {
	if (*referenceType == NULL) return;

    DntcReferenceTypeBase *singlePointerVariable = *referenceType;
    int32_t count = --(singlePointerVariable->activeReferenceCount);
    if (count <= 0) {
		singlePointerVariable->PrepForFree(singlePointerVariable);
		free(singlePointerVariable);
		*referenceType = NULL;
	}
}

void DntcReferenceTypeBase_Gc_Track(DntcReferenceTypeBase *referenceType) {

    referenceType->activeReferenceCount++;
}
