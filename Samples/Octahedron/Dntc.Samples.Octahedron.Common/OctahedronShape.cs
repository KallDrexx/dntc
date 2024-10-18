namespace Dntc.Samples.Octahedron.Common;

public struct OctahedronShape : IShape
{
    public int TriangleCount => 8;
    
    public Triangle GetTriangle(int index)
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
}