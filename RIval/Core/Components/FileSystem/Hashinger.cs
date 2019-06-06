using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Ignite.Core.Components.FileSystem
{
    public static class Hashinger
    {
        public static bool CompareHash(string h1, string h2)
        {
            return h1.ToLower() == h2.ToLower();
        }

        public static bool CompareHashRaw(string fullpath, string hash)
        {
            using (var stream = File.OpenRead(fullpath))
            {
                return CompareHash(GetHash<MD5>(stream), hash);
            }
        }

        private static string GetHash<T>(Stream stream) where T : HashAlgorithm
        {
            StringBuilder sb = new StringBuilder();

            MethodInfo create = typeof(T).GetMethod("Create", new Type[] { });
            using (T crypt = (T)create.Invoke(null, null))
            {
                byte[] hashBytes = crypt.ComputeHash(stream);
                foreach (byte bt in hashBytes)
                {
                    sb.Append(bt.ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}
