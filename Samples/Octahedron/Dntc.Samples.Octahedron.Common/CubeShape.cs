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
                    new Vector3(1, 1, 1),
                    new Vector3(-1, 1, 1),
                    new Vector3(1, -1, 1));
            
            case 1:
                return new Triangle(
                    new Vector3(1, -1, 1),
                    new Vector3(-1, 1, 1),
                    new Vector3(-1, -1, 1));
           
            // Right
            case 2:
                return new Triangle(
                    new Vector3(1, 1, 1),
                    new Vector3(1, -1, -1),
                    new Vector3(1, 1, -1));
            
            case 3:
                return new Triangle(
                    new Vector3(1, -1, -1),
                    new Vector3(1, 1, 1),
                    new Vector3(1, -1, 1));
           
            // Back
            case 4:
                return new Triangle(
                    new Vector3(1, 1, -1),
                    new Vector3(1, -1, -1),
                    new Vector3(-1, 1, -1));
            
            case 5:
                return new Triangle(
                    new Vector3(1, -1, -1),
                    new Vector3(-1, -1, -1),
                    new Vector3(-1, 1, -1));
            
            // Bottom
            case 6:
                return new Triangle(
                    new Vector3(1, -1, 1),
                    new Vector3(-1, -1, 1),
                    new Vector3(1, -1, -1));
            
            case 7:
                return new Triangle(
                    new Vector3(-1, -1, 1),
                    new Vector3(-1, -1, -1),
                    new Vector3(1, -1, -1));
            
            // Left
            case 8:
                return new Triangle(
                    new Vector3(-1, 1, 1),
                    new Vector3(-1, 1, -1),
                    new Vector3(-1, -1, 1));
            
            case 9:
                return new Triangle(
                    new Vector3(-1, 1, -1),
                    new Vector3(-1, -1, -1),
                    new Vector3(-1, -1, 1));
            
            // Top
            case 10:
                return new Triangle(
                    new Vector3(-1, 1, 1),
                    new Vector3(1, 1, 1),
                    new Vector3(1, 1, -1));
            
            case 11:
                return new Triangle(
                    new Vector3(1, 1, -1),
                    new Vector3(-1, 1, -1),
                    new Vector3(-1, 1, 1));
        }

        return new Triangle();
    }
}