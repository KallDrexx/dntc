#include "Dntc_Samples_Octahedron_Common.h"
#include <stdint.h>
#include <math.h>
#include "dotnet_arrays.h"
#include "dotnet_math.h"

void Dntc_Samples_Octahedron_Common_Vector3__ctor(Dntc_Samples_Octahedron_Common_Vector3 *__this, float x, float y, float z) {
	float __local_0;
	float __local_1;
	float __local_2;

	__local_0 = x;
	__local_1 = y;
	__local_2 = z;
	(__this)->X = __local_0;
	(__this)->Y = __local_1;
	(__this)->Z = __local_2;
	 return;
}

Dntc_Samples_Octahedron_Common_Camera Dntc_Samples_Octahedron_Common_Camera_Default() {
	Dntc_Samples_Octahedron_Common_Camera __local_0;

	*(&__local_0) = (Dntc_Samples_Octahedron_Common_Camera){0};
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0019 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0019, 1, 0, 0);
	(&__local_0)->Right = __temp_0019;
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0034 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0034, 0, 1, 0);
	(&__local_0)->Up = __temp_0034;
	 return __local_0;
}

void Dntc_Samples_Octahedron_Common_Triangle__ctor(Dntc_Samples_Octahedron_Common_Triangle *__this, Dntc_Samples_Octahedron_Common_Vector3 v1, Dntc_Samples_Octahedron_Common_Vector3 v2, Dntc_Samples_Octahedron_Common_Vector3 v3) {
	Dntc_Samples_Octahedron_Common_Vector3 __local_0;
	Dntc_Samples_Octahedron_Common_Vector3 __local_1;
	Dntc_Samples_Octahedron_Common_Vector3 __local_2;

	__local_0 = v1;
	__local_1 = v2;
	__local_2 = v3;
	(__this)->V1 = __local_0;
	(__this)->V2 = __local_1;
	(__this)->V3 = __local_2;
	 return;
}

Dntc_Samples_Octahedron_Common_Triangle Dntc_Samples_Octahedron_Common_OctahedronRenderer_GetTriangle(int32_t index) {
	Dntc_Samples_Octahedron_Common_Triangle __local_0;

	switch (index) {
		case 0:
			goto IL_002B;

		case 1:
			goto IL_006D;

		case 2:
			goto IL_00AF;

		case 3:
			goto IL_00F1;

		case 4:
			goto IL_0133;

		case 5:
			goto IL_0175;

		case 6:
			goto IL_01B7;

		case 7:
			goto IL_01F9;

	}
	goto IL_023B;

IL_002B:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_003A = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_003A, 1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_004E = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_004E, 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0062 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0062, 0, 0, 1);
	Dntc_Samples_Octahedron_Common_Triangle __temp_0067 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor(&__temp_0067, __temp_003A, __temp_004E, __temp_0062);
	 return __temp_0067;

IL_006D:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_007C = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_007C, 1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0090 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0090, 0, 0, -1);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00A4 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_00A4, 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Triangle __temp_00A9 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor(&__temp_00A9, __temp_007C, __temp_0090, __temp_00A4);
	 return __temp_00A9;

IL_00AF:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00BE = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_00BE, 1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00D2 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_00D2, 0, 0, 1);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_00E6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_00E6, 0, -1, 0);
	Dntc_Samples_Octahedron_Common_Triangle __temp_00EB = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor(&__temp_00EB, __temp_00BE, __temp_00D2, __temp_00E6);
	 return __temp_00EB;

IL_00F1:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0100 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0100, 1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0114 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0114, 0, -1, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0128 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0128, 0, 0, -1);
	Dntc_Samples_Octahedron_Common_Triangle __temp_012D = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor(&__temp_012D, __temp_0100, __temp_0114, __temp_0128);
	 return __temp_012D;

IL_0133:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0142 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0142, -1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0156 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0156, 0, 0, 1);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_016A = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_016A, 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Triangle __temp_016F = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor(&__temp_016F, __temp_0142, __temp_0156, __temp_016A);
	 return __temp_016F;

IL_0175:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0184 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0184, -1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0198 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0198, 0, 1, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01AC = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_01AC, 0, 0, -1);
	Dntc_Samples_Octahedron_Common_Triangle __temp_01B1 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor(&__temp_01B1, __temp_0184, __temp_0198, __temp_01AC);
	 return __temp_01B1;

IL_01B7:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01C6 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_01C6, -1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01DA = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_01DA, 0, -1, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_01EE = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_01EE, 0, 0, 1);
	Dntc_Samples_Octahedron_Common_Triangle __temp_01F3 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor(&__temp_01F3, __temp_01C6, __temp_01DA, __temp_01EE);
	 return __temp_01F3;

IL_01F9:
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0208 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0208, -1, 0, 0);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_021C = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_021C, 0, 0, -1);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0230 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0230, 0, -1, 0);
	Dntc_Samples_Octahedron_Common_Triangle __temp_0235 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor(&__temp_0235, __temp_0208, __temp_021C, __temp_0230);
	 return __temp_0235;

IL_023B:
	*(&__local_0) = (Dntc_Samples_Octahedron_Common_Triangle){0};
	 return __local_0;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ(Dntc_Samples_Octahedron_Common_Vector3 *__this, float degrees) {
	double __local_0;
	float __local_1;
	float __local_2;
	float __local_3;

	__local_0 = ((((double)degrees) * 3.141592653589793) / 180);
	__local_1 = ((float)sqrt(((double)((((__this)->X) * ((__this)->X)) + (((__this)->Y) * ((__this)->Y))))));
	__local_2 = (((float)cos((atan2(((double)((__this)->Y)), ((double)((__this)->X))) + __local_0))) * __local_1);
	__local_3 = (((float)sin((atan2(((double)((__this)->Y)), ((double)((__this)->X))) + __local_0))) * __local_1);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_006A = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_006A, __local_2, __local_3, ((__this)->Z));
	 return __temp_006A;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_RotateOnY(Dntc_Samples_Octahedron_Common_Vector3 *__this, float degrees) {
	double __local_0;
	float __local_1;
	float __local_2;
	float __local_3;

	__local_0 = ((((double)degrees) * 3.141592653589793) / 180);
	__local_1 = ((float)sqrt(((double)((((__this)->X) * ((__this)->X)) + (((__this)->Z) * ((__this)->Z))))));
	__local_2 = (((float)cos((atan2(((double)((__this)->Z)), ((double)((__this)->X))) + __local_0))) * __local_1);
	__local_3 = (((float)sin((atan2(((double)((__this)->Z)), ((double)((__this)->X))) + __local_0))) * __local_1);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_006A = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_006A, __local_2, ((__this)->Y), __local_3);
	 return __temp_006A;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_RotateOnX(Dntc_Samples_Octahedron_Common_Vector3 *__this, float degrees) {
	double __local_0;
	float __local_1;
	float __local_2;
	float __local_3;

	__local_0 = ((((double)degrees) * 3.141592653589793) / 180);
	__local_1 = ((float)sqrt(((double)((((__this)->Z) * ((__this)->Z)) + (((__this)->Y) * ((__this)->Y))))));
	__local_2 = (((float)cos((atan2(((double)((__this)->Y)), ((double)((__this)->Z))) + __local_0))) * __local_1);
	__local_3 = (((float)sin((atan2(((double)((__this)->Y)), ((double)((__this)->Z))) + __local_0))) * __local_1);
	Dntc_Samples_Octahedron_Common_Vector3 __temp_006A = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_006A, ((__this)->X), __local_3, __local_2);
	 return __temp_006A;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_op_Subtraction(Dntc_Samples_Octahedron_Common_Vector3 first, Dntc_Samples_Octahedron_Common_Vector3 second) {

	Dntc_Samples_Octahedron_Common_Vector3 __temp_0027 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0027, ((first.X) - (second.X)), ((first.Y) - (second.Y)), ((first.Z) - (second.Z)));
	 return __temp_0027;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_Cross(Dntc_Samples_Octahedron_Common_Vector3 *__this, Dntc_Samples_Octahedron_Common_Vector3 other) {
	float __local_0;
	float __local_1;

	__local_0 = ((((__this)->Z) * (other.X)) - (((__this)->X) * (other.Z)));
	__local_1 = ((((__this)->X) * (other.Y)) - (((__this)->Y) * (other.X)));
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0055 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0055, ((((__this)->Y) * (other.Z)) - (((__this)->Z) * (other.Y))), __local_0, __local_1);
	 return __temp_0055;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Triangle_get_Normal(Dntc_Samples_Octahedron_Common_Triangle *__this) {
	Dntc_Samples_Octahedron_Common_Vector3 __local_0;

	__local_0 = Dntc_Samples_Octahedron_Common_Vector3_op_Subtraction(((__this)->V2), ((__this)->V1));
	 return Dntc_Samples_Octahedron_Common_Vector3_Cross((&__local_0), Dntc_Samples_Octahedron_Common_Vector3_op_Subtraction(((__this)->V3), ((__this)->V1)));
}

float Dntc_Samples_Octahedron_Common_Vector3_get_Length(Dntc_Samples_Octahedron_Common_Vector3 *__this) {

	 return ((float)sqrt(((double)(((((__this)->X) * ((__this)->X)) + (((__this)->Y) * ((__this)->Y))) + (((__this)->Z) * ((__this)->Z))))));
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_op_Multiply(Dntc_Samples_Octahedron_Common_Vector3 vec, float scalar) {

	Dntc_Samples_Octahedron_Common_Vector3 __temp_0018 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0018, ((vec.X) * scalar), ((vec.Y) * scalar), ((vec.Z) * scalar));
	 return __temp_0018;
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_get_Unit(Dntc_Samples_Octahedron_Common_Vector3 *__this) {

	 return Dntc_Samples_Octahedron_Common_Vector3_op_Multiply(*(__this), (1 / Dntc_Samples_Octahedron_Common_Vector3_get_Length((__this))));
}

float Dntc_Samples_Octahedron_Common_Vector3_Dot(Dntc_Samples_Octahedron_Common_Vector3 *__this, Dntc_Samples_Octahedron_Common_Vector3 other) {

	 return (((((__this)->X) * (other.X)) + (((__this)->Y) * (other.Y))) + (((__this)->Z) * (other.Z)));
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_OctahedronRenderer_ProjectTo2d(Dntc_Samples_Octahedron_Common_Vector3 vector, Dntc_Samples_Octahedron_Common_Camera camera) {
	float __local_0;

	__local_0 = (Dntc_Samples_Octahedron_Common_Vector3_Dot((&vector), (camera.Up)) / Dntc_Samples_Octahedron_Common_Vector3_get_Length((&((&camera)->Up))));
	Dntc_Samples_Octahedron_Common_Vector3 __temp_003B = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_003B, (Dntc_Samples_Octahedron_Common_Vector3_Dot((&vector), (camera.Right)) / Dntc_Samples_Octahedron_Common_Vector3_get_Length((&((&camera)->Right)))), __local_0, 0);
	 return __temp_003B;
}

uint16_t Dntc_Samples_Octahedron_Common_OctahedronRenderer_Rgb888To565(uint8_t red, uint8_t green, uint8_t blue) {

	red = ((uint8_t)(red / 8));
	green = ((uint8_t)(green / 4));
	blue = ((uint8_t)(blue / 8));
	 return ((uint16_t)(((red << 11) | (green << 5)) | blue));
}

Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_OctahedronRenderer_ToScreen(Dntc_Samples_Octahedron_Common_Vector3 vector, Dntc_Samples_Octahedron_Common_Camera camera) {
	uint16_t __local_0;

	__local_0 = ((uint16_t)(((vector.Y) * 100) + ((float)((camera.PixelHeight) / 2))));
	Dntc_Samples_Octahedron_Common_Vector3 __temp_0037 = {0};
	Dntc_Samples_Octahedron_Common_Vector3__ctor(&__temp_0037, ((float)((uint16_t)(((vector.X) * 100) + ((float)((camera.PixelWidth) / 2))))), ((float)__local_0), 0);
	 return __temp_0037;
}

Dntc_Samples_Octahedron_Common_Triangle Dntc_Samples_Octahedron_Common_Triangle_SortPoints(Dntc_Samples_Octahedron_Common_Triangle *__this) {
	Dntc_Samples_Octahedron_Common_Vector3 __local_0;
	Dntc_Samples_Octahedron_Common_Vector3 __local_1;
	Dntc_Samples_Octahedron_Common_Vector3 __local_2;

	__local_0 = ((__this)->V1);
	__local_1 = ((__this)->V2);
	__local_2 = ((__this)->V3);
	if ((__local_0.Y) <= (__local_1.Y)) {
		goto IL_0027;
	}
	__local_0 = __local_1;
	__local_1 = __local_0;

IL_0027:
	if ((__local_2.Y) >= (__local_0.Y)) {
		goto IL_003D;
	}
	__local_2 = __local_1;
	__local_1 = __local_0;
	__local_0 = __local_2;
	goto IL_004F;

IL_003D:
	if ((__local_2.Y) >= (__local_1.Y)) {
		goto IL_004F;
	}
	__local_2 = __local_1;
	__local_1 = __local_2;

IL_004F:
	Dntc_Samples_Octahedron_Common_Triangle __temp_0052 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor(&__temp_0052, __local_0, __local_1, __local_2);
	 return __temp_0052;
}

void Dntc_Samples_Octahedron_Common_Point__ctor(Dntc_Samples_Octahedron_Common_Point *__this, Dntc_Samples_Octahedron_Common_Vector3 vector) {

	(__this)->X = ((int32_t)(vector.X));
	(__this)->Y = ((int32_t)(vector.Y));
	 return;
}

void Dntc_Samples_Octahedron_Common_PointPair__ctor(Dntc_Samples_Octahedron_Common_PointPair *__this, Dntc_Samples_Octahedron_Common_Vector3 v1, Dntc_Samples_Octahedron_Common_Vector3 v2) {

	Dntc_Samples_Octahedron_Common_Point __temp_0002 = {0};
	Dntc_Samples_Octahedron_Common_Point__ctor(&__temp_0002, v1);
	(__this)->First = __temp_0002;
	Dntc_Samples_Octahedron_Common_Point __temp_000E = {0};
	Dntc_Samples_Octahedron_Common_Point__ctor(&__temp_000E, v2);
	(__this)->Second = __temp_000E;
	(__this)->DeltaX = (((float)((&((__this)->Second))->X)) - ((float)((&((__this)->First))->X)));
	(__this)->DeltaY = (((float)((&((__this)->Second))->Y)) - ((float)((&((__this)->First))->Y)));
	(__this)->Slope = (((__this)->DeltaX) / ((__this)->DeltaY));
	 return;
}

void Dntc_Samples_Octahedron_Common_OctahedronRenderer_RenderTriangle(Dntc_Samples_Octahedron_Common_Triangle triangle, SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, uint16_t color) {
    printf("rendering triangle\n");
	Dntc_Samples_Octahedron_Common_Vector3 __local_0;
	Dntc_Samples_Octahedron_Common_Vector3 __local_1;
	Dntc_Samples_Octahedron_Common_Vector3 __local_2;
	Dntc_Samples_Octahedron_Common_PointPair __local_3;
	Dntc_Samples_Octahedron_Common_PointPair __local_4;
	Dntc_Samples_Octahedron_Common_PointPair __local_5;
	Dntc_Samples_Octahedron_Common_PointPair __local_6;
	float __local_7;
	float __local_8;
	int32_t __local_9;
	float __local_10;
	float __local_11;
	int32_t __local_12;
	int32_t __local_13;

	triangle = Dntc_Samples_Octahedron_Common_Triangle_SortPoints((&triangle));
	__local_0 = (triangle.V1);
	__local_1 = (triangle.V2);
	__local_2 = (triangle.V3);
	Dntc_Samples_Octahedron_Common_PointPair__ctor((&__local_3), __local_0, __local_1);
	Dntc_Samples_Octahedron_Common_PointPair __temp_0029 = {0};
	Dntc_Samples_Octahedron_Common_PointPair__ctor(&__temp_0029, __local_0, __local_2);
	Dntc_Samples_Octahedron_Common_PointPair__ctor((&__local_4), __local_1, __local_2);
	__local_5 = __local_3;
	__local_6 = __temp_0029;
	__local_7 = (__local_0.X);
	__local_8 = (__local_0.X);
	__local_9 = ((int32_t)(__local_0.Y));
	goto IL_011B;

IL_005A:
	if (__local_9 >= (camera.PixelHeight)) {
        printf("a\n");
		goto IL_0129;
	}
	if (((float)__local_9) != (__local_1.Y)) {
		goto IL_0085;
	}
	__local_5 = __local_4;
	__local_7 = ((float)((__local_5.First).X));

IL_0085:
	__local_10 = dn_min_float(__local_7, __local_8);
	if (__local_10 >= ((float)(camera.PixelWidth))) {
		goto IL_00FD;
	}
	__local_11 = 0;
	if (__local_8 <= __local_7) {
		goto IL_00B1;
	}
	__local_11 = (__local_8 - __local_7);
	goto IL_00B8;

IL_00B1:
	__local_11 = (__local_7 - __local_8);

IL_00B8:
	__local_11 = (dn_min_float((__local_10 + __local_11), ((float)((camera.PixelWidth) - 1))) - __local_10);
	__local_12 = ((int32_t)(((float)(__local_9 * (camera.PixelWidth))) + __local_10));
	__local_13 = 0;
	goto IL_00F6;

IL_00E5:
	if (pixels.length <= __local_12) {
		printf("Attempted to write to pixels[%ld], but only %zu items are in the array", __local_12, pixels.length);
		abort();
	}

	pixels.items[__local_12] = color;
    printf("Setting pixel %ld to %d", __local_12, color);
	__local_12 = (__local_12 + 1);
	__local_13 = (__local_13 + 1);

IL_00F6:
	if (((float)__local_13) <= __local_11) {
		goto IL_00E5;
	}

IL_00FD:
	__local_7 = (__local_7 + (__local_5.Slope));
	__local_8 = (__local_8 + (__local_6.Slope));
	__local_9 = (__local_9 + 1);

IL_011B:
	if (((float)__local_9) <= (__local_2.Y)) {
		goto IL_005A;
	}

IL_0129:
	 return;
}

void Dntc_Samples_Octahedron_Common_OctahedronRenderer_Render(SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, float secondsPassed) {
	Dntc_Samples_Octahedron_Common_Vector3 __local_0;
	Dntc_Samples_Octahedron_Common_Vector3 __local_1;
	Dntc_Samples_Octahedron_Common_Vector3 __local_2;
	int32_t __local_3;
	Dntc_Samples_Octahedron_Common_Triangle __local_4;
	Dntc_Samples_Octahedron_Common_Vector3 __local_5;
	Dntc_Samples_Octahedron_Common_Vector3 __local_6;
	Dntc_Samples_Octahedron_Common_Vector3 __local_7;
	Dntc_Samples_Octahedron_Common_Triangle __local_8;
	Dntc_Samples_Octahedron_Common_Vector3 __local_9;
	Dntc_Samples_Octahedron_Common_Vector3 __local_10;
	float __local_11;
	uint16_t __local_12;
	Dntc_Samples_Octahedron_Common_Vector3 __local_13;
	Dntc_Samples_Octahedron_Common_Vector3 __local_14;
	Dntc_Samples_Octahedron_Common_Vector3 __local_15;

	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__local_0), 1, 0, 3);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__local_1), 0, 100, 120);
	Dntc_Samples_Octahedron_Common_Vector3__ctor((&__local_2), ((__local_1.X) * secondsPassed), ((__local_1.Y) * secondsPassed), ((__local_1.Z) * secondsPassed));
	__local_3 = 0;
	goto IL_01B4;

IL_0052:
	__local_4 = Dntc_Samples_Octahedron_Common_OctahedronRenderer_GetTriangle(__local_3);
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
	if ((__local_9.Z) <= 0) {
		goto IL_01B0;
	}
	Dntc_Samples_Octahedron_Common_Triangle __temp_0138 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor(&__temp_0138, Dntc_Samples_Octahedron_Common_OctahedronRenderer_ProjectTo2d(__local_5, camera), Dntc_Samples_Octahedron_Common_OctahedronRenderer_ProjectTo2d(__local_6, camera), Dntc_Samples_Octahedron_Common_OctahedronRenderer_ProjectTo2d(__local_7, camera));
	__local_10 = Dntc_Samples_Octahedron_Common_Vector3_get_Unit((&__local_0));
	__local_11 = Dntc_Samples_Octahedron_Common_Vector3_Dot((&__local_10), __local_9);
	if (__local_11 >= 0) {
		goto IL_0161;
	}
	__local_11 = 0;

IL_0161:
	__local_12 = Dntc_Samples_Octahedron_Common_OctahedronRenderer_Rgb888To565(((uint8_t)(__local_11 * 255)), ((uint8_t)(__local_11 * 255)), ((uint8_t)(__local_11 * 255)));
	__local_13 = Dntc_Samples_Octahedron_Common_OctahedronRenderer_ToScreen((__temp_0138.V1), camera);
	__local_14 = Dntc_Samples_Octahedron_Common_OctahedronRenderer_ToScreen((__temp_0138.V2), camera);
	__local_15 = Dntc_Samples_Octahedron_Common_OctahedronRenderer_ToScreen((__temp_0138.V3), camera);
	Dntc_Samples_Octahedron_Common_Triangle __temp_01A2 = {0};
	Dntc_Samples_Octahedron_Common_Triangle__ctor(&__temp_01A2, __local_13, __local_14, __local_15);
	Dntc_Samples_Octahedron_Common_OctahedronRenderer_RenderTriangle(__temp_01A2, pixels, camera, __local_12);

IL_01B0:
	__local_3 = (__local_3 + 1);

IL_01B4:
	if (__local_3 < 8) {
		goto IL_0052;
	}
	 return;
}

