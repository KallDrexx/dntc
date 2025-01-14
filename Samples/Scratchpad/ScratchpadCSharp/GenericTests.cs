using ScratchpadCSharp.Dependency;

namespace ScratchpadCSharp;

public class GenericTests
{
    private interface IGetNumber
    {
        int GetNumber();
    }

    public readonly struct StaticNumberGetter(int number) : IGetNumber
    {
        public int GetNumber()
        {
            return number;
        }
    }

    public readonly struct AddNumberGetter(int first, int second) : IGetNumber
    {
        public int GetNumber()
        {
            return first + second;
        }
    }

    public static int GetStaticNumber(int x)
    {
        var getter = new StaticNumberGetter(x);
        return Run(getter);
    }

    public static int GetAddedNumber(int x, int y)
    {
        var getter = new AddNumberGetter(x, y);
        return Run(getter);
    }

    public static int GetGenericNumberFromDep(int x)
    {
        return GenericUtils.GenericReturnValue(x);
    }

    private static int Run<T>(T getter) where T : IGetNumber
    {
        return getter.GetNumber();
    }
}