// See https://aka.ms/new-console-template for more information

using System;
using Dntc.Attributes;
using HelloWorld;

namespace HelloWorld
{
    public class ConsoleBase
    {
        private int _arg;

        public ConsoleBase(int arg)
        {
            _arg = arg;
            Program.Printf("consolebase: %u", arg);
        }
        
        public virtual void VirtualMethod()
        {
            Program.Printf("virtual method %u", _arg);
        }
    }

    public class Console : ConsoleBase
    {
        private int _arg1;

        public Console(int arg) : base(arg + 1)
        {
            _arg1 = arg;
            Program.Printf("Console: %u", arg);
        }

        public void Test(object obj)
        {
            Program.Printf("test");
        }
        
        public override void VirtualMethod()
        {
            base.VirtualMethod();
            Program.Printf("virtual method override %u", _arg1);
        }
    }

    static class Program
    {
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
            var console = new Console(1);
            
            console.VirtualMethod();
        }

        [CustomFunctionName("main")]
        public static int Main()
        {
            Test();
            return 0;
        }
    }
}