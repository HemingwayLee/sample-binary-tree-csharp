using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiBinaryTree
{
    class Program
    {
        static TreeNode Find(int elem, TreeNode t)
        {
            if (t == null)
            {
                return null;
            }

            if (elem < t.val)
            {
                return Find(elem, t.left);
            }
            else if (elem > t.val)
            {
                return Find(elem, t.right);
            }
            else
            {
                return t;
            }
        }

        static TreeNode FindMin(TreeNode t)
        {
            if (t == null)
            {
                return null;
            }
            else if (t.left == null)
            {
                return t;
            }
            else
            {
                return FindMin(t.left);
            }
        }


        //Insert i into the tree t, duplicate will be discarded
        //Return a pointer to the resulting tree.                 
        static TreeNode Insert(int value, TreeNode t)
        {
            if (t == null)
            {
                TreeNode new_node = new TreeNode(value);
                return new_node;
            }

            if (value < t.val)
            {
                t.left = Insert(value, t.left);
            }
            else if (value > t.val)
            {
                t.right = Insert(value, t.right);
            }
            else //duplicate, ignore it
            {
                return t;
            }

            return t;
        }

        //Deletes node from the tree
        // Return a pointer to the resulting tree
        static TreeNode Remove(int value, TreeNode t)
        {
            if (t == null)
            {
                return null;
            }

            TreeNode tmp_cell = null;
            if (value < t.val)
            {
                t.left = Remove(value, t.left);
            }
            else if (value > t.val)
            {
                t.right = Remove(value, t.right);
            }
            else if (t.left != null && t.right != null)
            {
                tmp_cell = FindMin(t.right);
                t.val = tmp_cell.val;
                t.right = Remove(t.val, t.right);
            }
            else
            {
                tmp_cell = t;
                if (t.left == null)
                    t = t.right;
                else if (t.right == null)
                    t = t.left;
            }

            return t;
        }

        static void Main(string[] args)
        {
            TreeNode root = null;

            AsciiPrinter ascii = new AsciiPrinter();

            Console.Write("\nAfter inserting val 10..\n");
            root = Insert(10, root);
            ascii.PrintAsciiTree(root);

            Console.Write("\nAfter inserting val 5..\n");
            root = Insert(5, root);
            ascii.PrintAsciiTree(root);

            Console.Write("\nAfter inserting val 15..\n");
            root = Insert(15, root);
            ascii.PrintAsciiTree(root);

            Console.Write("\nAfter inserting vals 9, 13..\n");
            root = Insert(9, root);
            root = Insert(13, root);
            ascii.PrintAsciiTree(root);

            Console.Write("\nAfter inserting vals 2, 6, 12, 14, ..\n");
            root = Insert(2, root);
            root = Insert(6, root);
            root = Insert(12, root);
            root = Insert(14, root);
            ascii.PrintAsciiTree(root);

            Console.Write("\n\nAfter deleting a node (14) with no child..\n");
            root = Remove(14, root);
            ascii.PrintAsciiTree(root);

            Console.Write("\n\nAfter deleting a node (13) with left child..\n");
            root = Remove(13, root);
            ascii.PrintAsciiTree(root);

            Console.Write("\n\nAfter deleting a node (5) with left and right children..\n");
            root = Remove(5, root);
            ascii.PrintAsciiTree(root);

            Console.Write("\n\nAfter deleting a node (10, root node) with left and right children..\n");
            root = Remove(10, root);
            ascii.PrintAsciiTree(root);
        }
    }
}
