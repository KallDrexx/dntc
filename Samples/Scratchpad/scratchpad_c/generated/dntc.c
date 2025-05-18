#include "dntc.h"


void DntcReferenceTypeBase_Rc_Increment(DntcReferenceTypeBase *referenceType) {

    referenceType->activeReferenceCount++;
}
