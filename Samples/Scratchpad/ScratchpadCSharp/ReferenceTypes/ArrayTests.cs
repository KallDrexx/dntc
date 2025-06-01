namespace ScratchpadCSharp.ReferenceTypes;

public static class ArrayTests
{
    public struct ArrayStruct
    {
        public int Value;
    }

    public static ArrayStruct[] CreateSizedArrayTest()
    {
        return new ArrayStruct[5];
    }
}