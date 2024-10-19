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
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f));

            case 1:
                return new Triangle(
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f));
            
            // Front
            case 2:
                return new Triangle(
                    new Vector3(0, 1, 0),
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f));
            
            // Right
            case 3:
                return new Triangle(
                    new Vector3(0, 1, 0),
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f));
            
            // Back
            case 4:
                return new Triangle(
                    new Vector3(0, 1, 0),
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f));
            
            // Left
            case 5:
                return new Triangle(
                    new Vector3(0, 1, 0),
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f));
        }

        return new Triangle();
    }
}