using Dntc.Attributes;

namespace ScratchpadCSharp;

public static class SingleFileTests
{
    public struct SomeStruct
    {
        public int Value;
    }
    
    [CustomNameOnTranspile("main")]
    public static int MainFunction()
    {
        var test = new SomeStruct
        {
            Value = 33
        };

        return test.Value;
    }
}