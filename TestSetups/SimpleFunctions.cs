namespace TestSetups;

public static class SimpleFunctions
{
    public static int IntAdd(int a, int b)
    {
        return a + b;
    }

    public static int IfTest(int input)
    {
        if (input < 20)
        {
            return -1;
        }

        if (input >= 50)
        {
            return 100;
        }

        return input;
    }

    public static int StaticFunctionCall(int a)
    {
        IntAdd(a, 5);
        return IntAdd(a, 20);
    }

    public static int BitwiseOps(int a)
    {
        return (((a >> 1) | (a & 0x0F)) << 2) ^ 0xFF;
    }

    public static float MathOps(int input)
    {
        return input + 3.5f * input / 3 - 2 + (25 % 15);
    }

    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;
        
        public static Vector3 operator +(Vector3 first, Vector3 second)
        {
            // return new Vector3(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
            return new Vector3
            { 
                X = first.X + second.X, 
                Y = first.Y + second.Y, 
                Z = first.Z + second.Z
            };
        }

        public float Dot(Vector3 other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }
    }

    public struct Vector2
    {
        public float X;
        public float Y;
    }

    public struct Triangle
    {
        public Vector2 First;
        public Vector2 Second;
        public Vector2 Third;
    }

    public static Vector3 StructTest(float x, float y, float z)
    {
        return new Vector3
        {
            X = x,
            Y = y,
            Z = z,
        };
    }

    public static Vector3 StructAddTest(Vector3 first, Vector3 second)
    {
        return new Vector3
        {
            X = first.X + second.X,
            Y = first.Y + second.Y,
            Z = first.Z + second.Z,
        };
    }

    public static Vector2 Vector2Add(Vector2 first, Vector2 second)
    {
        return new Vector2
        {
            X = first.X + second.X,
            Y = first.Y + second.Y,
        };
    }

    public static Triangle TriangleBuilder(float x0, float y0, float x1, float y1, float x2, float y2)
    {
        return new Triangle
        {
            First = new Vector2
            {
                X = x0,
                Y = y0,
            },

            Second = new Vector2
            {
                X = x1,
                Y = y1,
            },

            Third = new Vector2
            {
                X = x2,
                Y = y2,
            },
        };
    }

    public static Triangle TriangleAdd(Triangle a, Triangle b)
    {
        return new Triangle
        {
            First = Vector2Add(a.First, b.First),
            Second = Vector2Add(a.Second, b.Second),
            Third = Vector2Add(a.Third, b.Third),
        };
    }

    public static float StructInstanceTest(Vector3 first, Vector3 second)
    {
        return first.Dot(second);
    }

    public static Vector3 StructOpOverload(Vector3 first, Vector3 second)
    {
        return first + second;
    }

    public static unsafe int FnPointerTest(delegate*<int, int, int> fn, int x, int y)
    {
        return fn(x, y);
    }
}