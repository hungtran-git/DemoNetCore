using System;
using HelloWorld_NetFramework_Attributes;

namespace MethodBoundaryAspect
{
    [HelloWorldAspect]
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
