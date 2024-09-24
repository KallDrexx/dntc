namespace Dntc.Samples.Octahedron.Common;

public readonly struct Vector3
{
    public readonly float X, Y, Z;

    public Vector3(float x, float y, float z)
    {
        (X, Y, Z) = (x, y, z);
    }

    public static Vector3 operator -(Vector3 first, Vector3 second)
    {
        return new Vector3(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
    }

    public static Vector3 operator +(Vector3 first, Vector3 second)
    {
        return new Vector3(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
    }

    public static Vector3 operator *(Vector3 vec, float scalar)
    {
        return new Vector3(vec.X * scalar, vec.Y * scalar, vec.Z * scalar);
    }

    public float Length => (float)Math.Sqrt(X * X + Y * Y + Z * Z);

    public float Dot(Vector3 other)
    {
        return X * other.X + Y * other.Y + Z * other.Z;
    }

    public Vector3 Unit => this * (1 / Length);

    public Vector3 Cross(Vector3 other)
    {
        var x = Y * other.Z - Z * other.Y;
        var y = Z * other.X - X * other.Z;
        var z = X * other.Y - Y * other.X;
        return new Vector3(x, y, z);
    }

    public Vector3 RotateOnZ(float degrees)
    {
        var rotationRadians = degrees * Math.PI / 180f;
        var currentRotation = Math.Atan2(Y, X);
        var length = (float)Math.Sqrt(X * X + Y * Y);
        var newRotation = currentRotation + rotationRadians;
        var x = (float)Math.Cos(newRotation) * length;
        var y = (float)Math.Sin(newRotation) * length;
        return new Vector3(x, y, Z);
    }

    public Vector3 RotateOnY(float degrees)
    {
        var rotationRadians = degrees * Math.PI / 180f;
        var currentRotation = Math.Atan2(Z, X);
        var length = (float)Math.Sqrt(X * X + Z * Z);
        var newRotation = currentRotation + rotationRadians;
        var x = (float)Math.Cos(newRotation) * length;
        var z = (float)Math.Sin(newRotation) * length;
        return new Vector3(x, Y, z);
    }

    public Vector3 RotateOnX(float degrees)
    {
        var rotationRadians = degrees * Math.PI / 180f;
        var currentRotation = Math.Atan2(Y, Z);
        var length = (float)Math.Sqrt(Z * Z + Y * Y);
        var newRotation = currentRotation + rotationRadians;
        var z = (float)Math.Cos(newRotation) * length;
        var y = (float)Math.Sin(newRotation) * length;
        return new(X, y, z);
    }
}