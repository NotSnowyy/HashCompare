using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HashCompare
{
    public class Files
    {
        public Files(string FileName, string FileHash, bool Unique)
        {
            this.FileName = FileName;
            this.FileHash = FileHash;
            this.Unique = Unique;
        }

        public string FileName { get; set; }
        public string FileHash { get; set; }
        public bool Unique { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Drop all your files on me UwU.");
                Console.ReadKey();

                return;
            }

            var fileList = new List<Files>();

            for (int i = 0; i < args.Length; i++)
            {
                string fileName = Path.GetFileName(args[i]);
                byte[] hashBuffer = File.ReadAllBytes(args[i]);
                byte[] hash;

                using (SHA256 sha = SHA256.Create())
                    hash = sha.ComputeHash(hashBuffer);

                fileList.Add(new Files(fileName, HashToString(hash), false));
            }

            for (int i = 0; i < fileList.Count; i++)
            {
                for (int j = i + 1; j < fileList.Count; j++)
                {
                    if (fileList[i].FileHash.Equals(fileList[j].FileHash))
                        fileList[i].Unique = fileList[j].Unique = true;
                }
            }

            foreach (var file in fileList)
            {
                if (file.Unique)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0} -> {1}", file.FileHash, file.FileName);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("{0} -> {1}", file.FileHash, file.FileName);
                }
            }

            Console.ReadKey();
        }

        static string HashToString(byte[] hash)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
                stringBuilder.Append(hash[i].ToString("x2"));

            return stringBuilder.ToString();
        }
    }
}
