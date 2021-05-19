using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Cryptography
{
    public class ECDsaDigialSignature : IDigialSignature, IExecute
    {
        public ECParameters publicKey { get; set; }
        public ECParameters privateKey { get; set; }
        private CngAlgorithm _cngAlgorithm;


        public byte[] publicKey_byte { get; set; }
        public byte[] privateKey_byte { get; set; }

        public string publicKey_string { get; set; } = "RUNTNUIAAAABUBbrh7xrHECobnlW4pzzI2geVYdzMS/4ub65Ruh3bC7cQbVqq9EPIQ1D4UcRGLqoc7M0NIHTDsKoUZKZvaHrqeMACBEc6pTWDPKckVlD4F/MrHonG6evt9YU2dWWw9R3pDmbFeSMuAJLx+5cGb32BhbBdX0uTzuzqEexbXszLReRBr4=";
        public string privateKey_string { get; set; } = "RUNTNkIAAAABUBbrh7xrHECobnlW4pzzI2geVYdzMS/4ub65Ruh3bC7cQbVqq9EPIQ1D4UcRGLqoc7M0NIHTDsKoUZKZvaHrqeMACBEc6pTWDPKckVlD4F/MrHonG6evt9YU2dWWw9R3pDmbFeSMuAJLx+5cGb32BhbBdX0uTzuzqEexbXszLReRBr4B4UCYhKol7/p1cC9sHkn02L+IbtZmhGZn4eVWu+S1qaS5TeMOu739s36xB0LAe6j8iivns0ks1U78cIEyBfY1qEI=";

        public ECDsaDigialSignature()
        {
            _cngAlgorithm = CngAlgorithm.Sha256;
        }

        public void AssignNewKey()
        {
            using (ECDsaCng dsa = new ECDsaCng())
            {
                dsa.HashAlgorithm = _cngAlgorithm;
                publicKey_byte = dsa.Key.Export(CngKeyBlobFormat.EccPublicBlob);
                privateKey_byte = dsa.Key.Export(CngKeyBlobFormat.EccPrivateBlob);
            }
        }

        public byte[] SignData(byte[] hashOfDataToSign)
        {
            using (ECDsaCng dsa = new ECDsaCng(CngKey.Import(privateKey_byte, CngKeyBlobFormat.EccPrivateBlob)))
            {
                dsa.HashAlgorithm = _cngAlgorithm;
                return dsa.SignData(hashOfDataToSign);
            }
        }

        public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature)
        {
            using (ECDsaCng dsa = new ECDsaCng(CngKey.Import(publicKey_byte, CngKeyBlobFormat.EccPublicBlob)))
            {
                dsa.HashAlgorithm = _cngAlgorithm;
                return dsa.VerifyData(hashOfDataToSign, signature);
            }
        }

        public void Execute()
        {
            var document = Encoding.UTF8.GetBytes("Document to Sign");
            byte[] hashedDocument;

            using (var sha256 = SHA256.Create())
            {
                hashedDocument = sha256.ComputeHash(document);
            }
            
            var digitalSignature = new ECDsaDigialSignature();
            digitalSignature.AssignNewKey();

            var signature = digitalSignature.SignData(hashedDocument);
            var verified = digitalSignature.VerifySignature(hashedDocument, signature);

            Console.WriteLine("Digital Signature Demonstration in .NET");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("   Original Text = " + System.Text.Encoding.Default.GetString(document));
            Console.WriteLine();
            Console.WriteLine("   Digital Signature = " + Convert.ToBase64String(signature));
            Console.WriteLine();

            if (verified)
            {
                Console.WriteLine("The digital signature has been \"CORRECTLY\" verified.");
            }
            else
            {
                Console.WriteLine("The digital signature has \"NOT BEEN CORRECTLY\" verified.");
            }

            Console.ReadLine();
        }
    }
}
