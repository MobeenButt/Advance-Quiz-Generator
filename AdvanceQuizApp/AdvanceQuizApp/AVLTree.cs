using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvanceQuizApp
{
    public class AVLTree<T> where T : IComparable<T>
    {
        public class Node
        {
            public T Key { get; set; }
            public List<object> Data { get; set; } // Store quizzes or user data
            public int Height { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }

            public Node(T key, object data)
            {
                Key = key;
                Data = new List<object> { data };
                Height = 1;
            }
        }

        public Node Root { get; private set; }

        private int Height(Node node) => node?.Height ?? 0;

        private int BalanceFactor(Node node) => Height(node.Left) - Height(node.Right);

        private Node RotateRight(Node y)
        {
            Node x = y.Left;
            Node T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;

            return x;
        }

        private Node RotateLeft(Node x)
        {
            Node y = x.Right;
            Node T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;

            return y;
        }

        public void Insert(T key, object data)
        {
            Root = Insert(Root, key, data);
        }

        private Node Insert(Node node, T key, object data)
        {
            if (node == null)
                return new Node(key, data);

            int compare = key.CompareTo(node.Key);

            if (compare < 0)
                node.Left = Insert(node.Left, key, data);
            else if (compare > 0)
                node.Right = Insert(node.Right, key, data);
            else
                node.Data.Add(data); // Add data for the same key

            node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));

            int balance = BalanceFactor(node);

            // Left-Left Case
            if (balance > 1 && key.CompareTo(node.Left.Key) < 0)
                return RotateRight(node);

            // Right-Right Case
            if (balance < -1 && key.CompareTo(node.Right.Key) > 0)
                return RotateLeft(node);

            // Left-Right Case
            if (balance > 1 && key.CompareTo(node.Left.Key) > 0)
            {
                node.Left = RotateLeft(node.Left);
                return RotateRight(node);
            }

            // Right-Left Case
            if (balance < -1 && key.CompareTo(node.Right.Key) < 0)
            {
                node.Right = RotateRight(node.Right);
                return RotateLeft(node);
            }

            return node;
        }

        public void Traverse(Action<Node> action)
        {
            void InOrder(Node node)
            {
                if (node == null) return;
                InOrder(node.Left);
                action(node);
                InOrder(node.Right);
            }

            InOrder(Root);
        }
    }
}    
