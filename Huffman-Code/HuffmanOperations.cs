using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Huffman_Coding
{
    internal class HuffmanNode : IComparable<HuffmanNode>
    {
        public string Symbol { get; set; }
        public int Frequency { get; set; }
        public string HuffmanCode { get; set; }

        public HuffmanNode LeftSubtreeTopNode { get; set; }
        public HuffmanNode RightSubtreeTopNode { get; set; }

        public bool PositionInTreeFound { get; set; }

        public HuffmanNode JoinWith(HuffmanNode other)
        {
            var parentNode = new HuffmanNode() { Symbol = this.Symbol + other.Symbol, Frequency = this.Frequency + other.Frequency };
            parentNode.LeftSubtreeTopNode = this.Frequency >= other.Frequency ? this : other;
            parentNode.RightSubtreeTopNode = this.Frequency < other.Frequency ? this : other;

            return parentNode;
        }

        public bool IsLeaf()
        {
            return this.LeftSubtreeTopNode == null && this.RightSubtreeTopNode == null;
        }

        public int CompareTo(HuffmanNode other)
        {
            return this.Frequency.CompareTo(other.Frequency);
        }
    }

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
