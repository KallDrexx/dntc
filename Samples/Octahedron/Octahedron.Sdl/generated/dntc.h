#ifndef DNTC_H_H
#define DNTC_H_H


#include <stdint.h>

typedef struct DntcReferenceTypeBase {
	void (*PrepForFree)(void* object);
	int32_t activeReferenceCount;
} DntcReferenceTypeBase;




#endif // DNTC_H_H
