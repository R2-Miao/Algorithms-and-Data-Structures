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
        /// <summary>
        /// HuffmanNode is a node in a binary tree.
        /// </summary>
        private class HuffmanNode : IComparable<HuffmanNode>
        {
            public string Characters { get; set; }
            public int NumOccurrences { get; set; }
            public BitArray HuffmanCode { get; set; }

            public HuffmanNode Left { get; set; }
            public HuffmanNode Right { get; set; }

            public HuffmanNode JoinWith(HuffmanNode other)
            {
                var parentNode = new HuffmanNode() { Characters = this.Characters + other.Characters, NumOccurrences = this.NumOccurrences + other.NumOccurrences };
                parentNode.Left = this.NumOccurrences >= other.NumOccurrences ? this : other;
                parentNode.Right = this.NumOccurrences < other.NumOccurrences ? this : other;

                return parentNode;
            }

            public bool IsLeaf
            {
                get
                {
                    return this.Characters.Length == 1;
                }
            }

            public int CompareTo(HuffmanNode other)
            {
                return this.NumOccurrences.CompareTo(other.NumOccurrences);
            }
        }
    }
}
