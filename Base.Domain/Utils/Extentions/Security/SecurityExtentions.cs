using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Utils.Extentions.Security
{
    public static class SecurityExtentions
    {
        /// <summary>
        /// Hashes String In SHA256 Protocol
        /// </summary>
        /// <param name="src">Source String That you want To Hash</param>
        /// <returns></returns>
        public static string HashData(this string src)
        {
            SHA256 sha256 = SHA256.Create();

            byte[] buffer = Encoding.UTF8.GetBytes(src);

            var resultBytes = sha256.ComputeHash(buffer);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < resultBytes.Length; i++)
            {
                builder.Append(resultBytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        /// <summary>
        /// Hashes BinaryData In SHA256 Protocol
        /// </summary>
        /// <param name="src">Binary Data That you want To Hash</param>
        /// <returns></returns>
        public static string HashData(this byte[] src)
        {
            SHA256 sha256 = SHA256.Create();

            var resultBytes = sha256.ComputeHash(src);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < resultBytes.Length; i++)
            {
                builder.Append(resultBytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
