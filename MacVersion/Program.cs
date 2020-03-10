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
            string text1 = "abc";
            string text2 = "abcasdfasdgasdfa";



            FileDataEncoder encoder1 = new FileDataEncoder(Encoding.ASCII.GetBytes(text1)) { EncryptionAlgorithm = new ExampleEncryption((ulong)Math.Pow(2, 17) - 31) };
            FileDataEncoder encoder2 = new FileDataEncoder(Encoding.ASCII.GetBytes(text2)) { EncryptionAlgorithm = new ExampleEncryption((ulong)Math.Pow(2, 17) - 31) };

            Console.WriteLine("Pre encryption hash:");
            Console.WriteLine(encoder1.Base64Hash);
            Console.WriteLine(encoder2.Base64Hash);

            /*
            encoder.Encrypt();
            Console.WriteLine(encoder);
            Console.WriteLine("post encryption hash:");
            Console.WriteLine(encoder.Base64Hash);

            encoder.EncryptionAlgorithm.Reset();

            encoder.Decrypt();
            Console.WriteLine(encoder);
            */
            Console.ReadLine();
        }
    }
}
