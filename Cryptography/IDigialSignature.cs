using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptography
{
    public interface IDigialSignature
    {
        public void AssignNewKey();
        public byte[] SignData(byte[] hashOfDataToSign);
        public bool VerifySignature(byte[] hashOfDataToSign, byte[] signature);
    }
}
