using Dntc.Attributes;

namespace ScratchpadCSharp;

public static class SingleFileTests
{
    [CustomNameOnTranspile("main")]
    public static int MainFunction()
    {
        return 0;
    }
}