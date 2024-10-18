namespace Dntc.Samples.Octahedron.Common;

public interface IShape
{
    int TriangleCount { get; }
    Triangle GetTriangle(int index);
}