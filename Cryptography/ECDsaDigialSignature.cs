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
        private HashAlgorithmName _hashAlgorithmName;


        public byte[] publicKey_byte { get; set; }
        public byte[] privateKey_byte { get; set; }

        public string publicKey_string { get; set; }
        public string privateKey_string { get; set; }

        public ECDsaDigialSignature()
        {
            _hashAlgorithmName = HashAlgorithmName.SHA256;
        }

        public void AssignNewKey()
        {
            using (ECDsaCng dsa = new ECDsaCng())
            {
                dsa.HashAlgorithm = CngAlgorithm.Sha256;
                publicKey_byte = dsa.Key.Export(CngKeyBlobFormat.EccPublicBlob);
                privateKey_byte = dsa.Key.Export(CngKeyBlobFormat.EccPrivateBlob);

                publicKey_string = Convert.ToBase64String(publicKey_byte);
                privateKey_string = Convert.ToBase64String(privateKey_byte);

            }
        }

        public byte[] SignData(byte[] hashOfDataToSign)
        {
            using (ECDsaCng dsa = new ECDsaCng(CngKey.Import(privateKey_byte, CngKeyBlobFormat.EccPrivateBlob)))
            {
                dsa.HashAlgorithm = CngAlgorithm.Sha256;
                return dsa.SignData(hashOfDataToSign);
            }
        }

        public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature)
        {
            using (ECDsaCng ecsdKey = new ECDsaCng(CngKey.Import(publicKey_byte, CngKeyBlobFormat.EccPublicBlob)))
            {
                return ecsdKey.VerifyData(hashOfDataToSign, signature);
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
