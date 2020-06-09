using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BunnyCDN.Net.Storage
{
    internal class Checksum
    {
        internal static string Generate(Stream stream)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(stream);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}