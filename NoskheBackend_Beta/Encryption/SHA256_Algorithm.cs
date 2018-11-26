using System;
using System.Security.Cryptography;
using System.Text;

namespace NoskheBackend_Beta.Encryption
{
    public static class SHA256_Algorithm
    {
        public static string Compute(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())            
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
