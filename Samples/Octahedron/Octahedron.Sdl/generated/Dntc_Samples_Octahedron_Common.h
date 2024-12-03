#ifndef DNTC_SAMPLES_OCTAHEDRON_COMMON_H_H
#define DNTC_SAMPLES_OCTAHEDRON_COMMON_H_H


#include <math.h>
#include <stdbool.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>
#include "dotnet_arrays.h"
#include "dotnet_math.h"

typedef struct {
	float X;
	float Y;
	float Z;
} Dntc_Samples_Octahedron_Common_Vector3;

typedef struct {
	Dntc_Samples_Octahedron_Common_Vector3 Right;
	Dntc_Samples_Octahedron_Common_Vector3 Up;
	int32_t PixelHeight;
	int32_t PixelWidth;
} Dntc_Samples_Octahedron_Common_Camera;

typedef struct {
	char __dummy; // Placeholder for empty type
} Dntc_Samples_Octahedron_Common_OctahedronShape;

typedef struct {
	char __dummy; // Placeholder for empty type
} Dntc_Samples_Octahedron_Common_PyramidShape;

typedef struct {
	char __dummy; // Placeholder for empty type
} Dntc_Samples_Octahedron_Common_CubeShape;

typedef struct {
	Dntc_Samples_Octahedron_Common_Vector3 V1;
	Dntc_Samples_Octahedron_Common_Vector3 V2;
	Dntc_Samples_Octahedron_Common_Vector3 V3;
} Dntc_Samples_Octahedron_Common_Triangle;

typedef struct {
	int32_t X;
	int32_t Y;
} Dntc_Samples_Octahedron_Common_Point;

typedef struct {
	Dntc_Samples_Octahedron_Common_Point First;
	Dntc_Samples_Octahedron_Common_Point Second;
	float DeltaX;
	float DeltaY;
	float Slope;
} Dntc_Samples_Octahedron_Common_PointPair;



void Dntc_Samples_Octahedron_Common_Vector3__ctor(Dntc_Samples_Octahedron_Common_Vector3 *__this, float x, float y, float z);
Dntc_Samples_Octahedron_Common_Camera Dntc_Samples_Octahedron_Common_Camera_Default();
void Dntc_Samples_Octahedron_Common_Triangle__ctor(Dntc_Samples_Octahedron_Common_Triangle *__this, Dntc_Samples_Octahedron_Common_Vector3 v1, Dntc_Samples_Octahedron_Common_Vector3 v2, Dntc_Samples_Octahedron_Common_Vector3 v3);
Dntc_Samples_Octahedron_Common_Triangle Dntc_Samples_Octahedron_Common_OctahedronShape_GetTriangle(Dntc_Samples_Octahedron_Common_OctahedronShape *__this, int32_t index);
Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_RotateOnZ(Dntc_Samples_Octahedron_Common_Vector3 *__this, float degrees);
Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_RotateOnY(Dntc_Samples_Octahedron_Common_Vector3 *__this, float degrees);
Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_RotateOnX(Dntc_Samples_Octahedron_Common_Vector3 *__this, float degrees);
Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_op_Subtraction(Dntc_Samples_Octahedron_Common_Vector3 first, Dntc_Samples_Octahedron_Common_Vector3 second);
Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_Cross(Dntc_Samples_Octahedron_Common_Vector3 *__this, Dntc_Samples_Octahedron_Common_Vector3 other);
Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Triangle_get_Normal(Dntc_Samples_Octahedron_Common_Triangle *__this);
float Dntc_Samples_Octahedron_Common_Vector3_get_Length(Dntc_Samples_Octahedron_Common_Vector3 *__this);
Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_op_Multiply(Dntc_Samples_Octahedron_Common_Vector3 vec, float scalar);
Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Vector3_get_Unit(Dntc_Samples_Octahedron_Common_Vector3 *__this);
float Dntc_Samples_Octahedron_Common_Vector3_Dot(Dntc_Samples_Octahedron_Common_Vector3 *__this, Dntc_Samples_Octahedron_Common_Vector3 other);
Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Renderer_ProjectTo2d(Dntc_Samples_Octahedron_Common_Vector3 vector, Dntc_Samples_Octahedron_Common_Camera camera);
uint16_t Dntc_Samples_Octahedron_Common_Renderer_Rgb888To565(uint8_t red, uint8_t green, uint8_t blue);
Dntc_Samples_Octahedron_Common_Vector3 Dntc_Samples_Octahedron_Common_Renderer_ToScreen(Dntc_Samples_Octahedron_Common_Vector3 vector, Dntc_Samples_Octahedron_Common_Camera camera);
Dntc_Samples_Octahedron_Common_Triangle Dntc_Samples_Octahedron_Common_Triangle_SortPoints(Dntc_Samples_Octahedron_Common_Triangle *__this);
void Dntc_Samples_Octahedron_Common_Point__ctor(Dntc_Samples_Octahedron_Common_Point *__this, Dntc_Samples_Octahedron_Common_Vector3 vector);
void Dntc_Samples_Octahedron_Common_PointPair__ctor(Dntc_Samples_Octahedron_Common_PointPair *__this, Dntc_Samples_Octahedron_Common_Vector3 v1, Dntc_Samples_Octahedron_Common_Vector3 v2);
void Dntc_Samples_Octahedron_Common_Renderer_RenderTriangle(Dntc_Samples_Octahedron_Common_Triangle triangle, SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, uint16_t color);
int32_t Dntc_Samples_Octahedron_Common_OctahedronShape_get_TriangleCount(Dntc_Samples_Octahedron_Common_OctahedronShape *__this);
void Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_OctahedronShape(SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, float secondsPassed, Dntc_Samples_Octahedron_Common_OctahedronShape shape);
Dntc_Samples_Octahedron_Common_Triangle Dntc_Samples_Octahedron_Common_PyramidShape_GetTriangle(Dntc_Samples_Octahedron_Common_PyramidShape *__this, int32_t index);
int32_t Dntc_Samples_Octahedron_Common_PyramidShape_get_TriangleCount(Dntc_Samples_Octahedron_Common_PyramidShape *__this);
void Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_PyramidShape(SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, float secondsPassed, Dntc_Samples_Octahedron_Common_PyramidShape shape);
Dntc_Samples_Octahedron_Common_Triangle Dntc_Samples_Octahedron_Common_CubeShape_GetTriangle(Dntc_Samples_Octahedron_Common_CubeShape *__this, int32_t index);
int32_t Dntc_Samples_Octahedron_Common_CubeShape_get_TriangleCount(Dntc_Samples_Octahedron_Common_CubeShape *__this);
void Dntc_Samples_Octahedron_Common_Renderer_PerformRender_Dntc_Samples_Octahedron_Common_CubeShape(SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, float secondsPassed, Dntc_Samples_Octahedron_Common_CubeShape shape);
void Dntc_Samples_Octahedron_Common_Renderer_Render(SystemUInt16Array pixels, Dntc_Samples_Octahedron_Common_Camera camera, float secondsPassed);

#endif // DNTC_SAMPLES_OCTAHEDRON_COMMON_H_H
