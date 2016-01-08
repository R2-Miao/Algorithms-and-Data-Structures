using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Huffman_Coding
{
    public static class HuffmanTranslator
    {
        public static string EncodeUsing(this string inputString, Dictionary<string, string> huffmanCodesToSymbols)
        {
            var encodedMessage = new StringBuilder();

            foreach (char c in inputString)
            {
                var correspondingHuffmanCodes = huffmanCodesToSymbols.Where(kvp => kvp.Value == c.ToString());

                if (correspondingHuffmanCodes.Count() > 1)
                {
                    throw new ArgumentException(String.Format("The character '{0}' has more than one corresponding Huffman code.", c));
                }

                encodedMessage.Append(correspondingHuffmanCodes.First().Key);
            }

            return encodedMessage.ToString();
        }

        public static string DecodeUsing(this string encodedStringInBinary, Dictionary<string, string> huffmanCodesToSymbols)
        {
            var decodedMessage = new StringBuilder();
            int currentIndex = 0;
            int codeLength = 1;

            while (currentIndex < encodedStringInBinary.Length)
            {
                if (currentIndex + codeLength > encodedStringInBinary.Length)
                {
                    throw new ArgumentException("Decoding failed: Either the encoded message or the Huffman codes provided, or both, contain errors.");
                }

                string huffmanCode = encodedStringInBinary.Substring(currentIndex, codeLength);

                if (huffmanCodesToSymbols.ContainsKey(huffmanCode))
                {
                    decodedMessage.Append(huffmanCodesToSymbols[huffmanCode]);

                    currentIndex += codeLength;
                    codeLength = 1;
                }
                else
                {
                    codeLength++;
                }
            }

            return decodedMessage.ToString();
        }
    }
}
