namespace Dntc.Samples.Octahedron.Common;

public struct Triangle
{
    public readonly Vector3 V1, V2, V3;

    public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        (V1, V2, V3) = (v1, v2, v3);
    }

    public Vector3 Normal => (V2 - V1).Cross(V3 - V1);

    public Triangle SortPoints()
    {
        var v1 = V1;
        var v2 = V2;
        var v3 = V3;
        Vector3 temp;
        
        // Sort the points from top to bottom
        if (v1.Y > v2.Y)
        {
            temp = v1;
            v1 = v2;
            v2 = temp;
        }

        if (v3.Y < v1.Y)
        {
            temp = v3;
            v3 = v2;
            v2 = v1;
            v1 = temp;
        } else if (v3.Y < v2.Y)
        {
            temp = v3;
            v3 = v2;
            v2 = temp;
        }

        return new Triangle(v1, v2, v3);
    }
    
    
}