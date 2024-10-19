namespace Dntc.Samples.Octahedron.Common;

public struct CubeShape : IShape
{
    public int TriangleCount => 12;
    
    public Triangle GetTriangle(int index)
    {
        switch (index)
        {
            // Front
            case 0:
                return new Triangle(
                    new Vector3(0.5f, 0.5f, 0.5f),
                    new Vector3(-0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f));
            
            case 1:
                return new Triangle(
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(-0.5f, 0.5f, 0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f));
           
            // Right
            case 2:
                return new Triangle(
                    new Vector3(0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f));
            
            case 3:
                return new Triangle(
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f));
           
            // Back
            case 4:
                return new Triangle(
                    new Vector3(0.5f, 0.5f, -0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(-0.5f, 0.5f, -0.5f));
            
            case 5:
                return new Triangle(
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(-0.5f, 0.5f, -0.5f));
            
            // Bottom
            case 6:
                return new Triangle(
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f));
            
            case 7:
                return new Triangle(
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f));
            
            // Left
            case 8:
                return new Triangle(
                    new Vector3(-0.5f, 0.5f, 0.5f),
                    new Vector3(-0.5f, 0.5f, -0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f));
            
            case 9:
                return new Triangle(
                    new Vector3(-0.5f, 0.5f, -0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f));
            
            // Top
            case 10:
                return new Triangle(
                    new Vector3(-0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f));
            
            case 11:
                return new Triangle(
                    new Vector3(0.5f, 0.5f, -0.5f),
                    new Vector3(-0.5f, 0.5f, -0.5f),
                    new Vector3(-0.5f, 0.5f, 0.5f));
        }

        return new Triangle();
    }
}