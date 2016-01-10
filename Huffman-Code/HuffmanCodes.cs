using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman_Coding
{
    public static partial class HuffmanCodes
    {
        public static Dictionary<char, BitArray> DetermineHuffmanCodes(string input)
        {
            var charFrequencies = new Dictionary<char, int>();

            foreach (char c in input)
            {
                if (charFrequencies.ContainsKey(c))
                {
                    charFrequencies[c]++;
                }
                else
                {
                    charFrequencies[c] = 1;
                }
            }

            List<HuffmanNode> singleCharHuffmanNodes = createHuffmanNodes(charFrequencies);
            singleCharHuffmanNodes.Sort();

            writeHuffmanCodesToLeafNodes(singleCharHuffmanNodes);

            var huffmanCodesToCharacters = new Dictionary<char, BitArray>();

            foreach (var node in singleCharHuffmanNodes)
            {
                huffmanCodesToCharacters.Add(node.Characters[0], node.HuffmanCode);
            }

            return huffmanCodesToCharacters;
        }

        private static List<HuffmanNode> createHuffmanNodes(Dictionary<char, int> charFrequency)
        {
            var huffmanNodes = new List<HuffmanNode>(capacity: charFrequency.Keys.Count());

            foreach (char c in charFrequency.Keys)
            {
                var huffmanNode = new HuffmanNode { Characters = c.ToString(), NumOccurrences = charFrequency[c] };
                huffmanNodes.Add(huffmanNode);
            }

            return huffmanNodes;
        }

        private static void writeHuffmanCodesToLeafNodes(List<HuffmanNode> singleCharHuffmanNodes)
        {
            var huffmanNodesCopy = new List<HuffmanNode>(singleCharHuffmanNodes);

            determineChildNodes(huffmanNodesCopy);
            determineHuffmanCodesOfLeafNodes(huffmanNodesCopy.First(), new BitArray(0));
        }

        private static void determineChildNodes(List<HuffmanNode> huffmanNodes)
        {
            while (huffmanNodes.Count > 1)
            {
                huffmanNodes.Sort();

                List<HuffmanNode> twoLeastFrequentNodes = huffmanNodes.Take(2).ToList();
                HuffmanNode leastFrequentNode = twoLeastFrequentNodes.First();
                HuffmanNode secondLeastFrequentNode = twoLeastFrequentNodes.Last();
                huffmanNodes.RemoveRange(0, 2);

                HuffmanNode parentNode = leastFrequentNode.JoinWith(secondLeastFrequentNode);
                huffmanNodes.Add(parentNode);
            }
        }

        private static void determineHuffmanCodesOfLeafNodes(HuffmanNode node, BitArray shortestBinaryCode)
        {
            const bool Zero = false;
            const bool One = true;

            if (node.IsLeaf)
            {
                node.HuffmanCode = shortestBinaryCode;
                return;
            }

            determineHuffmanCodesOfLeafNodes(node.Left, shortestBinaryCode.WithAppendedValue(Zero));
            determineHuffmanCodesOfLeafNodes(node.Right, shortestBinaryCode.WithAppendedValue(One));
        }

        public static void PrintTreeContaining(Dictionary<char, BitArray> huffmanCodes)
        {
            foreach (KeyValuePair<char, BitArray> huffmanCode in huffmanCodes)
            {
                Console.WriteLine(String.Format("{0} has the Huffman code: {1}.", huffmanCode.Key, huffmanCode.Value.ToBitString()));
            }
        }
    }
}
