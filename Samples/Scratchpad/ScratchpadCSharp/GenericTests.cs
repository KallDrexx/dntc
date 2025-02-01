﻿using System.Runtime.InteropServices;
using Dntc.Attributes;
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

    public static unsafe int* GenericPointerTest()
    {
        var value = GenericPointerReturnTypeTest<int>(4);
        *value = 25;

        return value;
    }

    [NativeFunctionCall("generic_pointer_return_type_test", "../native_test.h")]
    private static unsafe TItem* GenericPointerReturnTypeTest<TItem>(int size) where TItem : unmanaged
    {
        // TODO: Support sizeof function
        return (TItem*) Marshal.AllocHGlobal(size);
    }

    [NativeFunctionCall("sizeof", null)]
    private static int SizeOfDeref<T>(ref T obj)
    {
        return 0;
    }
}