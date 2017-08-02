using System;
using System.Security.Cryptography;
using System.Text;

namespace SharpRepository.Repository.Helpers
{
    public static class Md5Helper
    {
        /// <summary>
        /// Calculates a MD5 hash from the given string and uses the given
        /// encoding.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="useEncoding">Encoding method</param>
        /// <returns>MD5 computed string</returns>
        public static string CalculateMd5(string input, Encoding useEncoding)
        {
            var md5 = MD5.Create();
            var bytes = useEncoding.GetBytes(input);
            bytes = md5.ComputeHash(bytes);
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        /// <summary>
        /// Calculates a MD5 hash from the given string. 
        /// (By using the default encoding)
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>MD5 computed string</returns>
        public static string CalculateMd5(string input)
        {
            // That's just a shortcut to the base method
            var defaultEncoding = Encoding.GetEncoding(0);
            return CalculateMd5(input, defaultEncoding);
        }
    }
}
