using System;
using MethodBoundaryAspect.Fody.Attributes;

namespace MethodBoundaryDemo
{
    public class LogAttribute : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs arg)
        {
            Console.WriteLine("Entered method: " + arg.Method.Name);
        }

        public override void OnExit(MethodExecutionArgs arg)
        {
            Console.WriteLine("Exited method: " + arg.Method.Name);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            Console.WriteLine("Exception: " + args.Exception.Message);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            SayHello();
            Console.ReadKey();
        }
        [Log]
        public static void SayHello()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
