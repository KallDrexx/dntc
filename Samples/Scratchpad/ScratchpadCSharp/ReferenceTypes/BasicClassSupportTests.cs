namespace ScratchpadCSharp.ReferenceTypes;

public static class BasicClassSupportTests
{
    public class ParentBase
    {
        public virtual int Sum(int a, int b)
        {
            return a + b;
        }
    }
    
    public class Parent : ParentBase
    {
        public int FieldValue;

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

    public static void Test()
    {
        var parent = CreateParent(10);
        var value = GetParentValue(parent);
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
}