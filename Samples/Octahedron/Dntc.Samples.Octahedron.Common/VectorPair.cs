namespace Dntc.Samples.Octahedron.Common;

public struct VectorPair
{
    public Vector3 V1, V2;
    public float DeltaX, DeltaY;
    public float Slope;

    public VectorPair(Vector3 v1, Vector3 v2)
    {
        V1 = v1;
        V2 = v2;
        DeltaX = v2.X - v1.X;
        DeltaY = v2.Y - v2.Y;
        Slope = DeltaX / DeltaY;
    }
}