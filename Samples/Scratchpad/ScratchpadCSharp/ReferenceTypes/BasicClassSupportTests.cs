namespace ScratchpadCSharp.ReferenceTypes;

public static class BasicClassSupportTests
{
    public class Parent
    {
        public int FieldValue;

        public Parent(int value)
        {
            FieldValue = value;
        }
    }

    public static Parent CreateParent(int value)
    {
        return new Parent(value);
    }

    public static int GetParentValue(Parent parent)
    {
        return parent.FieldValue;
    }
}