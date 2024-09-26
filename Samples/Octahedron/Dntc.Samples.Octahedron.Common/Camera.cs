namespace Dntc.Samples.Octahedron.Common;

public struct Camera
{
    public Vector3 Right;
    public Vector3 Up;
    public int PixelHeight;
    public int PixelWidth;

    public static Camera Default()
    {
        return new Camera
        {
            Right = new Vector3(1, 0, 0),
            Up = new Vector3(0, 1, 0),
        };
    }
}