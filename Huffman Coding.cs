using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Huffman_Coding
{
    public class HuffmanNode : IComparable<HuffmanNode>
    {
        public string String { get; set; }
        public int Frequency { get; set; }
        public string HuffmanCode { get; set; }

        public HuffmanNode LeftSubtreeTopNode { get; set; }
        public HuffmanNode RightSubtreeTopNode { get; set; }
        
        public HuffmanNode JoinWith(HuffmanNode other)
        {
            var parentNode = new HuffmanNode() { String = this.String + other.String, Frequency = this.Frequency + other.Frequency };
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
        public static void PrintTreeFromFile (string filePath)
        {
            string input = File.ReadAllText(filePath);

            var charFrequency = new Dictionary<char, int>();

            foreach (char c in input)
            {
                updateFrequency(c, charFrequency);
            }

            List<HuffmanNode> huffmanNodes = createHuffmanNodes(charFrequency);
            huffmanNodes.Sort();

            printTreeContaining(huffmanNodes);               
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
                var huffmanNode = new HuffmanNode { String = c.ToString(), Frequency = charFrequency[c] };
                huffmanNodes.Add(huffmanNode);
            }

            return huffmanNodes;
        }

        private static void printTreeContaining(List<HuffmanNode> huffmanNodes)
        {
            buildTreeUsing(huffmanNodes);

            foreach (HuffmanNode node in huffmanNodes)
            {
                Console.WriteLine(String.Format("The Huffman code for {0} is: {1}", node.String, node.HuffmanCode.ToString()));
            }
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
    }

    class Program
    {
        static void Main(string[] args)
        {
            //The file contains the phrase "j'aime aller sur le bord de l'eau les jeudis ou les jours impairs"
            HuffmanTreeBuilder.PrintTreeFromFile(@"G:\Documents\testFile.txt");

            Console.WriteLine("Press any key to exit the program: ");
            Console.ReadKey();
        }
    }
}
