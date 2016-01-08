using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Huffman_Coding
{
    class Program
    {
        static void Main(string[] args)
        {
            string testPhrase = "j'aime aller sur le bord de l'eau les jeudis ou les jours impairs";

            Dictionary<string, string> huffmanCodes = HuffmanTreeBuilder.BuildTreeFrom(testPhrase);
            HuffmanTreeBuilder.PrintTreeContaining(huffmanCodes);

            string encodedTestPhrase = testPhrase.EncodeUsing(huffmanCodes);
            Console.WriteLine("Encoded test phrase: " + encodedTestPhrase);

            string decodedTestPhrase = encodedTestPhrase.DecodeUsing(huffmanCodes);
            Console.WriteLine("Decoded test phrase: " + decodedTestPhrase);
            Console.WriteLine("The decoded message is {0} as the original message.", testPhrase == decodedTestPhrase ? "the same" : "not the same");
            
            Console.WriteLine("Press any key to exit the program: ");
            Console.ReadKey();
        }
    }
}
