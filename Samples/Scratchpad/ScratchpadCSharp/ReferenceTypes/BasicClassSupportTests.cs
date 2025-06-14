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

        public int GetBaseFieldValue()
        {
            return BaseField;
        }
    }

    public class Parent : ParentBase
    {
        public int FieldValue;
        public InnerClass InnerClassInstance;
        public int FieldValueViaProperty => FieldValue;

        public Parent(int value)
        {
            FieldValue = value;
            InnerClassInstance = new InnerClass
            {
                TestValue = value,
            };
        }

        public override int Sum(int a, int b)
        {
            var result = base.Sum(a, b);

            return FieldValue + result;
        }
    }

    public class InnerClass
    {
        public int TestValue;
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

    private static int TestBaseMethodCall(Parent parent)
    {
        return parent.GetBaseFieldValue();
    }

    private static int StArgTest(InnerClass inner, bool refresh)
    {
        if (refresh)
        {
            inner = new InnerClass { TestValue = 200 };
        }

        return inner.TestValue;
    }

    public static int RefReferenceTypeTest()
    {
        var parent = new Parent(100);
        var inner = new InnerClass { TestValue = 50 };
        
        // Test ref reference type reassignment
        ModifyParentReference(ref parent);
        ModifyInnerReference(ref inner);
        
        return parent.FieldValue + inner.TestValue;
    }

    private static void ModifyParentReference(ref Parent parent)
    {
        // This should reassign the reference via double pointer
        parent = new Parent(300);
        parent.BaseField = 25;
    }

    private static void ModifyInnerReference(ref InnerClass inner)
    {
        // This should reassign the reference via double pointer  
        inner = new InnerClass { TestValue = 75 };
    }

    private static void SwapRefTest(ref InnerClass a, ref InnerClass b)
    {
        // ReSharper disable once SwapViaDeconstruction
        var temp = a;
        a = b;
        b = temp;
    }
}