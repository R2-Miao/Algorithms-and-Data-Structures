using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman_Coding
{
    public static class HuffmanTreeBuilder
    {
        public static Dictionary<string, string> BuildTreeFrom(string input)
        {
            var charFrequency = new Dictionary<char, int>();

            foreach (char c in input)
            {
                updateFrequency(c, charFrequency);
            }

            List<HuffmanNode> huffmanNodes = createHuffmanNodes(charFrequency);
            huffmanNodes.Sort();
            buildTreeUsing(huffmanNodes);

            var huffmanCodesToSymbols = new Dictionary<string, string>();

            foreach (var node in huffmanNodes)
            {
                huffmanCodesToSymbols.Add(node.HuffmanCode, node.Symbol);
            }

            return huffmanCodesToSymbols;
        }

        private static void updateFrequency(char c, Dictionary<char, int> characterFrequency)
        {
            bool charWasSeenBefore = characterFrequency.ContainsKey(c);

            if (charWasSeenBefore)
            {
                characterFrequency[c]++;
            }
            else
            {
                characterFrequency.Add(c, 1);
            }
        }

        private static List<HuffmanNode> createHuffmanNodes(Dictionary<char, int> charFrequency)
        {
            var huffmanNodes = new List<HuffmanNode>(capacity: charFrequency.Keys.Count());

            foreach (char c in charFrequency.Keys)
            {
                var huffmanNode = new HuffmanNode { Symbol = c.ToString(), Frequency = charFrequency[c] };
                huffmanNodes.Add(huffmanNode);
            }

            return huffmanNodes;
        }

        private static void buildTreeUsing(List<HuffmanNode> huffmanNodes)
        {
            var huffmanNodesCopy = new List<HuffmanNode>(huffmanNodes);

            determineChildNodes(huffmanNodesCopy);
            determineHuffmanCodesOfLeafNodes(huffmanNodesCopy.First(), "");
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

        private static void determineHuffmanCodesOfLeafNodes(HuffmanNode node, string shortestBinaryCode)
        {
            const int LeftSubtreeBit = 0;
            const int RightSubtreeBit = 1;

            if (node.IsLeaf())
            {
                node.HuffmanCode = shortestBinaryCode;
                return;
            }

            determineHuffmanCodesOfLeafNodes(node.LeftSubtreeTopNode, shortestBinaryCode + LeftSubtreeBit.ToString());
            determineHuffmanCodesOfLeafNodes(node.RightSubtreeTopNode, shortestBinaryCode + RightSubtreeBit.ToString());
        }

        public static void PrintTreeContaining(Dictionary<string, string> huffmanCodes)
        {
            foreach (KeyValuePair<string, string> huffmanCode in huffmanCodes)
            {
                Console.WriteLine(String.Format("{0} is the Huffman code for: {1}.", huffmanCode.Key, huffmanCode.Value));
            }
        }
    }
}
