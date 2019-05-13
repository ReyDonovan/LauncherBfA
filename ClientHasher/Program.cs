using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientHasher
{
    class Program
    {
        private static bool IsFull = false;
        private static int ServerId = -1;

        //Change me for custom url
        private static string downloadUrl_full = "http://crux.intellixservice.ru/cdn/full";
        //Change me for custom url
        private static string downloadUrl_lite = "http://crux.intellixservice.ru/cdn/lite";

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
            Console.WriteLine($"Start hashing: ");
            string prefix = (IsFull) ? "full" : "lite";
            using (FileStream io = new FileStream($"{prefix}_patch.rem", FileMode.OpenOrCreate))
            {
                var files = Directory.GetFiles(Environment.CurrentDirectory, "*.*", SearchOption.AllDirectories);

                foreach(var file in files)
                {
                    if (file.Contains("ClientHasher")) continue;
                    if (file.Contains($"{prefix}_patch.rem")) continue;

                    Console.WriteLine($"Founded: {file}.");
                    string rem = (IsFull)
                        ? $"{downloadUrl_full}/{ServerId}/{file.Replace(Environment.CurrentDirectory, "").Remove(0, 1).Replace("\\", "/")}"
                        : $"{downloadUrl_lite}/{ServerId}/{file.Replace(Environment.CurrentDirectory, "").Remove(0, 1).Replace("\\", "/")}";

                    byte[] buffer = Encoding.UTF8.GetBytes($"{file.Replace(Environment.CurrentDirectory, "")}={rem}*");
                    io.Write(buffer, 0, buffer.Length);
                }
            }

            Console.WriteLine($"Hashing stop!");
        }
    }
}
