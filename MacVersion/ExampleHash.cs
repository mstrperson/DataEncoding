using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEncoding
{
    public static class ExampleHash
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RawData"></param>
        /// <param name="blockSize"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(byte[] rawData, ulong publicKey, int blockSize = 256)
        {
            if (blockSize % 8 != 0) throw new ArgumentException("blockSize must be a multiple of 8 bits.", "blockSize");

            // Preparation:  make sure all the arrays are the correct size...
            int totalBytes = blockSize / 8;
            byte[] output = new byte[totalBytes];

            ulong tempKey = publicKey;

            // Fill in the provided publicKey into the output bytes as the seed.
            for (int i = totalBytes-1; i >= 0; i--)
            {
                output[i] = (byte)(tempKey % 256);

                tempKey /= 256;
                if (tempKey == 0) tempKey = publicKey;
            }

            // Pad rawData as needed to ensure that it has an even multiple of 'totalBytes' number of bytes.
            int neededBytes = rawData.Length;

            while(neededBytes % totalBytes != 0)
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
            for(int i = 0; i < rawData.Length; i += totalBytes)
            {
                // grab the next block of data.
                byte[] block = new byte[totalBytes];
                for(int j = 0; j < totalBytes; j++)
                {
                    block[j] = rawData[i + j];
                }

                // shuffle stuff around in a crazy way that 
                // makes it look random but is completely repeatable.
                for(int j = 0; j < 75; j++)
                {
                    block = PermuteBits(block);
                }

                output = HashAdd(block, output); // HasAdd this block.
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
            byte[] transpose = new byte[input.Length];
            for(int i = 0; i < input.Length; i++)
            {
                transpose[i == 0 ? input.Length - 1 : i - 1] = input[i];
            }

            for(int i = 0; i < input.Length; i++)
            {
                input[i] ^= transpose[i];
            }

            return input;
        }

        public static byte[] HashAdd(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) throw new InvalidOperationException("Parameters a and b must be the same length.");

            byte[] output = new byte[a.Length];

            for(int i = 0; i < a.Length; i++)
            {
                output[i] = (byte)((a[i] + b[i]) % 256); // each byte gets added together, and any overflow is truncated.
            }

            return output;
        }
    }
}
