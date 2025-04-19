using Dntc.Attributes;
using ScratchpadCSharp.Dependency;

namespace ScratchpadCSharp;

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

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
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

    public static float SquareRootTest(float value)
    {
        return (float)Math.Sqrt(value);
    }

    public static ushort ArrayTest(ushort[] test)
    {
        for (var x = 0; x < test.Length; x++)
        {
            test[x] = (ushort)x;
        }

        ushort sum = 0;
        for (var x = 0; x < test.Length; x++)
        {
            sum += test[x];
        }

        return sum;
    }

    public static Vector3 ConstructorTest(float x, float y, float z)
    {
        return new Vector3(x, y, z);
    }

    public static void RefTest(ref Vector3 vector, ref float floatToAdd, float amount)
    {
        floatToAdd += amount;
        vector.X += amount;
        vector.Y += amount;
        vector.Z += amount;
    }

    public static int SwapTest(int x, int y)
    {
        // Tests we can handle when release builds optimize out the temp variable. This code will compile
        // down into 2 loads then 2 stores without a temp variable. This works fine with a runtime
        // evaluation stack but breaks down when transpiling it.
        // ReSharper disable once SwapViaDeconstruction
        var temp = x;
        x = y;
        y = temp;
        return y;
    }

    public static float LocalSwapTest(float x0, float x1)
    {
        var first = new Vector2 { X = x0, Y = 0 };
        var second = new Vector2 { X = x1, Y = 0 };

        var temp = first;
        first = second;
        second = temp;

        return second.X;
    }

    public static int TernaryTest(int a, int b)
    {
        var diff = a < b
            ? b - a
            : a - b;
        
        return diff + 3;
    }

    private static int SomeStaticInt;

    public static int IncrementStaticInt()
    {
        SomeStaticInt++;
        return SomeStaticInt;
    }

    public static Vector3 AStaticVector = new Vector3(10, 11, 12);

    public static Vector3 GetStaticVector()
    {
        return AStaticVector;
    }

    public static int GlobalWithNoInitialValue;

    public static int ReturnOne()
    {
        return 1;
    }

    public static unsafe delegate*<int> GetFunctionPointer()
    {
        return &SimpleFunctions.ReturnOne;
    }

    public static unsafe int RunFunctionPointer(delegate*<int> function)
    {
        return function();
    }

    public static void SetOtherAssemblyFieldValue(int value)
    {
        Misc.FieldInAnotherAssembly = value;
    }

    public static int GetOtherAssemblyFieldValue()
    {
        return Misc.FieldInAnotherAssembly;
    }

    public static void LdIndRefTest(ref ushort[] array, int index, ushort value)
    {
        array[index] = value;
    }
}