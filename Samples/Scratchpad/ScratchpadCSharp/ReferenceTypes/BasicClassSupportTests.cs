namespace ScratchpadCSharp.ReferenceTypes;

public static class BasicClassSupportTests
{
    public class ParentBase
    {
        public int BaseField;

        public virtual int Sum(int a, int b)
        {
            return a + b;
        }
    }
    
    public class Parent : ParentBase
    {
        public int FieldValue;

        public int FieldValueViaProperty => FieldValue;

        public Parent(int value)
        {
            FieldValue = value;
        }

        public override int Sum(int a, int b)
        {
            var result = base.Sum(a, b);
            
            return FieldValue + result;
        }
    }

    public static int Test()
    {
        var parent = CreateParent(10);
        return GetParentValue(parent);
    }

    private static Parent CreateParent(int value)
    {
        return new Parent(value);
    }

    private static int GetParentValue(Parent parent)
    {
        var sum = parent.Sum(1, 2);
        return parent.FieldValue + sum;
    }

    private static int GetParentValueFromProperty(Parent parent)
    {
        return parent.FieldValueViaProperty;
    }

    private static int TestBaseFieldValue(Parent parent, int value)
    {
        parent.BaseField = value;

        return parent.BaseField + 5;
    }
}