using System;

namespace Cryptography
{
    class Program
    {
        static void Main(string[] args)
        {
            //var rsaDigialSignature = new RSADigialSignature();
            //rsaDigialSignature.Execute();

            var ecdsaDigialSignature = new ECDsaDigialSignature();
            ecdsaDigialSignature.Execute();
            Console.ReadKey();
        }
    }
}
