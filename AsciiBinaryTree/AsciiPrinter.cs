using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiBinaryTree
{
    public class AsciiPrinter
    {
        public class AsciiNode
        {
            public AsciiNode left = null;
            public AsciiNode right = null;
            //length of the edge from this node to its children
            public int edgeLength = 0;
            public int height = 0;
            
            //-1=I am left, 0=I am root, 1=right   
            public int parentDir = 0;

            public string label = string.Empty;
        }

        private const int INFINITY_NUM = (1 << 20);
        
        //used for printing next node in the same level, this is the x coordinate of the next char printed
        private int m_printNext = 0;
    
        private const int MAX_HEIGHT = 1000;
        private int[] m_lProfile = new int [MAX_HEIGHT];
        private int[] m_rProfile = new int [MAX_HEIGHT];

        //adjust gap between left and right nodes
        private const int GAP = 3;

        //prints ascii tree for given Tree structure
        public void PrintAsciiTree(TreeNode t)
        {
            if (t == null) {
                return;
            }

            AsciiNode proot = BuildAsciiTree(t);
            ComputeEdgeLengths(proot);

            for (int i = 0; i < proot.height && i < MAX_HEIGHT; i++) {
                m_lProfile[i] = INFINITY_NUM;
            }

            ComputeLProfile(proot, 0, 0);

            int xMin = 0;
            for (int i = 0; i < proot.height && i < MAX_HEIGHT; i++) {
                xMin = Math.Min(xMin, m_lProfile[i]);
            }

            for (int i = 0; i < proot.height; i++)
            {
                m_printNext = 0;
                PrintLevel(proot, -xMin, i);
                Console.Write("\n");
            }

            if (proot.height >= MAX_HEIGHT) {
                Console.Write("(This tree is taller than %d, and may be drawn incorrectly.)\n", MAX_HEIGHT);
            }
        }

        private AsciiNode BuildAsciiTreeRecursive(TreeNode t)
        {
            if (t == null) {
                return null;
            }

            AsciiNode node = new AsciiNode();
            node.left = BuildAsciiTreeRecursive(t.left);
            node.right = BuildAsciiTreeRecursive(t.right);

            if (node.left != null) {
                node.left.parentDir = -1;
            }

            if (node.right != null) {
                node.right.parentDir = 1;
            }

            node.label = t.val.ToString();
            
            return node;
        }

        //Copy the tree into the ascii node structre
        AsciiNode BuildAsciiTree(TreeNode t)
        {
            if (t == null)
            {
                return null;
            }

            AsciiNode node = BuildAsciiTreeRecursive(t);
            node.parentDir = 0;

            return node;
        }

        //The following function fills in the m_lProfile array for the given tree.
        //It assumes that the center of the label of the root of this tree
        //is located at a position (x,y).  It assumes that the edgeLength
        //fields have been computed for this tree.
        void ComputeLProfile(AsciiNode node, int x, int y)
        {
            if (node == null) {
                return;
            }

            int isleft = (node.parentDir == -1) ? 1 : 0;
            m_lProfile[y] = Math.Min(m_lProfile[y], x - ((node.label.Length - isleft) / 2));
            if (node.left != null)
            {
                for (int i = 1; i <= node.edgeLength && y + i < MAX_HEIGHT; i++) {
                    m_lProfile[y + i] = Math.Min(m_lProfile[y + i], x - i);
                }
            }

            ComputeLProfile(node.left, x - node.edgeLength - 1, y + node.edgeLength + 1);
            ComputeLProfile(node.right, x + node.edgeLength + 1, y + node.edgeLength + 1);
        }

        void ComputeRProfile(AsciiNode node, int x, int y)
        {
            if (node == null) {
                return;
            }

            int notleft = (node.parentDir != -1) ? 1 : 0;
            m_rProfile[y] = Math.Max(m_rProfile[y], x + ((node.label.Length - notleft) / 2));
            if (node.right != null)
            {
                for (int i = 1; i <= node.edgeLength && y + i < MAX_HEIGHT; i++) {
                    m_rProfile[y + i] = Math.Max(m_rProfile[y + i], x + i);
                }
            }

            ComputeRProfile(node.left, x - node.edgeLength - 1, y + node.edgeLength + 1);
            ComputeRProfile(node.right, x + node.edgeLength + 1, y + node.edgeLength + 1);
        }

        //This function fills in the edgeLength and 
        //height fields of the specified tree
        void ComputeEdgeLengths(AsciiNode node)
        {
            if (node == null) {
                return;
            }

            ComputeEdgeLengths(node.left);
            ComputeEdgeLengths(node.right);

            /* first fill in the edgeLength of node */
            if (node.right == null && node.left == null)
            {
                node.edgeLength = 0;
            }
            else
            {
                int hmin = 0;
                if (node.left != null)
                {
                    for (int i = 0; i<node.left.height && i < MAX_HEIGHT; i++) {
                        m_rProfile[i] = -INFINITY_NUM;
                    }
                    ComputeRProfile(node.left, 0, 0);
                    hmin = node.left.height;
                }
                else
                {
                    hmin = 0;
                }
                if (node.right != null)
                {
                    for (int i = 0; i<node.right.height && i < MAX_HEIGHT; i++) {
                        m_lProfile[i] = INFINITY_NUM;
                    }
                    ComputeLProfile(node.right, 0, 0);
                    hmin = Math.Min(node.right.height, hmin);
                }
                else
                {
                    hmin = 0;
                }

                int delta = 4;
                for (int i = 0; i<hmin; i++) {
                    delta = Math.Max(delta, GAP + 1 + m_rProfile[i] - m_lProfile[i]);
                }

                //If the node has two children of height 1, then we allow the
                //two leaves to be within 1, instead of 2 
                if (((node.left != null && node.left.height == 1) ||
                    (node.right != null && node.right.height == 1)) && delta>4)
                {
                    delta--;
                }

                node.edgeLength = ((delta + 1) / 2) - 1;
            }

            //now fill in the height of node
            int h = 1;
            if (node.left != null)
            {
                h = Math.Max(node.left.height + node.edgeLength + 1, h);
            }
            if (node.right != null)
            {
                h = Math.Max(node.right.height + node.edgeLength + 1, h);
            }
            node.height = h;
        }

        //This function prints the given level of the given tree, assuming
        //that the node has the given x cordinate.
        void PrintLevel(AsciiNode node, int x, int level)
        {
            if (node == null) {
                return;
            }

            int isleft = (node.parentDir == -1) ? 1 : 0;
            if (level == 0)
            {
                int i = 0;
                for (; i<(x - m_printNext - ((node.label.Length - isleft) / 2)); i++) {
                    Console.Write(" ");
                }
                m_printNext += i;
                Console.Write(node.label);
                m_printNext += node.label.Length;
            }
            else if (node.edgeLength >= level)
            {
                if (node.left != null)
                {
                    int i = 0;
                    for (; i<(x - m_printNext - (level)); i++) {
                        Console.Write(" ");
                    }
                    m_printNext += i;
                    Console.Write("/");
                    m_printNext++;
                }
                if (node.right != null)
                {
                    int i = 0;
                    for (; i<(x - m_printNext + (level)); i++) {
                        Console.Write(" ");
                    }
                    m_printNext += i;
                    Console.Write("\\");
                    m_printNext++;
                }
            }
            else
            {
                PrintLevel(node.left,
                    x - node.edgeLength - 1,
                    level - node.edgeLength - 1);
                PrintLevel(node.right,
                    x + node.edgeLength + 1,
                    level - node.edgeLength - 1);
            }
        }
    }
}