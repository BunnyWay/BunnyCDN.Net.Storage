using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BunnyCDN.Net.Storage
{
    internal class Checksum
    {
        internal static string Generate(Stream stream)
        {
            using (var sha = SHA256.Create())
            {
                byte[] checksumData = sha.ComputeHash(stream);
                return BitConverter.ToString(checksumData).Replace("-", String.Empty);
            }
        }
    }
}