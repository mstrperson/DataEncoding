using System;
using System.Collections.Generic;
using System.Linq;
namespace DataEncoding
{
    using static ExampleEncryption;

    public class RandomBitGenerator
    {

        private ulong key;

        private ulong state;

        private void UpdateState()
        {
            // hold onto the current state as it is used in the calculation
            // We can't just say 'state = Math.Pow(state, seed);', because 
            // that number will likely be so big you blow up your computer.
            ulong temp = state;

            // These numbers are VERY large, so in order to make sure we 
            // don't overflow a 64-bit integer, we need to do this exponent
            // one multiplication at a time and % out the prime at each step.
            for (ulong i = 0; i < this.key; i++)
            {
                temp = (temp * state) % prime;
            }

            // now store the new state.
            state = temp;
        }

        public byte[] NextBytes(int count)
        {
            if (count <= 0) throw new ArgumentOutOfRangeException("count");

            UpdateState();
            byte[] bytes = new byte[count];
            ulong temp = state;
            for(int i = 0; i < count; i++)
            {
                bytes[i] = (byte)(temp % 256);
                temp /= 256;
                if (temp == 0) temp = state;
            }

            return bytes;
        }

        public RandomBitGenerator(ulong key)
        {
            this.key = key;

        }
    }
}
