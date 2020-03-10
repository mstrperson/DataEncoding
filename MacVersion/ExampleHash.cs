using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataEncoding
{
    // this allows me to use the statically defined fields and methods in 
    // the ExampleEncryption class explicitly here as well.
    using static ExampleEncryption;

    public static class ExampleHash
    {
        public static readonly int BlockSize = 8; // 8 bytes block size = 64-bit hash
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RawData"></param>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(byte[] rawData, ulong publicKey)
        {
            RandomBitGenerator random = new RandomBitGenerator(publicKey);

            // Preparation:  make sure all the arrays are the correct size...
            
            byte[] output = new byte[8];


            // Pad rawData as needed to ensure that it has an even multiple of 'totalBytes' number of bytes.
            int neededBytes = rawData.Length;

            while(neededBytes % 8 != 0)
            {
                neededBytes++;
            }

            // if Padding is necessary, create a new array of the correct length and then copy data over into it.
            if(rawData.Length != neededBytes)
            {
                byte[] temp = new byte[neededBytes];

                for (int i = 0; i < rawData.Length; i++) temp[i] = rawData[i];

                rawData = temp;
            }

            // Preparation complete:  rawData now has an even multiple of 'totalBytes' number of bytes. Padded with zeros.

            // Now for the hashing algorithm.
            for(int i = 0; i < rawData.Length; i += 8)
            {
                // grab the next block of data.
                byte[] block = new byte[8];
                for(int j = 0; j < 8; j++)
                {
                    block[j] = rawData[i + j];
                }



                // shuffle stuff around in a crazy way that 
                // makes it look random but is completely repeatable.
                for (int j = 0; j < 50; j++)
                {
                    block = HashAdd(block, output, random.NextBytes(8));
                    output = PermuteBits(block);
                }
            }

            return output;
        }

        /// <summary>
        /// Do an interesting repeatable permutation of bits
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] PermuteBits(byte[] input)
        {
            byte[] transpose = FlipFlop(input);

            for(int i = 0; i < input.Length; i++)
            {
                input[i] ^= transpose[i];
            }


            return input;
        }

        public static byte[] FlipFlop(byte[] bytes)
        {
            if(bytes.Length == 2)
            {
                return new byte[] { (byte)(0x0F ^ bytes[1]), (byte)(0xF0 ^ bytes[0]) };
            }

            if (bytes.Length % 2 == 0)
            {
                byte[] firstHalf = bytes.ToList().GetRange(0, bytes.Length / 2).ToArray();
                byte[] secondHalf = bytes.ToList().GetRange(bytes.Length / 2, bytes.Length / 2).ToArray();

                firstHalf = FlipFlop(firstHalf);
                secondHalf = FlipFlop(secondHalf);

                List<byte> flopped = new List<byte>();
                flopped.AddRange(secondHalf);
                flopped.AddRange(firstHalf);

                return flopped.ToArray();
            }
            else
            {
                byte[] firstHalf = bytes.ToList().GetRange(0, bytes.Length / 2).ToArray();
                byte[] secondHalf = bytes.ToList().GetRange(bytes.Length / 2 + 1, bytes.Length / 2).ToArray();

                firstHalf = FlipFlop(firstHalf);
                secondHalf = FlipFlop(secondHalf);

                List<byte> flopped = new List<byte>();
                flopped.AddRange(secondHalf);
                flopped.Add((byte)(0xC3 ^ bytes[bytes.Length / 2]));
                flopped.AddRange(firstHalf);

                return flopped.ToArray();
            }
        }

        public static byte[] HashAdd(byte[] a, byte[] b, byte[] convolution)
        {
            if (a.Length != b.Length) throw new InvalidOperationException("Parameters a and b must be the same length.");

            byte[] output = new byte[a.Length];

            for(int i = 0; i < a.Length; i++)
            {
                output[i] = a[i];
                for (int j = b.Length - 1; j >= 0; j--)
                {
                    output[i] |= (byte)((output[i] + b[j]) % 256); // each byte gets added together, and any overflow is truncated.
                }

                output[i] = (byte)(output[i] ^ convolution[i]);
            }

            return output;
        }
    }
}
