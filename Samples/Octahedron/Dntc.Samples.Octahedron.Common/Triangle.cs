namespace Dntc.Samples.Octahedron.Common;

public struct Triangle
{
    public readonly Vector3 V1, V2, V3;

    public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        (V1, V2, V3) = (v1, v2, v3);
    }

    public Vector3 Normal => (V2 - V1).Cross(V3 - V1);
}