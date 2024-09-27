namespace Dntc.Samples.Octahedron.Common;

public struct PointPair
{
    public Point First, Second;
    public float DeltaX, DeltaY;
    public float Slope;

    public PointPair(Vector3 v1, Vector3 v2)
    {
        First = new Point(v1);
        Second = new Point(v2);
        DeltaX = Second.X - (float)First.X;
        DeltaY = Second.Y - (float)First.Y;
        Slope = DeltaX / DeltaY;
    }
}