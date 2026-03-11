using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace WebSharp
{
    public class Crypto
    {
        private static string ComputeHash(HashAlgorithm hashAlgorithm, string value, Encoding encoding)
        {
            byte[] hash = hashAlgorithm.ComputeHash(encoding.GetBytes(value));

            StringBuilder hex = new StringBuilder(hash.Length);
            for (int i = 0; i < hash.Length; i++)
                hex.AppendFormat("{0:x2}", hash[i]); //  x.ToString("x2");

            return hex.ToString();
        }

        private static string ComputeHash(HashAlgorithm hashAlgorithm, byte[] value)
        {
            byte[] hash = hashAlgorithm.ComputeHash(value);

            StringBuilder hex = new StringBuilder(hash.Length);
            for (int i = 0; i < hash.Length; i++)
                hex.AppendFormat("{0:x2}", hash[i]); //  x.ToString("x2");

            return hex.ToString();
        }


        public static byte[] GetSHA256Raw(string value)
        {
            if (String.IsNullOrEmpty(value)) return null;

            using (SHA256 provider = SHA256.Create())
            {
                return provider.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }

        public static string GetSHA256(string value)
        {
            using (SHA256 provider = SHA256.Create())
            {
                return ComputeHash(provider, value, Encoding.UTF8);
            }
        }
    }
}
