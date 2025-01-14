using Dntc.Attributes;

namespace ScratchpadCSharp.Dependency;

public static class GenericUtils
{
    [NativeFunctionCall("printf", "<stdio.h>")]
    public static void Printf<T>(string template, T input1)
    {
        
    }

    public static T GenericReturnValue<T>(T value)
    {
        return value;
    }
}