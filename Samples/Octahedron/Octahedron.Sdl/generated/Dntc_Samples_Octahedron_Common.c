#include <math.h>
#include <stdbool.h>
#include <stdint.h>
#include "Dntc_Samples_Octahedron_Common.h"
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
	Dntc_Samples_Octahedron_Common_Camera __local_0 = {0};
	Dntc_Samples_Octahedron_Common_Camera __local_1 = {0};
	(*(&__local_0)) = ((Dntc_Samples_Octahedron_Common_Camera){0});
	Dntc_Samples_Octahedron_Common_Vector3 __temp_001a = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_001a), 1, 0, 0);
	((&__local_0)->Right) = __temp_001a;
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0035 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0035), 0, 1, 0);
	((&__local_0)->Up) = __temp_0035;
	__local_1 = __local_0;
	goto IL_0043;

IL_0043:
	return __local_1;
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
	int32_t __local_0 = {0};
	int32_t __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_2 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_3 = {0};
	__local_1 = index;
	__local_0 = __local_1;
	switch(__local_0) {
		case 0: goto IL_0030;
		case 1: goto IL_0077;
		case 2: goto IL_00be;
		case 3: goto IL_0105;
		case 4: goto IL_014c;
		case 5: goto IL_0193;
		case 6: goto IL_01da;
		case 7: goto IL_021e;
	}

	goto IL_0262;

IL_0030:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_003f = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_003f), 1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0053 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0053), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0067 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0067), 0, 0, 1);
	Dntc_Samples_Octahedron_Common_Triangle __temp_006c = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_006c), __temp_003f, __temp_0053, __temp_0067);
	__local_2 = __temp_006c;
	goto IL_026e;

IL_0077:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0086 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0086), 1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_009a = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_009a), 0, 0, -1);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00ae = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00ae), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Triangle __temp_00b3 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_00b3), __temp_0086, __temp_009a, __temp_00ae);
	__local_2 = __temp_00b3;
	goto IL_026e;

IL_00be:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00cd = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00cd), 1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00e1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00e1), 0, 0, 1);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00f5 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00f5), 0, -1, 0);
	Dntc_Samples_Octahedron_Common_Triangle __temp_00fa = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_00fa), __temp_00cd, __temp_00e1, __temp_00f5);
	__local_2 = __temp_00fa;
	goto IL_026e;

IL_0105:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0114 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0114), 1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0128 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0128), 0, -1, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_013c = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_013c), 0, 0, -1);
	Dntc_Samples_Octahedron_Common_Triangle __temp_0141 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0141), __temp_0114, __temp_0128, __temp_013c);
	__local_2 = __temp_0141;
	goto IL_026e;

IL_014c:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_015b = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_015b), -1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_016f = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_016f), 0, 0, 1);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0183 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0183), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Triangle __temp_0188 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0188), __temp_015b, __temp_016f, __temp_0183);
	__local_2 = __temp_0188;
	goto IL_026e;

IL_0193:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01a2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01a2), -1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01b6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01b6), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01ca = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01ca), 0, 0, -1);
	Dntc_Samples_Octahedron_Common_Triangle __temp_01cf = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_01cf), __temp_01a2, __temp_01b6, __temp_01ca);
	__local_2 = __temp_01cf;
	goto IL_026e;

IL_01da:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01e9 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01e9), -1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01fd = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01fd), 0, -1, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0211 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0211), 0, 0, 1);
	Dntc_Samples_Octahedron_Common_Triangle __temp_0216 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0216), __temp_01e9, __temp_01fd, __temp_0211);
	__local_2 = __temp_0216;
	goto IL_026e;

IL_021e:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_022d = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_022d), -1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0241 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0241), 0, 0, -1);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0255 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0255), 0, -1, 0);
	Dntc_Samples_Octahedron_Common_Triangle __temp_025a = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_025a), __temp_022d, __temp_0241, __temp_0255);
	__local_2 = __temp_025a;
	goto IL_026e;

IL_0262:
	(*(&__local_3)) = ((Dntc_Samples_Octahedron_Common_Triangle){0});
	__local_2 = __local_3;
	goto IL_026e;

IL_026e:
	return __local_2;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ(Dntc_Samples_Octahedron_Common_Vector3 *__this, float degrees) {
	double __local_0 = {0};
	double __local_1 = {0};
	float __local_2 = {0};
	double __local_3 = {0};
	float __local_4 = {0};
	float __local_5 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_6 = {0};
	__local_0 = ((((double)degrees) * 3.141592653589793) / 180);
	__local_1 = atan2(((double)(__this->Y)), ((double)(__this->X)));
	__local_2 = ((float)sqrt(((double)(((__this->X) * (__this->X)) + ((__this->Y) * (__this->Y))))));
	__local_3 = (__local_1 + __local_0);
	__local_4 = (((float)cos(__local_3)) * __local_2);
	__local_5 = (((float)sin(__local_3)) * __local_2);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0073 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0073), __local_4, __local_5, (__this->Z));
	__local_6 = __temp_0073;
	goto IL_007c;

IL_007c:
	return __local_6;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_RotateOnY(Dntc_Samples_Octahedron_Common_Vector3 *__this, float degrees) {
	double __local_0 = {0};
	double __local_1 = {0};
	float __local_2 = {0};
	double __local_3 = {0};
	float __local_4 = {0};
	float __local_5 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_6 = {0};
	__local_0 = ((((double)degrees) * 3.141592653589793) / 180);
	__local_1 = atan2(((double)(__this->Z)), ((double)(__this->X)));
	__local_2 = ((float)sqrt(((double)(((__this->X) * (__this->X)) + ((__this->Z) * (__this->Z))))));
	__local_3 = (__local_1 + __local_0);
	__local_4 = (((float)cos(__local_3)) * __local_2);
	__local_5 = (((float)sin(__local_3)) * __local_2);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0073 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0073), __local_4, (__this->Y), __local_5);
	__local_6 = __temp_0073;
	goto IL_007c;

IL_007c:
	return __local_6;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_RotateOnX(Dntc_Samples_Octahedron_Common_Vector3 *__this, float degrees) {
	double __local_0 = {0};
	double __local_1 = {0};
	float __local_2 = {0};
	double __local_3 = {0};
	float __local_4 = {0};
	float __local_5 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_6 = {0};
	__local_0 = ((((double)degrees) * 3.141592653589793) / 180);
	__local_1 = atan2(((double)(__this->Y)), ((double)(__this->Z)));
	__local_2 = ((float)sqrt(((double)(((__this->Z) * (__this->Z)) + ((__this->Y) * (__this->Y))))));
	__local_3 = (__local_1 + __local_0);
	__local_4 = (((float)cos(__local_3)) * __local_2);
	__local_5 = (((float)sin(__local_3)) * __local_2);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0073 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0073), (__this->X), __local_5, __local_4);
	__local_6 = __temp_0073;
	goto IL_007c;

IL_007c:
	return __local_6;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_op_Subtraction(Dntc_Samples_Octahedron_Common_Vector3 first, Dntc_Samples_Octahedron_Common_Vector3 second) {
	Dntc_Samples_Octahedron_Common_Vector3 __local_0 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0028 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0028), ((first.X) - (second.X)), ((first.Y) - (second.Y)), ((first.Z) - (second.Z)));
	__local_0 = __temp_0028;
	goto IL_0030;

IL_0030:
	return __local_0;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_Cross(Dntc_Samples_Octahedron_Common_Vector3 *__this, Dntc_Samples_Octahedron_Common_Vector3 other) {
	float __local_0 = {0};
	float __local_1 = {0};
	float __local_2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_3 = {0};
	__local_0 = (((__this->Y) * (other.Z)) - ((__this->Z) * (other.Y)));
	__local_1 = (((__this->Z) * (other.X)) - ((__this->X) * (other.Z)));
	__local_2 = (((__this->X) * (other.Y)) - ((__this->Y) * (other.X)));
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0058 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0058), __local_0, __local_1, __local_2);
	__local_3 = __temp_0058;
	goto IL_0060;

IL_0060:
	return __local_3;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Triangle_get_Normal(Dntc_Samples_Octahedron_Common_Triangle *__this) {
	Dntc_Samples_Octahedron_Common_Vector3 __local_0 = {0};
	__local_0 = Dntc_Samples_Octahedron_Common_Vector3_op_Subtraction((__this->V2), (__this->V1));
	return Dntc_Samples_Octahedron_Common_Vector3_Cross((&__local_0), Dntc_Samples_Octahedron_Common_Vector3_op_Subtraction((__this->V3), (__this->V1)));
}

float Dntc_Samples_Octahedron_Common_Vector3_get_Length(Dntc_Samples_Octahedron_Common_Vector3 *__this) {
	return ((float)sqrt(((double)((((__this->X) * (__this->X)) + ((__this->Y) * (__this->Y))) + ((__this->Z) * (__this->Z))))));
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_op_Multiply(Dntc_Samples_Octahedron_Common_Vector3 vec, float scalar) {
	Dntc_Samples_Octahedron_Common_Vector3 __local_0 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0019 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0019), ((vec.X) * scalar), ((vec.Y) * scalar), ((vec.Z) * scalar));
	__local_0 = __temp_0019;
	goto IL_0021;

IL_0021:
	return __local_0;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_get_Unit(Dntc_Samples_Octahedron_Common_Vector3 *__this) {
	return Dntc_Samples_Octahedron_Common_Vector3_op_Multiply((*__this), (1 / Dntc_Samples_Octahedron_Common_Vector3_get_Length(__this)));
}

float Dntc_Samples_Octahedron_Common_Vector3_Dot(Dntc_Samples_Octahedron_Common_Vector3 *__this, Dntc_Samples_Octahedron_Common_Vector3 other) {
	float __local_0 = {0};
	__local_0 = ((((__this->X) * (other.X)) + ((__this->Y) * (other.Y))) + ((__this->Z) * (other.Z)));
	goto IL_002d;

IL_002d:
	return __local_0;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(Dntc_Samples_Octahedron_Common_Vector3 vector, Dntc_Samples_Octahedron_Common_Camera camera) {
	float __local_0 = {0};
	float __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_2 = {0};
	__local_0 = (Dntc_Samples_Octahedron_Common_Vector3_Dot((&vector), (camera.Right)) / Dntc_Samples_Octahedron_Common_Vector3_get_Length((&((&camera)->Right))));
	__local_1 = (Dntc_Samples_Octahedron_Common_Vector3_Dot((&vector), (camera.Up)) / Dntc_Samples_Octahedron_Common_Vector3_get_Length((&((&camera)->Up))));
	Dntc_Samples_Octahedron_Common_Vector3 __temp_003e = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_003e), __local_0, __local_1, 0);
	__local_2 = __temp_003e;
	goto IL_0046;

IL_0046:
	return __local_2;
}

uint16_t Dntc_Samples_Octahedron_Common_Renderer_Rgb888To565(uint8_t red, uint8_t green, uint8_t blue) {
	uint16_t __local_0 = {0};
	red = ((uint8_t)(red / 8));
	green = ((uint8_t)(green / 4));
	blue = ((uint8_t)(blue / 8));
	__local_0 = ((uint16_t)(((red << 11) | (green << 5)) | blue));
	goto IL_0021;

IL_0021:
	return __local_0;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Renderer_ToScreen(Dntc_Samples_Octahedron_Common_Vector3 vector, Dntc_Samples_Octahedron_Common_Camera camera) {
	uint16_t __local_0 = {0};
	uint16_t __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_2 = {0};
	__local_0 = ((uint16_t)(((vector.X) * 100) + ((float)((camera.PixelWidth) / 2))));
	__local_1 = ((uint16_t)(((vector.Y) * 100) + ((float)((camera.PixelHeight) / 2))));
	Dntc_Samples_Octahedron_Common_Vector3 __temp_003a = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_003a), ((float)__local_0), ((float)__local_1), 0);
	__local_2 = __temp_003a;
	goto IL_0042;

IL_0042:
	return __local_2;
}

Dntc_Samples_Octahedron_Common_Triangle Dntc_Samples_Octahedron_Common_Triangle_SortPoints(Dntc_Samples_Octahedron_Common_Triangle *__this) {
	Dntc_Samples_Octahedron_Common_Vector3 __local_0 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_3 = {0};
	bool __local_4 = {0};
	bool __local_5 = {0};
	bool __local_6 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_7 = {0};
	__local_0 = (__this->V1);
	__local_1 = (__this->V2);
	__local_2 = (__this->V3);
	__local_4 = ((__local_0.Y) > (__local_1.Y));
	if (!__local_4) {
		goto IL_0032;
	}
	__local_3 = __local_0;
	__local_0 = __local_1;
	__local_1 = __local_3;

IL_0032:
	__local_5 = ((__local_2.Y) < (__local_0.Y));
	if (!__local_5) {
		goto IL_0052;
	}
	__local_3 = __local_2;
	__local_2 = __local_1;
	__local_1 = __local_0;
	__local_0 = __local_3;
	goto IL_006e;

IL_0052:
	__local_6 = ((__local_2.Y) < (__local_1.Y));
	if (!__local_6) {
		goto IL_006e;
	}
	__local_3 = __local_2;
	__local_2 = __local_1;
	__local_1 = __local_3;

IL_006e:
	Dntc_Samples_Octahedron_Common_Triangle __temp_0071 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0071), __local_0, __local_1, __local_2);
	__local_7 = __temp_0071;
	goto IL_007a;

IL_007a:
	return __local_7;
}

void Dntc_Samples_Octahedron_Common_Point__ctor(Dntc_Samples_Octahedron_Common_Point *__this, Dntc_Samples_Octahedron_Common_Vector3 vector) {
	(__this->X) = ((int32_t)(vector.X));
	(__this->Y) = ((int32_t)(vector.Y));
	return;
}

void Dntc_Samples_Octahedron_Common_PointPair__ctor(Dntc_Samples_Octahedron_Common_PointPair *__this, Dntc_Samples_Octahedron_Common_Vector3 v1, Dntc_Samples_Octahedron_Common_Vector3 v2) {
	Dntc_Samples_Octahedron_Common_Point __temp_0003 = {0};
	Dntc_Samples_Octahedron_Common_Point__ctor((&__temp_0003), v1);
	(__this->First) = __temp_0003;
	Dntc_Samples_Octahedron_Common_Point __temp_000f = {0};
	Dntc_Samples_Octahedron_Common_Point__ctor((&__temp_000f), v2);
	(__this->Second) = __temp_000f;
	(__this->DeltaX) = (((float)((&(__this->Second))->X)) - ((float)((&(__this->First))->X)));
	(__this->DeltaY) = (((float)((&(__this->Second))->Y)) - ((float)((&(__this->First))->Y)));
	(__this->Slope) = ((__this->DeltaX) / (__this->DeltaY));
	return;
}

void Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle(Dntc_Samples_Octahedron_Common_Triangle triangle, SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, uint16_t color) {
	Dntc_Samples_Octahedron_Common_Vector3 __local_0 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_2 = {0};
	Dntc_Samples_Octahedron_Common_PointPair __local_3 = {0};
	Dntc_Samples_Octahedron_Common_PointPair __local_4 = {0};
	Dntc_Samples_Octahedron_Common_PointPair __local_5 = {0};
	Dntc_Samples_Octahedron_Common_PointPair __local_6 = {0};
	Dntc_Samples_Octahedron_Common_PointPair __local_7 = {0};
	float __local_8 = {0};
	float __local_9 = {0};
	int32_t __local_10 = {0};
	float __local_11 = {0};
	bool __local_12 = {0};
	bool __local_13 = {0};
	bool __local_14 = {0};
	float __local_15 = {0};
	float __local_16 = {0};
	int32_t __local_17 = {0};
	bool __local_18 = {0};
	int32_t __local_19 = {0};
	bool __local_20 = {0};
	bool __local_21 = {0};
	triangle = Dntc_Samples_Octahedron_Common_Triangle_SortPoints((&triangle));
	__local_0 = (triangle.V1);
	__local_1 = (triangle.V2);
	__local_2 = (triangle.V3);
	Dntc_Samples_Octahedron_Common_PointPair__ctor((&__local_3), __local_0, __local_1);
	Dntc_Samples_Octahedron_Common_PointPair__ctor((&__local_4), __local_0, __local_2);
	Dntc_Samples_Octahedron_Common_PointPair__ctor((&__local_5), __local_1, __local_2);
	__local_6 = __local_3;
	__local_7 = __local_4;
	__local_8 = (__local_0.X);
	__local_9 = (__local_0.X);
	__local_10 = ((int32_t)(__local_0.Y));
	goto IL_0157;

IL_005f:
	__local_12 = ((__local_10 < (camera.PixelHeight)) == 0);
	if (!__local_12) {
		goto IL_0079;
	}
	goto IL_016e;

IL_0079:
	__local_13 = (((float)__local_10) == (__local_1.Y));
	if (!__local_13) {
		goto IL_009f;
	}
	__local_6 = __local_5;
	__local_8 = ((float)((__local_6.First).X));

IL_009f:
	__local_11 = dn_min_float(__local_8, __local_9);
	__local_14 = (__local_11 < ((float)(camera.PixelWidth)));
	if (!__local_14) {
		goto IL_0138;
	}
	__local_15 = 0;
	__local_18 = (__local_9 > __local_8);
	if (!__local_18) {
		goto IL_00da;
	}
	__local_15 = (__local_9 - __local_8);
	goto IL_00e3;

IL_00da:
	__local_15 = (__local_8 - __local_9);

IL_00e3:
	__local_16 = dn_min_float((__local_11 + __local_15), ((float)((camera.PixelWidth) - 1)));
	__local_15 = (__local_16 - __local_11);
	__local_17 = ((int32_t)(((float)(__local_10 * (camera.PixelWidth))) + __local_11));
	__local_19 = 0;
	goto IL_0127;

IL_0114:
	if ((pixels.length) <= __local_17) {
		printf("ATtempted to write to pixels[%ld], but only %zu items are in the array", __local_17, (pixels.length));
		abort();
	}
	(pixels.items)[__local_17] = color;
	__local_17 = (__local_17 + 1);
	__local_19 = (__local_19 + 1);

IL_0127:
	__local_20 = ((((float)__local_19) > __local_15) == 0);
	if (__local_20) {
		goto IL_0114;
	}

IL_0138:
	__local_8 = (__local_8 + (__local_6.Slope));
	__local_9 = (__local_9 + (__local_7.Slope));
	__local_10 = (__local_10 + 1);

IL_0157:
	__local_21 = ((((float)__local_10) > (__local_2.Y)) == 0);
	if (__local_21) {
		goto IL_005f;
	}

IL_016e:
	return;
}

int32_t Dntc_Samples_Octahedron_Common_OctahedronShape_get_TriangleCount(Dntc_Samples_Octahedron_Common_OctahedronShape *__this) {
	return 8;
}

void Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_OctahedronShape(SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, float secondsPassed, Dntc_Samples_Octahedron_Common_OctahedronShape shape) {
	Dntc_Samples_Octahedron_Common_Vector3 __local_0 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_2 = {0};
	int32_t __local_3 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_4 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_5 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_7 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_8 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_9 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_10 = {0};
	bool __local_11 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_12 = {0};
	float __local_13 = {0};
	uint8_t __local_14 = {0};
	uint16_t __local_15 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_16 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_17 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_18 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_19 = {0};
	bool __local_20 = {0};
	bool __local_21 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__local_0), 1, 0, 3);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__local_1), 0, 100, 120);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__local_2), ((__local_1.X) * secondsPassed), ((__local_1.Y) * secondsPassed), ((__local_1.Z) * secondsPassed));
	__local_3 = 0;
	goto IL_01de;

IL_0053:
	__local_4 = Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle((&shape), __local_3);
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&__local_4)->V1)), (__local_2.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (__local_2.Y));
	__local_5 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (__local_2.X));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&__local_4)->V2)), (__local_2.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (__local_2.Y));
	__local_6 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (__local_2.X));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&__local_4)->V3)), (__local_2.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (__local_2.Y));
	__local_7 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (__local_2.X));
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__local_8), __local_5, __local_6, __local_7);
	__local_10 = Dntc_Samples_Octahedron_Common_Triangle_get_Normal((&__local_8));
	__local_9 = Dntc_Samples_Octahedron_Common_Vector3_get_Unit((&__local_10));
	__local_11 = ((__local_9.Z) > 0);
	if (!__local_11) {
		goto IL_01d9;
	}
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__local_12), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(__local_5, camera), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(__local_6, camera), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(__local_7, camera));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_get_Unit((&__local_0));
	__local_13 = Dntc_Samples_Octahedron_Common_Vector3_Dot((&__local_10), __local_9);
	__local_20 = (__local_13 < 0);
	if (!__local_20) {
		goto IL_017a;
	}
	__local_13 = 0;

IL_017a:
	__local_14 = ((uint8_t)(__local_13 * 255));
	__local_15 = Dntc_Samples_Octahedron_Common_Renderer_Rgb888To565(__local_14, __local_14, __local_14);
	__local_16 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((__local_12.V1), camera);
	__local_17 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((__local_12.V2), camera);
	__local_18 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((__local_12.V3), camera);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__local_19), __local_16, __local_17, __local_18);
	Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle(__local_19, pixels, camera, __local_15);

IL_01d9:
	__local_3 = (__local_3 + 1);

IL_01de:
	__local_21 = (__local_3 < Dntc_Samples_Octahedron_Common_OctahedronShape_get_TriangleCount((&shape)));
	if (__local_21) {
		goto IL_0053;
	}
	return;
}

Dntc_Samples_Octahedron_Common_Triangle Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle(Dntc_Samples_Octahedron_Common_PyramidShape *__this, int32_t index) {
	int32_t __local_0 = {0};
	int32_t __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_2 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_3 = {0};
	__local_1 = index;
	__local_0 = __local_1;
	switch(__local_0) {
		case 0: goto IL_0028;
		case 1: goto IL_006f;
		case 2: goto IL_00b6;
		case 3: goto IL_00fd;
		case 4: goto IL_0144;
		case 5: goto IL_0188;
	}

	goto IL_01cc;

IL_0028:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0037 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0037), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_004b = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_004b), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_005f = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_005f), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_0064 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0064), __temp_0037, __temp_004b, __temp_005f);
	__local_2 = __temp_0064;
	goto IL_01d8;

IL_006f:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_007e = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_007e), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0092 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0092), -0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00a6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00a6), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_00ab = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_00ab), __temp_007e, __temp_0092, __temp_00a6);
	__local_2 = __temp_00ab;
	goto IL_01d8;

IL_00b6:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00c5 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00c5), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00d9 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00d9), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00ed = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00ed), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_00f2 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_00f2), __temp_00c5, __temp_00d9, __temp_00ed);
	__local_2 = __temp_00f2;
	goto IL_01d8;

IL_00fd:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_010c = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_010c), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0120 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0120), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0134 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0134), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_0139 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0139), __temp_010c, __temp_0120, __temp_0134);
	__local_2 = __temp_0139;
	goto IL_01d8;

IL_0144:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0153 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0153), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0167 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0167), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_017b = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_017b), -0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_0180 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0180), __temp_0153, __temp_0167, __temp_017b);
	__local_2 = __temp_0180;
	goto IL_01d8;

IL_0188:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0197 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0197), 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01ab = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01ab), -0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01bf = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01bf), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_01c4 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_01c4), __temp_0197, __temp_01ab, __temp_01bf);
	__local_2 = __temp_01c4;
	goto IL_01d8;

IL_01cc:
	(*(&__local_3)) = ((Dntc_Samples_Octahedron_Common_Triangle){0});
	__local_2 = __local_3;
	goto IL_01d8;

IL_01d8:
	return __local_2;
}

int32_t Dntc_Samples_Octahedron_Common_PyramidShape_get_TriangleCount(Dntc_Samples_Octahedron_Common_PyramidShape *__this) {
	return 6;
}

void Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_PyramidShape(SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, float secondsPassed, Dntc_Samples_Octahedron_Common_PyramidShape shape) {
	Dntc_Samples_Octahedron_Common_Vector3 __local_0 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_2 = {0};
	int32_t __local_3 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_4 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_5 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_7 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_8 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_9 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_10 = {0};
	bool __local_11 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_12 = {0};
	float __local_13 = {0};
	uint8_t __local_14 = {0};
	uint16_t __local_15 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_16 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_17 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_18 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_19 = {0};
	bool __local_20 = {0};
	bool __local_21 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__local_0), 1, 0, 3);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__local_1), 0, 100, 120);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__local_2), ((__local_1.X) * secondsPassed), ((__local_1.Y) * secondsPassed), ((__local_1.Z) * secondsPassed));
	__local_3 = 0;
	goto IL_01de;

IL_0053:
	__local_4 = Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle((&shape), __local_3);
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&__local_4)->V1)), (__local_2.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (__local_2.Y));
	__local_5 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (__local_2.X));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&__local_4)->V2)), (__local_2.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (__local_2.Y));
	__local_6 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (__local_2.X));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&__local_4)->V3)), (__local_2.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (__local_2.Y));
	__local_7 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (__local_2.X));
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__local_8), __local_5, __local_6, __local_7);
	__local_10 = Dntc_Samples_Octahedron_Common_Triangle_get_Normal((&__local_8));
	__local_9 = Dntc_Samples_Octahedron_Common_Vector3_get_Unit((&__local_10));
	__local_11 = ((__local_9.Z) > 0);
	if (!__local_11) {
		goto IL_01d9;
	}
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__local_12), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(__local_5, camera), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(__local_6, camera), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(__local_7, camera));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_get_Unit((&__local_0));
	__local_13 = Dntc_Samples_Octahedron_Common_Vector3_Dot((&__local_10), __local_9);
	__local_20 = (__local_13 < 0);
	if (!__local_20) {
		goto IL_017a;
	}
	__local_13 = 0;

IL_017a:
	__local_14 = ((uint8_t)(__local_13 * 255));
	__local_15 = Dntc_Samples_Octahedron_Common_Renderer_Rgb888To565(__local_14, __local_14, __local_14);
	__local_16 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((__local_12.V1), camera);
	__local_17 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((__local_12.V2), camera);
	__local_18 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((__local_12.V3), camera);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__local_19), __local_16, __local_17, __local_18);
	Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle(__local_19, pixels, camera, __local_15);

IL_01d9:
	__local_3 = (__local_3 + 1);

IL_01de:
	__local_21 = (__local_3 < Dntc_Samples_Octahedron_Common_PyramidShape_get_TriangleCount((&shape)));
	if (__local_21) {
		goto IL_0053;
	}
	return;
}

Dntc_Samples_Octahedron_Common_Triangle Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle(Dntc_Samples_Octahedron_Common_CubeShape *__this, int32_t index) {
	int32_t __local_0 = {0};
	int32_t __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_2 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_3 = {0};
	__local_1 = index;
	__local_0 = __local_1;
	switch(__local_0) {
		case 0: goto IL_0040;
		case 1: goto IL_0087;
		case 2: goto IL_00ce;
		case 3: goto IL_0115;
		case 4: goto IL_015c;
		case 5: goto IL_01a3;
		case 6: goto IL_01ea;
		case 7: goto IL_0231;
		case 8: goto IL_0278;
		case 9: goto IL_02bf;
		case 10: goto IL_0306;
		case 11: goto IL_034a;
	}

	goto IL_038e;

IL_0040:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_004f = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_004f), 0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0063 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0063), -0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0077 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0077), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_007c = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_007c), __temp_004f, __temp_0063, __temp_0077);
	__local_2 = __temp_007c;
	goto IL_039a;

IL_0087:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0096 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0096), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00aa = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00aa), -0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00be = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00be), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_00c3 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_00c3), __temp_0096, __temp_00aa, __temp_00be);
	__local_2 = __temp_00c3;
	goto IL_039a;

IL_00ce:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00dd = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00dd), 0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00f1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_00f1), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0105 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0105), 0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_010a = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_010a), __temp_00dd, __temp_00f1, __temp_0105);
	__local_2 = __temp_010a;
	goto IL_039a;

IL_0115:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0124 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0124), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0138 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0138), 0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_014c = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_014c), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_0151 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0151), __temp_0124, __temp_0138, __temp_014c);
	__local_2 = __temp_0151;
	goto IL_039a;

IL_015c:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_016b = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_016b), 0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_017f = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_017f), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0193 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0193), -0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_0198 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0198), __temp_016b, __temp_017f, __temp_0193);
	__local_2 = __temp_0198;
	goto IL_039a;

IL_01a3:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01b2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01b2), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01c6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01c6), -0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01da = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01da), -0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_01df = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_01df), __temp_01b2, __temp_01c6, __temp_01da);
	__local_2 = __temp_01df;
	goto IL_039a;

IL_01ea:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01f9 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_01f9), 0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_020d = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_020d), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0221 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0221), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_0226 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0226), __temp_01f9, __temp_020d, __temp_0221);
	__local_2 = __temp_0226;
	goto IL_039a;

IL_0231:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0240 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0240), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0254 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0254), -0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0268 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0268), 0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_026d = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_026d), __temp_0240, __temp_0254, __temp_0268);
	__local_2 = __temp_026d;
	goto IL_039a;

IL_0278:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0287 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0287), -0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_029b = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_029b), -0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_02af = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_02af), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_02b4 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_02b4), __temp_0287, __temp_029b, __temp_02af);
	__local_2 = __temp_02b4;
	goto IL_039a;

IL_02bf:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_02ce = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_02ce), -0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_02e2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_02e2), -0.5, -0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_02f6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_02f6), -0.5, -0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_02fb = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_02fb), __temp_02ce, __temp_02e2, __temp_02f6);
	__local_2 = __temp_02fb;
	goto IL_039a;

IL_0306:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0315 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0315), -0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0329 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0329), 0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_033d = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_033d), 0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_0342 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0342), __temp_0315, __temp_0329, __temp_033d);
	__local_2 = __temp_0342;
	goto IL_039a;

IL_034a:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0359 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0359), 0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_036d = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_036d), -0.5, 0.5, -0.5);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0381 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__temp_0381), -0.5, 0.5, 0.5);
	Dntc_Samples_Octahedron_Common_Triangle __temp_0386 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__temp_0386), __temp_0359, __temp_036d, __temp_0381);
	__local_2 = __temp_0386;
	goto IL_039a;

IL_038e:
	(*(&__local_3)) = ((Dntc_Samples_Octahedron_Common_Triangle){0});
	__local_2 = __local_3;
	goto IL_039a;

IL_039a:
	return __local_2;
}

int32_t Dntc_Samples_Octahedron_Common_CubeShape_get_TriangleCount(Dntc_Samples_Octahedron_Common_CubeShape *__this) {
	return 12;
}

void Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_CubeShape(SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, float secondsPassed, Dntc_Samples_Octahedron_Common_CubeShape shape) {
	Dntc_Samples_Octahedron_Common_Vector3 __local_0 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_1 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_2 = {0};
	int32_t __local_3 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_4 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_5 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_7 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_8 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_9 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_10 = {0};
	bool __local_11 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_12 = {0};
	float __local_13 = {0};
	uint8_t __local_14 = {0};
	uint16_t __local_15 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_16 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_17 = {0};
	Dntc_Samples_Octahedron_Common_Vector3 __local_18 = {0};
	Dntc_Samples_Octahedron_Common_Triangle __local_19 = {0};
	bool __local_20 = {0};
	bool __local_21 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__local_0), 1, 0, 3);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__local_1), 0, 100, 120);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__local_2), ((__local_1.X) * secondsPassed), ((__local_1.Y) * secondsPassed), ((__local_1.Z) * secondsPassed));
	__local_3 = 0;
	goto IL_01de;

IL_0053:
	__local_4 = Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle((&shape), __local_3);
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&__local_4)->V1)), (__local_2.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (__local_2.Y));
	__local_5 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (__local_2.X));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&__local_4)->V2)), (__local_2.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (__local_2.Y));
	__local_6 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (__local_2.X));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ((&((&__local_4)->V3)), (__local_2.Z));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnY((&__local_10), (__local_2.Y));
	__local_7 = Dntc_Samples_Octahedron_Common_Vector3_RotateOnX((&__local_10), (__local_2.X));
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__local_8), __local_5, __local_6, __local_7);
	__local_10 = Dntc_Samples_Octahedron_Common_Triangle_get_Normal((&__local_8));
	__local_9 = Dntc_Samples_Octahedron_Common_Vector3_get_Unit((&__local_10));
	__local_11 = ((__local_9.Z) > 0);
	if (!__local_11) {
		goto IL_01d9;
	}
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__local_12), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(__local_5, camera), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(__local_6, camera), Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(__local_7, camera));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_get_Unit((&__local_0));
	__local_13 = Dntc_Samples_Octahedron_Common_Vector3_Dot((&__local_10), __local_9);
	__local_20 = (__local_13 < 0);
	if (!__local_20) {
		goto IL_017a;
	}
	__local_13 = 0;

IL_017a:
	__local_14 = ((uint8_t)(__local_13 * 255));
	__local_15 = Dntc_Samples_Octahedron_Common_Renderer_Rgb888To565(__local_14, __local_14, __local_14);
	__local_16 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((__local_12.V1), camera);
	__local_17 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((__local_12.V2), camera);
	__local_18 = Dntc_Samples_Octahedron_Common_Renderer_ToScreen((__local_12.V3), camera);
	Dntc_Samples_Octahedron_Common_Triangle__ctor((&__local_19), __local_16, __local_17, __local_18);
	Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle(__local_19, pixels, camera, __local_15);

IL_01d9:
	__local_3 = (__local_3 + 1);

IL_01de:
	__local_21 = (__local_3 < Dntc_Samples_Octahedron_Common_CubeShape_get_TriangleCount((&shape)));
	if (__local_21) {
		goto IL_0053;
	}
	return;
}

void Dntc_Samples_Octahedron_Common_Renderer_Render(SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, float secondsPassed) {
	int32_t __local_0 = {0};
	bool __local_1 = {0};
	Dntc_Samples_Octahedron_Common_OctahedronShape __local_2 = {0};
	bool __local_3 = {0};
	Dntc_Samples_Octahedron_Common_PyramidShape __local_4 = {0};
	Dntc_Samples_Octahedron_Common_CubeShape __local_5 = {0};
	__local_0 = ((int32_t)(((double)secondsPassed) / 1.5));
	__local_1 = ((__local_0 % 3) == 0);
	if (!__local_1) {
		goto IL_002f;
	}
	(*(&__local_2)) = ((Dntc_Samples_Octahedron_Common_OctahedronShape){0});
	Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_OctahedronShape(pixels, camera, secondsPassed, __local_2);
	goto IL_0065;

IL_002f:
	__local_3 = ((__local_0 % 3) == 1);
	if (!__local_3) {
		goto IL_0050;
	}
	(*(&__local_4)) = ((Dntc_Samples_Octahedron_Common_PyramidShape){0});
	Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_PyramidShape(pixels, camera, secondsPassed, __local_4);
	goto IL_0065;

IL_0050:
	(*(&__local_5)) = ((Dntc_Samples_Octahedron_Common_CubeShape){0});
	Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_CubeShape(pixels, camera, secondsPassed, __local_5);

IL_0065:
	return;
}

