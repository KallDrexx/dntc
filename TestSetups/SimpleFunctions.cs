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
        return input + 3.5f * input / 3 - 2;
    }

    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;
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
}