namespace ScratchpadCSharp.ReferenceTypes;

public static class ArrayTests
{
    public struct ArrayStruct
    {
        public int Value;
    }

    public class ArrayClass
    {
        public int Value;
    }

    public static ArrayStruct[] CreateSizedArrayTest()
    {
        return new ArrayStruct[5];
    }

    public static ArrayStruct[] CreateModifiedArrayTest()
    {
        var array = CreateSizedArrayTest();
        for (var x = 0; x < array.Length; x++)
        {
            array[x].Value = x;
        }

        return array;
    }

    public static ArrayClass[] CreateReferenceTypeArray(int count)
    {
        var array = new ArrayClass[count];
        for (var x = 0; x < count; x++)
        {
            array[x] = new ArrayClass
            {
                Value = x,
            };
        }

        return array;
    }
}