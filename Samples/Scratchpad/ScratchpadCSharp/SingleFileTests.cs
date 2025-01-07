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
    
    [CustomFunctionName("main")]
    public static int MainFunction()
    {
        var test = new SomeStruct
        {
            Value = 33
        };
       
        DoStuff2();
        DoStuff();

        return test.Value;
    }
}