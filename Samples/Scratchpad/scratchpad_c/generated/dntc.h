#ifndef DNTC_H_H
#define DNTC_H_H


#include <stdint.h>

typedef struct DntcReferenceTypeBase {
	int32_t activeReferenceCount;
} DntcReferenceTypeBase;



void DntcReferenceTypeBase_Rc_Increment(DntcReferenceTypeBase *referenceType);

#endif // DNTC_H_H
