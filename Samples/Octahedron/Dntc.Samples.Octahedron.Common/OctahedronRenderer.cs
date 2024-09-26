namespace Dntc.Samples.Octahedron.Common;

public static class OctahedronRenderer
{
    public static void Render(Triangle[] triangles, ushort[] pixels, Camera camera, float secondsPassed)
    {
        var light = new Vector3(1, 0, 3);
        var rotationsDegreesPerSecond = new Vector3(0, 100, 120);

        var rotation = new Vector3(
            rotationsDegreesPerSecond.X * secondsPassed,
            rotationsDegreesPerSecond.Y * secondsPassed,
            rotationsDegreesPerSecond.Z * secondsPassed);

        foreach (var triangle in triangles)
        {
            var rotatedV1 = triangle.V1
                .RotateOnZ(rotation.Z)
                .RotateOnY(rotation.Y)
                .RotateOnX(rotation.X);
                
            var rotatedV2 = triangle.V2
                .RotateOnZ(rotation.Z)
                .RotateOnY(rotation.Y)
                .RotateOnX(rotation.X);
            
            var rotatedV3 = triangle.V3
                .RotateOnZ(rotation.Z)
                .RotateOnY(rotation.Y)
                .RotateOnX(rotation.X);

            var projectedTriangle = new Triangle(
                ProjectTo2d(rotatedV1, camera),
                ProjectTo2d(rotatedV2, camera),
                ProjectTo2d(rotatedV3, camera));

            var normal = projectedTriangle.Normal.Unit;
            if (normal.Z > 0)
            {
                var alignment = light.Unit.Dot(normal);
                if (alignment < 0) alignment = 0;

                var colorValue = (byte)(alignment * 255);
                var color = Rgb888To565(colorValue, colorValue, colorValue);
                
                // draw
                var p1 = ToScreen(projectedTriangle.V1, camera);
                var p2 = ToScreen(projectedTriangle.V2, camera);
                var p3 = ToScreen(projectedTriangle.V3, camera);
                
                
            }
        }
    }

    private static Vector3 ProjectTo2d(Vector3 vector, Camera camera)
    {
        var x = vector.Dot(camera.Right) / camera.Right.Length;
        var y = vector.Dot(camera.Up) / camera.Up.Length;

        return new Vector3(x, y, 0);
    }

    private static Vector3 ToScreen(Vector3 vector, Camera camera)
    {
        var x = (ushort)(vector.X * 100 + camera.PixelWidth / 2);
        var y = (ushort)(vector.Y * 100 + camera.PixelHeight / 2);

        return new Vector3(x, y, 0);
    }

    private static ushort Rgb888To565(byte red, byte green, byte blue)
    {
        red /= 8;
        green /= 4;
        blue /= 8;

        return (ushort)((red << 11) | (green << 5) | blue);
    }

    private static void RenderTriangle(Triangle triangle, ushort[] pixels, Camera camera)
    {
        triangle = triangle.SortPoints();
        var topMidPair = new VectorPair(triangle.V1, triangle.V2);
        var topBottomPair = new VectorPair(triangle.V1, triangle.V3);
        var midBottomPair = new VectorPair(triangle.V2, triangle.V3);

        var shortPair = topMidPair;
        var longPair = topBottomPair;

    }
}