namespace Dntc.Samples.Octahedron.Common;

public struct PyramidShape : IShape
{
    public int TriangleCount => 6;
    
    public Triangle GetTriangle(int index)
    {
        switch (index)
        {
            // bottom
            case 0:
                return new Triangle(
                    new Vector3(1, -1, 1),
                    new Vector3(-1, -1, 1),
                    new Vector3(1, -1, -1));

            case 1:
                return new Triangle(
                    new Vector3(-1, -1, 1),
                    new Vector3(-1, -1, -1),
                    new Vector3(1, -1, -1));
            
            // Front
            case 2:
                return new Triangle(
                    new Vector3(0, 1, 0),
                    new Vector3(-1, -1, 1),
                    new Vector3(1, -1, 1));
            
            // Right
            case 3:
                return new Triangle(
                    new Vector3(0, 1, 0),
                    new Vector3(1, -1, 1),
                    new Vector3(1, -1, -1));
            
            // Back
            case 4:
                return new Triangle(
                    new Vector3(0, 1, 0),
                    new Vector3(1, -1, -1),
                    new Vector3(-1, -1, -1));
            
            // Left
            case 5:
                return new Triangle(
                    new Vector3(0, 1, 0),
                    new Vector3(-1, -1, -1),
                    new Vector3(-1, -1, 1));
        }

        return new Triangle();
    }
}