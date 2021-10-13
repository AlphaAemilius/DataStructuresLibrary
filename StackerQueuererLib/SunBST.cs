using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackerQueuererLib
{
    public class SunBST<T> where T : IComparable
    {
        public class Node
        {
            public Node Parent { get; set; }
            public T Data { get; set; }
            public Node LeftN { get; set; }
            public Node RightN { get; set; }

            public Node(T t)
            {
                LeftN = null;
                RightN = null;
                Parent = null;
                Data = t;
            }
            public Node() { }
        }

        public Node Root { get; set; }
        public int Size { get; set; }

        public SunBST(T t)
        {
            Root = new Node(t);
        }

        private void InOrderTraversal(Node root)
        {
            if (root != null)
            {

            }
        }


        /// <summary>
        /// Returns the height of a given node within the tree
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public int GetHeight(Node n)
        {
            int height = 1;
            Node current = n;
            while (n.Parent != null)
            {
                height++;
                current = current.Parent;
            }
            return height;
        }

        /// <summary>
        /// Traverses the tree to find if given data is present.
        /// </summary>
        /// <param name="data"></param>
        /// <returns> True if found, false if otherwise</returns>
        public bool Contains(T data)
        {
            Node n = Root;
            while (n != null)
            {
                int result = data.CompareTo(n.Data);
                if (result == 0)
                {
                    return true;
                }
                n = result < 0 ? n.LeftN : n.RightN;

            }

            return false;
        }

        /// <summary>
        /// Adds new item of type T to the BST 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="useRecursion"></param>
        public void Add(T data, bool useRecursion)
        {
            if (Root == null)  //if size=0 => add root and return
            {
                Root = new Node(data);
                return;
            }

            if (useRecursion)
            {
                AddRecursively(Root, data);
            }
            else
            {
                AddIteratively(data);
            }
            Size++;

        }

        private Node AddRecursively(Node rootN, T data)
        {

            if (rootN == null) { rootN = new Node(data); }


            if (rootN.Data.CompareTo(data) < 0)
            {
                rootN.RightN = AddRecursively(rootN.RightN, data);
            }
            else
            {
                rootN.LeftN = AddRecursively(rootN.LeftN, data);
            }
            return rootN;
        }

        /// <summary>
        /// Finds new parent node iteratively and adds new node to left or right
        /// </summary>
        /// <param name="data"></param>
        public void AddIteratively(T data)
        {
            Node newParent = FindNewParent(data);
            Node n = new Node() { Data = data, Parent = newParent };  //creates new node

            if (data.CompareTo(newParent.Data) < 0)
            {
                newParent.LeftN = n;
            }
            else
            {
                newParent.RightN = n;
            }
        }

        /// <summary>
        /// Finds the new parent node of the child data to be added
        /// </summary>
        /// <param name="data"></param>
        /// <returns>The new parent node</returns>
        public Node FindNewParent(T data)
        {
            Node current = Root;    //iterating node
            Node parent = null;
            while (current != null)  //while there is still nodes
            {
                parent = current;
                int result = data.CompareTo(current.Data);
                if (result == 0) //found node
                {
                    throw new ArgumentException($"The node {data} already exists");
                }

                current = result < 0 ? current.LeftN : current.RightN;
            }

            return parent; //returns last node

        }

        /// <summary>
        /// Finds a node iteratively based on the node's data
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Node containing data </returns>
        public Node Find(T data)
        {
            Node current = Root;    //iterating node
            while (current != null)  //while there is still nodes
            {

                int result = data.CompareTo(current.Data);
                if (result == 0) //found node
                {
                    return current;
                }
                current = result < 0 ? current.LeftN : current.RightN;
            }

            return null; //nothing found

        }


        public void Remove(T data)
        {
            Remove(Root, data);
        }

        private void Remove(Node node, T data)
        {
            if (node == null)
            {
                throw new ArgumentException($"The node {data} does not exist");
            }
            else if (data.CompareTo(node.Data) < 0)
            {
                Remove(node.LeftN, data);
            }
            else if (data.CompareTo(node.Data) > 0)
            {
                Remove(node.RightN, data);
            }
            else  //found node to remove
            {
                if (node.LeftN == null && node.RightN == null)   //if node has no children
                {
                    ReplaceInParent(node, null);   // null because we do not want to replace the leaf with anything
                    Size--;
                }
                else if (node.RightN == null)   //has left child
                {
                    ReplaceInParent(node, node.LeftN);
                }
                else if (node.LeftN == null)   //has right child
                {
                    ReplaceInParent(node, node.RightN);
                }
                else
                {
                    Node successor = FindMinimumInSubtree(node.RightN);
                    node.Data = successor.Data;
                    Remove(successor, successor.Data);
                }
            }
        }


        /// <summary>
        /// Takes node for removal and node to replace it in its parent node
        /// </summary>
        /// <param name="node">Node for removal</param>
        /// <param name="newNode"></param>
        private void ReplaceInParent(Node node, Node newNode)
        {
            if (node.Parent != null)
            {
                if (node.Parent.LeftN == node) // if node is the left child
                {
                    node.Parent.LeftN = newNode;    //replace node with newNode
                }
                else  // if node is the right child
                {
                    node.Parent.RightN = newNode;
                }
            }
            else   // if only 1 element
            {
                Root = newNode;
            }

            if (newNode != null) //?
            {
                newNode.Parent = node.Parent;
            }
        }
        private Node FindMinimumInSubtree(Node node)
        {
            while (node.LeftN != null)
            {
                node = node.LeftN;
            }

            return node;
        }

    }
}
