using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DataEncoding
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.WriteLine("Gimme some text!");
            string text = Console.ReadLine();

            Console.WriteLine("Preparing the encoder...");
            FileDataEncoder encoder = new FileDataEncoder(Encoding.ASCII.GetBytes(text)) { EncryptionAlgorithm = new ExampleEncryption((ulong)Math.Pow(2, 8) - 31) };

            Console.WriteLine("Beginning Encryption.\n");
            encoder.Encrypt();

            Console.WriteLine("Encrypted Data:");

            Console.ForegroundColor = Console.BackgroundColor == ConsoleColor.White ? ConsoleColor.DarkBlue : ConsoleColor.Blue;
            Console.WriteLine(encoder);
            Console.ForegroundColor = defaultColor;

            encoder.EncryptionAlgorithm.Reset();

            Console.WriteLine("Beginning Decryption.\n");
            encoder.Decrypt();

            Console.WriteLine("Decrypted Message:");
            Console.ForegroundColor = Console.BackgroundColor == ConsoleColor.White ? ConsoleColor.DarkGreen : ConsoleColor.Green;
            Console.WriteLine(encoder);

            Console.ForegroundColor = defaultColor;

            Console.WriteLine("\nDone!");
        }
    }
}
