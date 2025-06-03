#include "Dntc_Samples_Octahedron_Common.h"
#include <stdint.h>
#include <math.h>
#include <stdbool.h>
#include "dotnet_arrays.h"
#include "dotnet_math.h"


void Dntc_Samples_Octahedron_Common_Vector3__ctor(Dntc_Samples_Octahedron_Common_Vector3 *__this, float x, float y, float z) {
	float __local_0 = {0};
	float __local_1 = {0};
	float __local_2 = {0};
	__local_0 = x;
	__local_1 = y;
	__local_2 = z;
	(__this->X) = __local_0;
	(__this->Y) = __local_1;
	(__this->Z) = __local_2;
	return;
}

Dntc_Samples_Octahedron_Common_Camera Dntc_Samples_Octahedron_Common_Camera_Default(void) {
	Dntc_Samples_Octahedron_Common_Camera __return_value = {0};
	Dntc_Samples_Octahedron_Common_Camera __local_0 = {0};
	Dntc_Samples_Octahedron_Common_Camera __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_001a = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0035 = {0};
	(*(&__local_0)) = ((Dntc_Samples_Octahedron_Common_Camera){0});
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_001a), 1, 0, 0);
	((&__local_0)->Right) = __temp_001a;
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0035), 0, 1, 0);
	((&__local_0)->Up) = __temp_0035;
	__local_1 = __local_0;
	goto Dntc_Samples_Octahedron_Common_Camera_Default_IL_0043;

Dntc_Samples_Octahedron_Common_Camera_Default_IL_0043:
	__return_value = __local_1;
	return __return_value;
}

void Dntc_Samples_Octahedron_Common_Triangle__ctor(Dntc_Samples_Octahedron_Common_Triangle *__this, Dntc_Samples_Octahedron_Common_Vector3 v1, Dntc_Samples_Octahedron_Common_Vector3 v2, Dntc_Samples_Octahedron_Common_Vector3 v3) {
	Dntc_Samples_Octahedron_Common_Vector3 __local_0 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_2 = {0};
	__local_0 = v1;
	__local_1 = v2;
	__local_2 = v3;
	(__this->V1) = __local_0;
	(__this->V2) = __local_1;
	(__this->V3) = __local_2;
	return;
}

Dntc_Samples_Octahedron_Common_Triangle Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle(Dntc_Samples_Octahedron_Common_OctahedronShape *__this, int32_t index) {
	Dntc_Samples_Octahedron_Common_Triangle __return_value = {0};
	int32_t __local_0 = {0};
	int32_t __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_2 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_3 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_003f = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0053 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0067 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_006c = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0086 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_009a = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00ae = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_00b3 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00cd = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00e1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00f5 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_00fa = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0114 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0128 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_013c = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_0141 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_015b = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_016f = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0183 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_0188 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01a2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01b6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01ca = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_01cf = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01e9 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01fd = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0211 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_0216 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_022d = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0241 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0255 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_025a = {0};
	__local_1 = index;
	__local_0 = __local_1;
	switch(__local_0) {
		case 0: goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_0030;
		case 1: goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_0077;
		case 2: goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_00be;
		case 3: goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_0105;
		case 4: goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_014c;
		case 5: goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_0193;
		case 6: goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_01da;
		case 7: goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_021e;
	}

	goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_0262;

Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_0030:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_003f), 1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0053), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0067), 0, 0, 1);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_006c), __temp_003f, __temp_0053, __temp_0067);
	__local_2 = __temp_006c;
	goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_026e;

Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_0077:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0086), 1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_009a), 0, 0, -1);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00ae), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_00b3), __temp_0086, __temp_009a, __temp_00ae);
	__local_2 = __temp_00b3;
	goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_026e;

Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_00be:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00cd), 1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00e1), 0, 0, 1);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00f5), 0, -1, 0);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_00fa), __temp_00cd, __temp_00e1, __temp_00f5);
	__local_2 = __temp_00fa;
	goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_026e;

Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_0105:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0114), 1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0128), 0, -1, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_013c), 0, 0, -1);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0141), __temp_0114, __temp_0128, __temp_013c);
	__local_2 = __temp_0141;
	goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_026e;

Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_014c:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_015b), -1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_016f), 0, 0, 1);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0183), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0188), __temp_015b, __temp_016f, __temp_0183);
	__local_2 = __temp_0188;
	goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_026e;

Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_0193:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01a2), -1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01b6), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01ca), 0, 0, -1);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_01cf), __temp_01a2, __temp_01b6, __temp_01ca);
	__local_2 = __temp_01cf;
	goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_026e;

Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_01da:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01e9), -1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01fd), 0, -1, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0211), 0, 0, 1);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0216), __temp_01e9, __temp_01fd, __temp_0211);
	__local_2 = __temp_0216;
	goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_026e;

Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_021e:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_022d), -1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0241), 0, 0, -1);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0255), 0, -1, 0);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_025a), __temp_022d, __temp_0241, __temp_0255);
	__local_2 = __temp_025a;
	goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_026e;

Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_0262:
	(*(&__local_3)) = ((Dntc_Samples_Octahedron_Common_Triangle){0});
	__local_2 = __local_3;
	goto Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_026e;

Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle_IL_026e:
	__return_value = __local_2;
	return __return_value;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ(Dntc_Samples_Octahedron_Common_Vector3 *__this, float degrees) {
	Dntc_Samples_Octahedron_Common_Vector3 __return_value = {0};
	double rotationRadians = {0};
	double currentRotation = {0};
	float length = {0};
	double newRotation = {0};
	float x = {0};
	float y = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0073 = {0};
	rotationRadians = ((((double)degrees) * 3.141592653589793) / 180);
	currentRotation = atan2(((double)(__this->Y)), ((double)(__this->X)));
	length = ((float)sqrt(((double)(((__this->X) * (__this->X)) + ((__this->Y) * (__this->Y))))));
	newRotation = (currentRotation + rotationRadians);
	x = (((float)cos(newRotation)) * length);
	y = (((float)sin(newRotation)) * length);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0073), x, y, (__this->Z));
	__local_6 = __temp_0073;
	goto Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ_IL_007c;

Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ_IL_007c:
	__return_value = __local_6;
	return __return_value;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_RotateOnY(Dntc_Samples_Octahedron_Common_Vector3 *__this, float degrees) {
	Dntc_Samples_Octahedron_Common_Vector3 __return_value = {0};
	double rotationRadians = {0};
	double currentRotation = {0};
	float length = {0};
	double newRotation = {0};
	float x = {0};
	float z = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0073 = {0};
	rotationRadians = ((((double)degrees) * 3.141592653589793) / 180);
	currentRotation = atan2(((double)(__this->Z)), ((double)(__this->X)));
	length = ((float)sqrt(((double)(((__this->X) * (__this->X)) + ((__this->Z) * (__this->Z))))));
	newRotation = (currentRotation + rotationRadians);
	x = (((float)cos(newRotation)) * length);
	z = (((float)sin(newRotation)) * length);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0073), x, (__this->Y), z);
	__local_6 = __temp_0073;
	goto Dntc_Samples_Octahedron_Common_Vector3_RotateOnY_IL_007c;

Dntc_Samples_Octahedron_Common_Vector3_RotateOnY_IL_007c:
	__return_value = __local_6;
	return __return_value;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_RotateOnX(Dntc_Samples_Octahedron_Common_Vector3 *__this, float degrees) {
	Dntc_Samples_Octahedron_Common_Vector3 __return_value = {0};
	double rotationRadians = {0};
	double currentRotation = {0};
	float length = {0};
	double newRotation = {0};
	float z = {0};
	float y = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0073 = {0};
	rotationRadians = ((((double)degrees) * 3.141592653589793) / 180);
	currentRotation = atan2(((double)(__this->Y)), ((double)(__this->Z)));
	length = ((float)sqrt(((double)(((__this->Z) * (__this->Z)) + ((__this->Y) * (__this->Y))))));
	newRotation = (currentRotation + rotationRadians);
	z = (((float)cos(newRotation)) * length);
	y = (((float)sin(newRotation)) * length);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0073), (__this->X), y, z);
	__local_6 = __temp_0073;
	goto Dntc_Samples_Octahedron_Common_Vector3_RotateOnX_IL_007c;

Dntc_Samples_Octahedron_Common_Vector3_RotateOnX_IL_007c:
	__return_value = __local_6;
	return __return_value;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_op_Subtraction(Dntc_Samples_Octahedron_Common_Vector3 first, Dntc_Samples_Octahedron_Common_Vector3 second) {
	Dntc_Samples_Octahedron_Common_Vector3 __return_value = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_0 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0028 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0028), ((first.X) - (second.X)), ((first.Y) - (second.Y)), ((first.Z) - (second.Z)));
	__local_0 = __temp_0028;
	goto Dntc_Samples_Octahedron_Common_Vector3_op_Subtraction_IL_0030;

Dntc_Samples_Octahedron_Common_Vector3_op_Subtraction_IL_0030:
	__return_value = __local_0;
	return __return_value;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_Cross(Dntc_Samples_Octahedron_Common_Vector3 *__this, Dntc_Samples_Octahedron_Common_Vector3 other) {
	Dntc_Samples_Octahedron_Common_Vector3 __return_value = {0};
	float x = {0};
	float y = {0};
	float z = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_3 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0058 = {0};
	x = (((__this->Y) * (other.Z)) - ((__this->Z) * (other.Y)));
	y = (((__this->Z) * (other.X)) - ((__this->X) * (other.Z)));
	z = (((__this->X) * (other.Y)) - ((__this->Y) * (other.X)));
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0058), x, y, z);
	__local_3 = __temp_0058;
	goto Dntc_Samples_Octahedron_Common_Vector3_Cross_IL_0060;

Dntc_Samples_Octahedron_Common_Vector3_Cross_IL_0060:
	__return_value = __local_3;
	return __return_value;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Triangle_get_Normal(Dntc_Samples_Octahedron_Common_Triangle *__this) {
	Dntc_Samples_Octahedron_Common_Vector3 __return_value = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_0 = {0};
	__local_0 = Dntc_Samples_Octahedron_Common_Vector3_op_Subtraction((__this->V2), (__this->V1));
	__return_value = Dntc_Samples_Octahedron_Common_Vector3_Cross((&__local_0), Dntc_Samples_Octahedron_Common_Vector3_op_Subtraction((__this->V3), (__this->V1)));
	return __return_value;
}

float Dntc_Samples_Octahedron_Common_Vector3_get_Length(Dntc_Samples_Octahedron_Common_Vector3 *__this) {
	float __return_value = {0};
	__return_value = ((float)sqrt(((double)((((__this->X) * (__this->X)) + ((__this->Y) * (__this->Y))) + ((__this->Z) * (__this->Z))))));
	return __return_value;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_op_Multiply(Dntc_Samples_Octahedron_Common_Vector3 vec, float scalar) {
	Dntc_Samples_Octahedron_Common_Vector3 __return_value = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_0 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0019 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0019), ((vec.X) * scalar), ((vec.Y) * scalar), ((vec.Z) * scalar));
	__local_0 = __temp_0019;
	goto Dntc_Samples_Octahedron_Common_Vector3_op_Multiply_IL_0021;

Dntc_Samples_Octahedron_Common_Vector3_op_Multiply_IL_0021:
	__return_value = __local_0;
	return __return_value;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_get_Unit(Dntc_Samples_Octahedron_Common_Vector3 *__this) {
	Dntc_Samples_Octahedron_Common_Vector3 __return_value = {0};
	__return_value = Dntc_Samples_Octahedron_Common_Vector3_op_Multiply((*__this), (1 / Dntc_Samples_Octahedron_Common_Vector3_get_Length(__this)));
	return __return_value;
}

float Dntc_Samples_Octahedron_Common_Vector3_Dot(Dntc_Samples_Octahedron_Common_Vector3 *__this, Dntc_Samples_Octahedron_Common_Vector3 other) {
	float __return_value = {0};
	float __local_0 = {0};
	__local_0 = ((((__this->X) * (other.X)) + ((__this->Y) * (other.Y))) + ((__this->Z) * (other.Z)));
	goto Dntc_Samples_Octahedron_Common_Vector3_Dot_IL_002d;

Dntc_Samples_Octahedron_Common_Vector3_Dot_IL_002d:
	__return_value = __local_0;
	return __return_value;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(Dntc_Samples_Octahedron_Common_Vector3 vector, Dntc_Samples_Octahedron_Common_Camera camera) {
	Dntc_Samples_Octahedron_Common_Vector3 __return_value = {0};
	float x = {0};
	float y = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_003e = {0};
	x = (Dntc_Samples_Octahedron_Common_Vector3_Dot((&vector), (camera.Right)) / Dntc_Samples_Octahedron_Common_Vector3_get_Length((&((&camera)->Right))));
	y = (Dntc_Samples_Octahedron_Common_Vector3_Dot((&vector), (camera.Up)) / Dntc_Samples_Octahedron_Common_Vector3_get_Length((&((&camera)->Up))));
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_003e), x, y, 0);
	__local_2 = __temp_003e;
	goto Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d_IL_0046;

Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d_IL_0046:
	__return_value = __local_2;
	return __return_value;
}

uint16_t Dntc_Samples_Octahedron_Common_Renderer_Rgb888To565(uint8_t red, uint8_t green, uint8_t blue) {
	uint16_t __return_value = {0};
	uint16_t __local_0 = {0};
	red = ((uint8_t)(red / 8));
	green = ((uint8_t)(green / 4));
	blue = ((uint8_t)(blue / 8));
	__local_0 = ((uint16_t)(((red << 11) | (green << 5)) | blue));
	goto Dntc_Samples_Octahedron_Common_Renderer_Rgb888To565_IL_0021;

Dntc_Samples_Octahedron_Common_Renderer_Rgb888To565_IL_0021:
	__return_value = __local_0;
	return __return_value;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Renderer_ToScreen(Dntc_Samples_Octahedron_Common_Vector3 vector, Dntc_Samples_Octahedron_Common_Camera camera) {
	Dntc_Samples_Octahedron_Common_Vector3 __return_value = {0};
	uint16_t x = {0};
	uint16_t y = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_003a = {0};
	x = ((uint16_t)(((vector.X) * 100) + ((float)((camera.PixelWidth) / 2))));
	y = ((uint16_t)(((vector.Y) * 100) + ((float)((camera.PixelHeight) / 2))));
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_003a), ((float)x), ((float)y), 0);
	__local_2 = __temp_003a;
	goto Dntc_Samples_Octahedron_Common_Renderer_ToScreen_IL_0042;

Dntc_Samples_Octahedron_Common_Renderer_ToScreen_IL_0042:
	__return_value = __local_2;
	return __return_value;
}

Dntc_Samples_Octahedron_Common_Triangle Dntc_Samples_Octahedron_Common_Triangle_SortPoints(Dntc_Samples_Octahedron_Common_Triangle *__this) {
	Dntc_Samples_Octahedron_Common_Triangle __return_value = {0};
	Dntc_Samples_Octahedron_Common_Vector3 v1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 v2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 v3 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 temp = {0};
	bool __local_4 = {0};
	bool __local_5 = {0};
	bool __local_6 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_7 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_0071 = {0};
	v1 = (__this->V1);
	v2 = (__this->V2);
	v3 = (__this->V3);
	__local_4 = ((v1.Y) > (v2.Y));
	if (!__local_4) {
		goto Dntc_Samples_Octahedron_Common_Triangle_SortPoints_IL_0032;
	}
	temp = v1;
	v1 = v2;
	v2 = temp;

Dntc_Samples_Octahedron_Common_Triangle_SortPoints_IL_0032:
	__local_5 = ((v3.Y) < (v1.Y));
	if (!__local_5) {
		goto Dntc_Samples_Octahedron_Common_Triangle_SortPoints_IL_0052;
	}
	temp = v3;
	v3 = v2;
	v2 = v1;
	v1 = temp;
	goto Dntc_Samples_Octahedron_Common_Triangle_SortPoints_IL_006e;

Dntc_Samples_Octahedron_Common_Triangle_SortPoints_IL_0052:
	__local_6 = ((v3.Y) < (v2.Y));
	if (!__local_6) {
		goto Dntc_Samples_Octahedron_Common_Triangle_SortPoints_IL_006e;
	}
	temp = v3;
	v3 = v2;
	v2 = temp;

Dntc_Samples_Octahedron_Common_Triangle_SortPoints_IL_006e:
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0071), v1, v2, v3);
	__local_7 = __temp_0071;
	goto Dntc_Samples_Octahedron_Common_Triangle_SortPoints_IL_007a;

Dntc_Samples_Octahedron_Common_Triangle_SortPoints_IL_007a:
	__return_value = __local_7;
	return __return_value;
}

void Dntc_Samples_Octahedron_Common_Point__ctor(Dntc_Samples_Octahedron_Common_Point *__this, Dntc_Samples_Octahedron_Common_Vector3 vector) {
	(__this->X) = ((int32_t)(vector.X));
	(__this->Y) = ((int32_t)(vector.Y));
	return;
}

void Dntc_Samples_Octahedron_Common_PointPair__ctor(Dntc_Samples_Octahedron_Common_PointPair *__this, Dntc_Samples_Octahedron_Common_Vector3 v1, Dntc_Samples_Octahedron_Common_Vector3 v2) {
	Dntc_Samples_Octahedron_Common_Point __temp_0003 = {0};
	Dntc_Samples_Octahedron_Common_Point __temp_000f = {0};
	Dntc_Samples_Octahedron_Common_Point__ctor((&__temp_0003), v1);
	(__this->First) = __temp_0003;
	Dntc_Samples_Octahedron_Common_Point__ctor((&__temp_000f), v2);
	(__this->Second) = __temp_000f;
	(__this->DeltaX) = (((float)((&(__this->Second))->X)) - ((float)((&(__this->First))->X)));
	(__this->DeltaY) = (((float)((&(__this->Second))->Y)) - ((float)((&(__this->First))->Y)));
	(__this->Slope) = ((__this->DeltaX) / (__this->DeltaY));
	return;
}

void Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle(Dntc_Samples_Octahedron_Common_Triangle triangle, SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, uint16_t color) {
	Dntc_Samples_Octahedron_Common_Vector3 top = {0};
	Dntc_Samples_Octahedron_Common_Vector3 mid = {0};
	Dntc_Samples_Octahedron_Common_Vector3 bottom = {0};
	Dntc_Samples_Octahedron_Common_PointPair topMidPair = {0};
	Dntc_Samples_Octahedron_Common_PointPair topBottomPair = {0};
	Dntc_Samples_Octahedron_Common_PointPair midBottomPair = {0};
	Dntc_Samples_Octahedron_Common_PointPair shortPair = {0};
	Dntc_Samples_Octahedron_Common_PointPair longPair = {0};
	float shortX = {0};
	float longX = {0};
	int32_t y = {0};
	float startCol = {0};
	bool __local_12 = {0};
	bool __local_13 = {0};
	bool __local_14 = {0};
	float diff = {0};
	float endCol = {0};
	int32_t index = {0};
	bool __local_18 = {0};
	int32_t x = {0};
	bool __local_20 = {0};
	bool __local_21 = {0};
	triangle = Dntc_Samples_Octahedron_Common_Triangle_SortPoints((&triangle));
	top = (triangle.V1);
	mid = (triangle.V2);
	bottom = (triangle.V3);
	Dntc_Samples_Octahedron_Common_PointPair__ctor((&topMidPair), top, mid);
	Dntc_Samples_Octahedron_Common_PointPair__ctor((&topBottomPair), top, bottom);
	Dntc_Samples_Octahedron_Common_PointPair__ctor((&midBottomPair), mid, bottom);
	shortPair = topMidPair;
	longPair = topBottomPair;
	shortX = (top.X);
	longX = (top.X);
	y = ((int32_t)(top.Y));
	goto Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_0157;

Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_005f:
	__local_12 = ((y < (camera.PixelHeight)) == 0);
	if (!__local_12) {
		goto Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_0079;
	}
	goto Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_016e;

Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_0079:
	__local_13 = (((float)y) == (mid.Y));
	if (!__local_13) {
		goto Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_009f;
	}
	shortPair = midBottomPair;
	shortX = ((float)((shortPair.First).X));

Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_009f:
	startCol = dn_min_float(shortX, longX);
	__local_14 = (startCol < ((float)(camera.PixelWidth)));
	if (!__local_14) {
		goto Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_0138;
	}
	diff = 0;
	__local_18 = (longX > shortX);
	if (!__local_18) {
		goto Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_00da;
	}
	diff = (longX - shortX);
	goto Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_00e3;

Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_00da:
	diff = (shortX - longX);

Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_00e3:
	endCol = dn_min_float((startCol + diff), ((float)((camera.PixelWidth) - 1)));
	diff = (endCol - startCol);
	index = ((int32_t)(((float)(y * (camera.PixelWidth))) + startCol));
	x = 0;
	goto Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_0127;

Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_0114:
	if ((pixels.length) <= index) {
		printf("Attempted to access to pixels[%d], but only %u items are in the array", index, (pixels.length));
		abort();
	}
	(pixels.items)[index] = color;
	index = (index + 1);
	x = (x + 1);

Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_0127:
	__local_20 = ((((float)x) > diff) == 0);
	if (__local_20) {
		goto Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_0114;
	}

Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_0138:
	shortX = (shortX + (shortPair.Slope));
	longX = (longX + (longPair.Slope));
	y = (y + 1);

Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_0157:
	__local_21 = ((((float)y) > (bottom.Y)) == 0);
	if (__local_21) {
		goto Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_005f;
	}

Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle_IL_016e:
	return;
}

int32_t Dntc_Samples_Octahedron_Common_OctahedronShape_get_TriangleCount(Dntc_Samples_Octahedron_Common_OctahedronShape *__this) {
	int32_t __return_value = {0};
	__return_value = 8;
	return __return_value;
}

void Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_OctahedronShape(SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, float secondsPassed, Dntc_Samples_Octahedron_Common_OctahedronShape shape) {
	Dntc_Samples_Octahedron_Common_Vector3 light = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotationsDegreesPerSecond = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotation = {0};
	int32_t x = {0};
	Dntc_Samples_Octahedron_Common_Triangle triangle = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotatedV1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotatedV2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotatedV3 = {0};
	Dntc_Samples_Octahedron_Common_Triangle rotatedTriangle = {0};
	Dntc_Samples_Octahedron_Common_Vector3 normal = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_10 = {0};
	bool __local_11 = {0};
	Dntc_Samples_Octahedron_Common_Triangle projectedTriangle = {0};
	float alignment = {0};
	uint8_t colorValue = {0};
	uint16_t color = {0};
	Dntc_Samples_Octahedron_Common_Vector3 p1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 p2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 p3 = {0};
	Dntc_Samples_Octahedron_Common_Triangle triangleToDraw = {0};
	bool __local_20 = {0};
	bool __local_21 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&light), 1, 0, 3);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&rotationsDegreesPerSecond), 0, 100, 120);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&rotation), ((rotationsDegreesPerSecond.X) * secondsPassed), ((rotationsDegreesPerSecond.Y) * secondsPassed), ((rotationsDegreesPerSecond.Z) * secondsPassed));
	x = 0;
	goto Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_OctahedronShape_IL_01de;

Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_OctahedronShape_IL_0053:
	triangle = Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle((&shape), x);
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&triangle)->V1)), (rotation.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (rotation.Y));
	rotatedV1 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (rotation.X));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&triangle)->V2)), (rotation.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (rotation.Y));
	rotatedV2 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (rotation.X));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&triangle)->V3)), (rotation.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (rotation.Y));
	rotatedV3 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (rotation.X));
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&rotatedTriangle), rotatedV1, rotatedV2, rotatedV3);
	__local_10 = Dntc_Samples_Octahedron_Common_Triangle_get_Normal((&rotatedTriangle));
	normal = Dntc_Samples_Octahedron_Common_Vector3_get_Unit((&__local_10));
	__local_11 = ((normal.Z) > 0);
	if (!__local_11) {
		goto Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_OctahedronShape_IL_01d9;
	}
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&projectedTriangle), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(rotatedV1, camera), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(rotatedV2, camera), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(rotatedV3, camera));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_get_Unit((&light));
	alignment = Dntc_Samples_Octahedron_Common_Vector3_Dot((&__local_10), normal);
	__local_20 = (alignment < 0);
	if (!__local_20) {
		goto Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_OctahedronShape_IL_017a;
	}
	alignment = 0;

Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_OctahedronShape_IL_017a:
	colorValue = ((uint8_t)(alignment * 255));
	color = Dntc_Samples_Octahedron_Common_Renderer_Rgb888To565(colorValue, colorValue, colorValue);
	p1 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((projectedTriangle.V1), camera);
	p2 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((projectedTriangle.V2), camera);
	p3 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((projectedTriangle.V3), camera);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&triangleToDraw), p1, p2, p3);
	Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle(triangleToDraw, pixels, camera, color);

Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_OctahedronShape_IL_01d9:
	x = (x + 1);

Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_OctahedronShape_IL_01de:
	__local_21 = (x < Dntc_Samples_Octahedron_Common_OctahedronShape_get_TriangleCount((&shape)));
	if (__local_21) {
		goto Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_OctahedronShape_IL_0053;
	}
	return;
}

Dntc_Samples_Octahedron_Common_Triangle Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle(Dntc_Samples_Octahedron_Common_PyramidShape *__this, int32_t index) {
	Dntc_Samples_Octahedron_Common_Triangle __return_value = {0};
	int32_t __local_0 = {0};
	int32_t __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_2 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_3 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0037 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_004b = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_005f = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_0064 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_007e = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0092 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00a6 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_00ab = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00c5 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00d9 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00ed = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_00f2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_010c = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0120 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0134 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_0139 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0153 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0167 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_017b = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_0180 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0197 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01ab = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01bf = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_01c4 = {0};
	__local_1 = index;
	__local_0 = __local_1;
	switch(__local_0) {
		case 0: goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_0028;
		case 1: goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_006f;
		case 2: goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_00b6;
		case 3: goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_00fd;
		case 4: goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_0144;
		case 5: goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_0188;
	}

	goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_01cc;

Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_0028:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0037), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_004b), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_005f), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0064), __temp_0037, __temp_004b, __temp_005f);
	__local_2 = __temp_0064;
	goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_01d8;

Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_006f:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_007e), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0092), -0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00a6), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_00ab), __temp_007e, __temp_0092, __temp_00a6);
	__local_2 = __temp_00ab;
	goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_01d8;

Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_00b6:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00c5), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00d9), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00ed), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_00f2), __temp_00c5, __temp_00d9, __temp_00ed);
	__local_2 = __temp_00f2;
	goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_01d8;

Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_00fd:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_010c), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0120), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0134), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0139), __temp_010c, __temp_0120, __temp_0134);
	__local_2 = __temp_0139;
	goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_01d8;

Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_0144:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0153), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0167), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_017b), -0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0180), __temp_0153, __temp_0167, __temp_017b);
	__local_2 = __temp_0180;
	goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_01d8;

Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_0188:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0197), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01ab), -0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01bf), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_01c4), __temp_0197, __temp_01ab, __temp_01bf);
	__local_2 = __temp_01c4;
	goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_01d8;

Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_01cc:
	(*(&__local_3)) = ((Dntc_Samples_Octahedron_Common_Triangle){0});
	__local_2 = __local_3;
	goto Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_01d8;

Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle_IL_01d8:
	__return_value = __local_2;
	return __return_value;
}

int32_t Dntc_Samples_Octahedron_Common_PyramidShape_get_TriangleCount(Dntc_Samples_Octahedron_Common_PyramidShape *__this) {
	int32_t __return_value = {0};
	__return_value = 6;
	return __return_value;
}

void Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_PyramidShape(SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, float secondsPassed, Dntc_Samples_Octahedron_Common_PyramidShape shape) {
	Dntc_Samples_Octahedron_Common_Vector3 light = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotationsDegreesPerSecond = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotation = {0};
	int32_t x = {0};
	Dntc_Samples_Octahedron_Common_Triangle triangle = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotatedV1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotatedV2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotatedV3 = {0};
	Dntc_Samples_Octahedron_Common_Triangle rotatedTriangle = {0};
	Dntc_Samples_Octahedron_Common_Vector3 normal = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_10 = {0};
	bool __local_11 = {0};
	Dntc_Samples_Octahedron_Common_Triangle projectedTriangle = {0};
	float alignment = {0};
	uint8_t colorValue = {0};
	uint16_t color = {0};
	Dntc_Samples_Octahedron_Common_Vector3 p1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 p2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 p3 = {0};
	Dntc_Samples_Octahedron_Common_Triangle triangleToDraw = {0};
	bool __local_20 = {0};
	bool __local_21 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&light), 1, 0, 3);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&rotationsDegreesPerSecond), 0, 100, 120);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&rotation), ((rotationsDegreesPerSecond.X) * secondsPassed), ((rotationsDegreesPerSecond.Y) * secondsPassed), ((rotationsDegreesPerSecond.Z) * secondsPassed));
	x = 0;
	goto Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_PyramidShape_IL_01de;

Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_PyramidShape_IL_0053:
	triangle = Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle((&shape), x);
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&triangle)->V1)), (rotation.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (rotation.Y));
	rotatedV1 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (rotation.X));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&triangle)->V2)), (rotation.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (rotation.Y));
	rotatedV2 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (rotation.X));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&triangle)->V3)), (rotation.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (rotation.Y));
	rotatedV3 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (rotation.X));
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&rotatedTriangle), rotatedV1, rotatedV2, rotatedV3);
	__local_10 = Dntc_Samples_Octahedron_Common_Triangle_get_Normal((&rotatedTriangle));
	normal = Dntc_Samples_Octahedron_Common_Vector3_get_Unit((&__local_10));
	__local_11 = ((normal.Z) > 0);
	if (!__local_11) {
		goto Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_PyramidShape_IL_01d9;
	}
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&projectedTriangle), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(rotatedV1, camera), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(rotatedV2, camera), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(rotatedV3, camera));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_get_Unit((&light));
	alignment = Dntc_Samples_Octahedron_Common_Vector3_Dot((&__local_10), normal);
	__local_20 = (alignment < 0);
	if (!__local_20) {
		goto Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_PyramidShape_IL_017a;
	}
	alignment = 0;

Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_PyramidShape_IL_017a:
	colorValue = ((uint8_t)(alignment * 255));
	color = Dntc_Samples_Octahedron_Common_Renderer_Rgb888To565(colorValue, colorValue, colorValue);
	p1 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((projectedTriangle.V1), camera);
	p2 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((projectedTriangle.V2), camera);
	p3 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((projectedTriangle.V3), camera);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&triangleToDraw), p1, p2, p3);
	Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle(triangleToDraw, pixels, camera, color);

Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_PyramidShape_IL_01d9:
	x = (x + 1);

Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_PyramidShape_IL_01de:
	__local_21 = (x < Dntc_Samples_Octahedron_Common_PyramidShape_get_TriangleCount((&shape)));
	if (__local_21) {
		goto Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_PyramidShape_IL_0053;
	}
	return;
}

Dntc_Samples_Octahedron_Common_Triangle Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle(Dntc_Samples_Octahedron_Common_CubeShape *__this, int32_t index) {
	Dntc_Samples_Octahedron_Common_Triangle __return_value = {0};
	int32_t __local_0 = {0};
	int32_t __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_2 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_3 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_004f = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0063 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0077 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_007c = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0096 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00aa = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00be = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_00c3 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00dd = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00f1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0105 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_010a = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0124 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0138 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_014c = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_0151 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_016b = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_017f = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0193 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_0198 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01b2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01c6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01da = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_01df = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01f9 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_020d = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0221 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_0226 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0240 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0254 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0268 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_026d = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0287 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_029b = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_02af = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_02b4 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_02ce = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_02e2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_02f6 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_02fb = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0315 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0329 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_033d = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_0342 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0359 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_036d = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0381 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __temp_0386 = {0};
	__local_1 = index;
	__local_0 = __local_1;
	switch(__local_0) {
		case 0: goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_0040;
		case 1: goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_0087;
		case 2: goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_00ce;
		case 3: goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_0115;
		case 4: goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_015c;
		case 5: goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_01a3;
		case 6: goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_01ea;
		case 7: goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_0231;
		case 8: goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_0278;
		case 9: goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_02bf;
		case 10: goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_0306;
		case 11: goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_034a;
	}

	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_038e;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_0040:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_004f), 0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0063), -0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0077), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_007c), __temp_004f, __temp_0063, __temp_0077);
	__local_2 = __temp_007c;
	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_0087:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0096), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00aa), -0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00be), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_00c3), __temp_0096, __temp_00aa, __temp_00be);
	__local_2 = __temp_00c3;
	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_00ce:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00dd), 0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00f1), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0105), 0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_010a), __temp_00dd, __temp_00f1, __temp_0105);
	__local_2 = __temp_010a;
	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_0115:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0124), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0138), 0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_014c), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0151), __temp_0124, __temp_0138, __temp_014c);
	__local_2 = __temp_0151;
	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_015c:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_016b), 0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_017f), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0193), -0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0198), __temp_016b, __temp_017f, __temp_0193);
	__local_2 = __temp_0198;
	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_01a3:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01b2), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01c6), -0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01da), -0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_01df), __temp_01b2, __temp_01c6, __temp_01da);
	__local_2 = __temp_01df;
	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_01ea:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01f9), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_020d), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0221), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0226), __temp_01f9, __temp_020d, __temp_0221);
	__local_2 = __temp_0226;
	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_0231:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0240), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0254), -0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0268), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_026d), __temp_0240, __temp_0254, __temp_0268);
	__local_2 = __temp_026d;
	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_0278:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0287), -0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_029b), -0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_02af), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_02b4), __temp_0287, __temp_029b, __temp_02af);
	__local_2 = __temp_02b4;
	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_02bf:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_02ce), -0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_02e2), -0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_02f6), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_02fb), __temp_02ce, __temp_02e2, __temp_02f6);
	__local_2 = __temp_02fb;
	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_0306:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0315), -0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0329), 0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_033d), 0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0342), __temp_0315, __temp_0329, __temp_033d);
	__local_2 = __temp_0342;
	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_034a:
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0359), 0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_036d), -0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0381), -0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0386), __temp_0359, __temp_036d, __temp_0381);
	__local_2 = __temp_0386;
	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_038e:
	(*(&__local_3)) = ((Dntc_Samples_Octahedron_Common_Triangle){0});
	__local_2 = __local_3;
	goto Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a;

Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle_IL_039a:
	__return_value = __local_2;
	return __return_value;
}

int32_t Dntc_Samples_Octahedron_Common_CubeShape_get_TriangleCount(Dntc_Samples_Octahedron_Common_CubeShape *__this) {
	int32_t __return_value = {0};
	__return_value = 12;
	return __return_value;
}

void Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_CubeShape(SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, float secondsPassed, Dntc_Samples_Octahedron_Common_CubeShape shape) {
	Dntc_Samples_Octahedron_Common_Vector3 light = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotationsDegreesPerSecond = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotation = {0};
	int32_t x = {0};
	Dntc_Samples_Octahedron_Common_Triangle triangle = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotatedV1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotatedV2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 rotatedV3 = {0};
	Dntc_Samples_Octahedron_Common_Triangle rotatedTriangle = {0};
	Dntc_Samples_Octahedron_Common_Vector3 normal = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_10 = {0};
	bool __local_11 = {0};
	Dntc_Samples_Octahedron_Common_Triangle projectedTriangle = {0};
	float alignment = {0};
	uint8_t colorValue = {0};
	uint16_t color = {0};
	Dntc_Samples_Octahedron_Common_Vector3 p1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 p2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 p3 = {0};
	Dntc_Samples_Octahedron_Common_Triangle triangleToDraw = {0};
	bool __local_20 = {0};
	bool __local_21 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&light), 1, 0, 3);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&rotationsDegreesPerSecond), 0, 100, 120);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&rotation), ((rotationsDegreesPerSecond.X) * secondsPassed), ((rotationsDegreesPerSecond.Y) * secondsPassed), ((rotationsDegreesPerSecond.Z) * secondsPassed));
	x = 0;
	goto Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_CubeShape_IL_01de;

Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_CubeShape_IL_0053:
	triangle = Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle((&shape), x);
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&triangle)->V1)), (rotation.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (rotation.Y));
	rotatedV1 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (rotation.X));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&triangle)->V2)), (rotation.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (rotation.Y));
	rotatedV2 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (rotation.X));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&triangle)->V3)), (rotation.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (rotation.Y));
	rotatedV3 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (rotation.X));
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&rotatedTriangle), rotatedV1, rotatedV2, rotatedV3);
	__local_10 = Dntc_Samples_Octahedron_Common_Triangle_get_Normal((&rotatedTriangle));
	normal = Dntc_Samples_Octahedron_Common_Vector3_get_Unit((&__local_10));
	__local_11 = ((normal.Z) > 0);
	if (!__local_11) {
		goto Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_CubeShape_IL_01d9;
	}
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&projectedTriangle), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(rotatedV1, camera), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(rotatedV2, camera), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(rotatedV3, camera));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_get_Unit((&light));
	alignment = Dntc_Samples_Octahedron_Common_Vector3_Dot((&__local_10), normal);
	__local_20 = (alignment < 0);
	if (!__local_20) {
		goto Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_CubeShape_IL_017a;
	}
	alignment = 0;

Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_CubeShape_IL_017a:
	colorValue = ((uint8_t)(alignment * 255));
	color = Dntc_Samples_Octahedron_Common_Renderer_Rgb888To565(colorValue, colorValue, colorValue);
	p1 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((projectedTriangle.V1), camera);
	p2 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((projectedTriangle.V2), camera);
	p3 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((projectedTriangle.V3), camera);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&triangleToDraw), p1, p2, p3);
	Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle(triangleToDraw, pixels, camera, color);

Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_CubeShape_IL_01d9:
	x = (x + 1);

Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_CubeShape_IL_01de:
	__local_21 = (x < Dntc_Samples_Octahedron_Common_CubeShape_get_TriangleCount((&shape)));
	if (__local_21) {
		goto Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_CubeShape_IL_0053;
	}
	return;
}

void Dntc_Samples_Octahedron_Common_Renderer_Render(SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, float secondsPassed) {
	int32_t fullSeconds = {0};
	bool __local_1 = {0};
	Dntc_Samples_Octahedron_Common_OctahedronShape __local_2 = {0};
	bool __local_3 = {0};
	Dntc_Samples_Octahedron_Common_PyramidShape __local_4 = {0};
	Dntc_Samples_Octahedron_Common_CubeShape __local_5 = {0};
	fullSeconds = ((int32_t)(((double)secondsPassed) / 1.5));
	__local_1 = ((fullSeconds % 3) == 0);
	if (!__local_1) {
		goto Dntc_Samples_Octahedron_Common_Renderer_Render_IL_002f;
	}
	(*(&__local_2)) = ((Dntc_Samples_Octahedron_Common_OctahedronShape){0});
	Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_OctahedronShape(pixels, camera, secondsPassed, __local_2);
	goto Dntc_Samples_Octahedron_Common_Renderer_Render_IL_0065;

Dntc_Samples_Octahedron_Common_Renderer_Render_IL_002f:
	__local_3 = ((fullSeconds % 3) == 1);
	if (!__local_3) {
		goto Dntc_Samples_Octahedron_Common_Renderer_Render_IL_0050;
	}
	(*(&__local_4)) = ((Dntc_Samples_Octahedron_Common_PyramidShape){0});
	Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_PyramidShape(pixels, camera, secondsPassed, __local_4);
	goto Dntc_Samples_Octahedron_Common_Renderer_Render_IL_0065;

Dntc_Samples_Octahedron_Common_Renderer_Render_IL_0050:
	(*(&__local_5)) = ((Dntc_Samples_Octahedron_Common_CubeShape){0});
	Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_CubeShape(pixels, camera, secondsPassed, __local_5);

Dntc_Samples_Octahedron_Common_Renderer_Render_IL_0065:
	return;
}
