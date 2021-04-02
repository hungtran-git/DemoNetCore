using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace jwt_authentication_api_example.Helpers
{
    public static class TypeConverterExtension
    {
        public static byte[] ToByteArray(this string value) =>
               Convert.FromBase64String(value);
    }
    public class AppSettings
    {
        public string Secret { get; set; }
        public string RsaPrivateKey { get; set; }
        public string RsaPublicKey { get; set; }
        public string ECDsaPrivateKey { get; set; }
        public string ECDsaPublicKey { get; set; }
    }
}
