using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman_Coding
{
    public class HuffmanNode : IComparable<HuffmanNode>
    {
        public string Symbol { get; set; }
        public int Frequency { get; set; }
        public string HuffmanCode { get; set; }

        public HuffmanNode LeftSubtreeTopNode { get; set; }
        public HuffmanNode RightSubtreeTopNode { get; set; }

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
}
