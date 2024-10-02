namespace Dntc.Samples.Octahedron.Common;

public static class OctahedronRenderer
{
    public static void Render(ushort[] pixels, Camera camera, float secondsPassed)
    {
        var light = new Vector3(1, 0, 3);
        var rotationsDegreesPerSecond = new Vector3(0, 100, 120);
        // var rotationsDegreesPerSecond = new Vector3(0, 0, 0);

        var rotation = new Vector3(
            rotationsDegreesPerSecond.X * secondsPassed,
            rotationsDegreesPerSecond.Y * secondsPassed,
            rotationsDegreesPerSecond.Z * secondsPassed);

        for (var x = 0; x < 8; x++)
        {
            var triangle = GetTriangle(x);
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

            var rotatedTriangle = new Triangle(rotatedV1, rotatedV2, rotatedV3);
            var normal = rotatedTriangle.Normal.Unit;
            if (normal.Z > 0)
            {
                var projectedTriangle = new Triangle(
                    ProjectTo2d(rotatedV1, camera),
                    ProjectTo2d(rotatedV2, camera),
                    ProjectTo2d(rotatedV3, camera));
                
                var alignment = light.Unit.Dot(normal);
                if (alignment < 0) alignment = 0;

                var colorValue = (byte)(alignment * 255);
                var color = Rgb888To565(colorValue, colorValue, colorValue);

                // draw
                var p1 = ToScreen(projectedTriangle.V1, camera);
                var p2 = ToScreen(projectedTriangle.V2, camera);
                var p3 = ToScreen(projectedTriangle.V3, camera);

                var triangleToDraw = new Triangle(p1, p2, p3);
                RenderTriangle(triangleToDraw, pixels, camera, color);
            }
        }
    }

    private static Triangle GetTriangle(int index)
    {
        switch (index)
        {
            case 0:
                return new Triangle(
                    new Vector3(1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, 1));

            case 1:
                return new Triangle(
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, -1),
                    new Vector3(0, 1, 0));

            case 2:
                return new Triangle(
                    new Vector3(1, 0, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(0, -1, 0));

            case 3:
                return new Triangle(
                    new Vector3(1, 0, 0),
                    new Vector3(0, -1, 0),
                    new Vector3(0, 0, -1));

            case 4:
                return new Triangle(
                    new Vector3(-1, 0, 0),
                    new Vector3(0, 0, 1),
                    new Vector3(0, 1, 0));

            case 5:
                return new Triangle(
                    new Vector3(-1, 0, 0),
                    new Vector3(0, 1, 0),
                    new Vector3(0, 0, -1));

            case 6:
                return new Triangle(
                    new Vector3(-1, 0, 0),
                    new Vector3(0, -1, 0),
                    new Vector3(0, 0, 1));

            case 7:
                return new Triangle(
                    new Vector3(-1, 0, 0),
                    new Vector3(0, 0, -1),
                    new Vector3(0, -1, 0));

            // Should throw an error 
            default: return new Triangle();
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

    private static void RenderTriangle(Triangle triangle, ushort[] pixels, Camera camera, ushort color)
    {
        triangle = triangle.SortPoints();
        var top = triangle.V1;
        var mid = triangle.V2;
        var bottom = triangle.V3;

        var topMidPair = new PointPair(top, mid);
        var topBottomPair = new PointPair(top, bottom);
        var midBottomPair = new PointPair(mid, bottom);

        var shortPair = topMidPair;
        var longPair = topBottomPair;
        var shortX = top.X;
        var longX = top.X;

        for (var y = (int)top.Y; y <= bottom.Y; y++)
        {
            if (y >= camera.PixelHeight)
            {
                break;
            }

            if (y == mid.Y)
            {
                // We reached the midpoint, swap to the next pair
                shortPair = midBottomPair;
                shortX = shortPair.First.X;
            }

            // Draw the row
            var startCol = Math.Min(shortX, longX);
            if (startCol < camera.PixelWidth)
            {
                // var diff = longX > shortX
                //     ? (longX - shortX)
                //     : (shortX - longX);
                
                var diff = 0f;
                if (longX > shortX) {
                    diff = longX - shortX;
                } else {
                    diff = shortX - longX;
                }

                var endCol = Math.Min(startCol + diff, camera.PixelWidth - 1);
                diff = endCol - startCol;

                var index = (int)(y * camera.PixelWidth + startCol);
                for (var x = 0; x <= diff; x++)
                {
                    pixels[index] = color;
                    index++;
                }
            }

            // Adjust the x position
            shortX += shortPair.Slope;
            longX += longPair.Slope;
        }
    }
}