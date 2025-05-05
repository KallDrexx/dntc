// See https://aka.ms/new-console-template for more information

using System;
using Dntc.Attributes;
using HelloWorld;

namespace HelloWorld
{
    public interface IMyInterface
    {
        void Foo();
        
        void Bar();
    }

    public class ConsoleBase
    {
        private int _arg;

        public ConsoleBase(int arg)
        {
            _arg = arg;
            Program.Printf("consolebase: %u \n", arg);
        }
        
        public virtual void VirtualMethod()
        {
            Program.Printf("virtual method %u \n", _arg);
        }

        public virtual void VirtualMethod2()
        {
            Program.Printf("virtual method 2 %u \n", _arg);
        }
    }

    public class Console1 : Console, IMyInterface
    {
        public Console1(int arg) : base(arg)
        {
        }

        public override void VirtualMethod2()
        {
            base.VirtualMethod2();
            Program.Printf("Console 1 vm2 + \n");
        }

        public override void VirtualMethod()
        {
            Program.Printf("Console 1 vm + \n");
            base.VirtualMethod();
        }

        public void Foo()
        {
            Program.Printf("Console1::Foo \n");
        }

        public void Bar()
        {
            Program.Printf("Console1::Bar \n");
        }
    }

    public class Console : ConsoleBase
    {
        private int _arg1;

        public Console(int arg) : base(arg + 1)
        {
            _arg1 = arg;
            Program.Printf("Console: %u \n", arg);
        }

        public void Test(object obj)
        {
            Program.Printf("test \n");
        }
        
       public override void VirtualMethod()
        {
            base.VirtualMethod();
            Program.Printf("virtual method override %u \n", _arg1);
        }

        public override void VirtualMethod2()
        {
            base.VirtualMethod2();
            Program.Printf("virtual method 2 override %u \n", _arg1);;
        }
    }

    static class Program
    {
        public static int Value = 0;
        
        [NativeFunctionCall("printf", "<stdio.h>")]
        public static void Printf(string value)
        {
        }

        [NativeFunctionCall("printf", "<stdio.h>")]
        public static void Printf<T>(string template, T input1)
        {
            
        }

        public static void Test()
        {
            Printf("%u", Value);
            Console1 console = new Console1(1);
            IMyInterface myInterface = new Console1(2);
            console.VirtualMethod();
            console.VirtualMethod2();

            
            
            myInterface.Foo();
            myInterface.Bar();


            if (console is IMyInterface iface)
            {
                iface.Foo();
                iface.Bar();
            }
        }

        [CustomFunctionName("main")]
        public static int Main()
        {
            Test();
            return 0;
        }
    }
}