#ifndef DNTC_H_
#define DNTC_H_

typedef struct TypeInfo {
    uint32_t* implemented_interfaces; // Array of implemented interfaces
	size_t* interface_offsets; // Array of offsets for each interface
	size_t interface_count; // Count of implemented interfaces
} TypeInfo;

// all types will inherit this.
typedef struct ReferenceType_Base {
	TypeInfo* type_info;
} ReferenceType_Base;

// Function to check if an instance implements a specific interface
// Function to check if an instance implements a specific interface and return a pointer to it
void* dynamic_cast_interface(ReferenceType_Base* instance, uint32_t interface) {
    if (instance && instance->type_info) {
        TypeInfo* type_info = instance->type_info;
        for (size_t i = 0; i < type_info->interface_count; ++i) {
            if (type_info->implemented_interfaces[i] == interface) {
                // Calculate the address of the interface implementation using the offset
                return (void*)((char*)instance + type_info->interface_offsets[i]);
            }
        }
    }
    return NULL; // Interface is not implemented
}



#endif