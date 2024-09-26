namespace Dntc.Samples.Octahedron.Common;

public static class OctahedronBuilder
{
    public static void SetupTriangles(Triangle[] triangles)
    {
        triangles[0] = new Triangle(
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, 1));

        triangles[1] = new Triangle(
            new Vector3(1, 0, 0),
            new Vector3(0, 0, -1),
            new Vector3(0, 1, 0));

        triangles[2] = new Triangle(
            new Vector3(1, 0, 0),
            new Vector3(0, 0, -1),
            new Vector3(0, 1, 0));

        triangles[3] = new Triangle(
            new Vector3(1, 0, 0),
            new Vector3(0, -1, 0),
            new Vector3(0, 0, -1));

        triangles[4] = new Triangle(
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, 1),
            new Vector3(0, 1, 0));

        triangles[5] = new Triangle(
            new Vector3(-1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, -1));

        triangles[6] = new Triangle(
            new Vector3(-1, 0, 0),
            new Vector3(0, -1, 0),
            new Vector3(0, 0, 1));

        triangles[7] = new Triangle(
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, -1),
            new Vector3(0, -1, 0));
    }
}