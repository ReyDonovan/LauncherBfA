using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClientHasher
{
    class Program
    {
        private static bool IsFull = false;
        private static int ServerId = -1;
        private static TimeSpan ElapsedTime = new TimeSpan();

        //Change me for custom url
        private static string downloadUrl_full = "http://wowignite.ru/cdn/full";
        //Change me for custom url
        private static string downloadUrl_lite = "http://wowignite.ru/cdn/mini";

        static void Main(string[] args)
        {
            Console.WriteLine("Choice the hash type (1 - full, 2 - lite): ");
            string res = Console.ReadLine();

            if(int.TryParse(res, out int choice))
            {
                if(choice == 1)
                {
                    IsFull = true;
                }
                else
                {
                    IsFull = false;
                }

                Console.WriteLine("Type your server id: ");

                res = Console.ReadLine();
                if(int.TryParse(res, out int server))
                {
                    Console.WriteLine($"Server id choosen: {server}");

                    ServerId = server;
                }
            }

            Make();

            Console.ReadKey();
        }

        private static void Make()
        {
            ElapsedTime = new TimeSpan(0, 0, 0, 0, 0);

            Console.WriteLine($"Start hashing: ");
            string prefix = (IsFull) ? "full" : "lite";
            using (FileStream io = new FileStream($"{prefix}_patch.rem", FileMode.OpenOrCreate))
            {
                var files = Directory.GetFiles(Environment.CurrentDirectory, "*.*", SearchOption.AllDirectories);
                Shuffle(files);

                foreach(var file in files)
                {
                    if (file.Contains("ClientHasher")) continue;
                    if (file.Contains($"{prefix}_patch.rem")) continue;

                    Console.WriteLine($"Founded: {file}.");
                    Console.Write(" -- Hashing ...");

                    string rem = (IsFull)
                        ? $"{downloadUrl_full}/{ServerId}/{file.Replace(Environment.CurrentDirectory, "").Remove(0, 1).Replace("\\", "/")}"
                        : $"{downloadUrl_lite}/{ServerId}/{file.Replace(Environment.CurrentDirectory, "").Remove(0, 1).Replace("\\", "/")}";

                    byte[] buffer = Encoding.UTF8.GetBytes($"{file.Replace(Environment.CurrentDirectory, "")}={rem}={GetHash(file)}*");
                    io.Write(buffer, 0, buffer.Length);
                }
            }

            Console.WriteLine($"Hashing stop!");
            Console.WriteLine($"All elapsed time: {ElapsedTime.TotalSeconds} sec.");
        }

        private static string GetHash(string fullPath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(fullPath))
                {
                    return GetHash<MD5>(stream);
                }
            }
        }

        private static String GetHash<T>(Stream stream) where T : HashAlgorithm
        {
            Stopwatch watcher = new Stopwatch();
            watcher.Start();

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
            watcher.Stop();
            Console.WriteLine($" OK! Elapsed: {watcher.Elapsed.TotalSeconds} sec.");
            ElapsedTime += watcher.Elapsed;

            return sb.ToString();
        }

        public static void Shuffle(string[] array)
        {
            if (array.Length < 1) return;
            var random = new Random();
            for (var i = 0; i < array.Length; i++)
            {
                var key = array[i];
                var rnd = random.Next(i, array.Length);
                array[i] = array[rnd];
                array[rnd] = key;
            }
        }
    }
}
