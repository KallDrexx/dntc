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
        return IntAdd(a, 20);
    }
}