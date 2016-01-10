using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman_Coding
{
    public static class BitArrayExtensions
    {
        public static BitArray ToBitArray(this string zeroesAndOnesString)
        {
            int length = zeroesAndOnesString.Length;
            var result = new BitArray(length);

            for (int i = 0; i < length; i++)
            {
                char current = zeroesAndOnesString[i];

                if (!(current == '0' || current == '1'))
                {
                    throw new ArgumentException("Your input string contains characters other than '0' or '1'.");
                }

                result[i] = current == '0' ? false : true;
            }

            return result;
        }

        public static BitArray SubArray(this BitArray source, int beginIndex, int length)
        {
            var result = new BitArray(length);
            for (int sourceIndex = beginIndex; sourceIndex < beginIndex + length; sourceIndex++)
            {
                result[sourceIndex - beginIndex] = source[sourceIndex];
            }

            return result;
        }

        public static string ToBitString(this BitArray bitArray)
        {
            int length = bitArray.Length;

            var bitCharacters = new char[length];

            for (int i = 0; i < length; i++)
            {
                char bit = bitArray.Get(i) ? '1' : '0';
                bitCharacters[i] = bit;
            }

            return new string(bitCharacters);
        }

        public static BitArray WithAppendedValue(this BitArray source, bool value)
        {
            var newArray = new BitArray(source.Length + 1);
            int i = 0;
            for (; i < source.Length; i++)
            {
                newArray[i] = source[i];
            }

            newArray[i] = value;

            return newArray;
        }

    }
}
