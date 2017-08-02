using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SharpRepository.Repository.Caching.Hash
{
    public static class Utility
    {
        /// <summary>
        /// Creates an MD5 fingerprint of the string.
        /// </summary>
        public static string ToMd5Fingerprint(this string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s.ToCharArray());
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(bytes);

                // concat the hash bytes into one long string
                return hash.Aggregate(new StringBuilder(32),
                    (sb, b) => sb.Append(b.ToString("X2")))
                    .ToString();
            }
        }

        public static string ToConcatenatedString<T>(this IEnumerable<T> source, Func<T, string> selector, string separator)
        {
            var b = new StringBuilder();
            var needSeparator = false;

            foreach (var item in source)
            {
                if (needSeparator)
                    b.Append(separator);

                b.Append(selector(item));
                needSeparator = true;
            }

            return b.ToString();
        }

        public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> source)
        {
            return new LinkedList<T>(source);
        }
    }
}
