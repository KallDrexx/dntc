namespace ScratchpadCSharp;

public static class EnumTests
{
    public enum TestEnum { First = 1, Third = 3, }
    public enum UShortEnum : ushort { First = 1, Third = 3, }

    public static TestEnum GlobalEnumValue;

    public static void SetEnumValue(TestEnum value)
    {
        GlobalEnumValue = value;
    }

    public static TestEnum GetTestEnumValue()
    {
        return GlobalEnumValue;
    }

    public static UShortEnum GetUShortEnumValue()
    {
        return UShortEnum.Third;
    }
}