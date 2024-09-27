namespace Dntc.Samples.Octahedron.Common;

public struct Point
{
    public int X;
    public int Y;

    public Point(Vector3 vector)
    {
        X = (int)vector.X;
        Y = (int)vector.Y;
    }
}