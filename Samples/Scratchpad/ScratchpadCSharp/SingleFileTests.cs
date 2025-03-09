using Dntc.Attributes;

namespace ScratchpadCSharp;

public static class SingleFileTests
{
    public struct SomeStruct
    {
        public int Value;
    }

    [NativeFunctionCall("do_stuff", "header_test1.h")]
    public static void DoStuff()
    {
        
    }

    [NativeFunctionCall("do_stuff2", "header_test2.h")]
    public static void DoStuff2()
    {
        
    }

    [CustomFunction("#define addOneMacro(a) ((a) + 1)", null, "addOneMacro")]
    public static int AddOneMacro(int a)
    {
        return 0;
    }

    [StaticallySizedArray(10)]
    [InitialGlobalValue("{1,2,3,4,5,6,7,8,9,10}")]
    public static int[] NumberArray;

    [CustomFunctionName("main")]
    public static int MainFunction()
    {
        var test = new SomeStruct
        {
            Value = 33
        };
       
        DoStuff2();
        DoStuff();

        var output = AddOneMacro(10);
        var number = NumberArray[3];

        return test.Value;
    }
}