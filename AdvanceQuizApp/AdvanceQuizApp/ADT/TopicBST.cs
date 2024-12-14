using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdvanceQuizApp.ADT
{
    public class TopicBST
    {
        private class Node
        {
            public string Topic;
            public Node Left, Right;

            public Node(string topic)
            {
                Topic = topic;
                Left = Right = null;
            }
        }

        private Node root;

        public TopicBST()
        {
            root = null;
        }

        // Insert a topic into the BST
        public void Insert(string topic)
        {
            root = InsertRec(root, topic);
        }

        private Node InsertRec(Node node, string topic)
        {
            if (node == null)
                return new Node(topic);

            if (string.Compare(topic, node.Topic) < 0)
                node.Left = InsertRec(node.Left, topic);
            else if (string.Compare(topic, node.Topic) > 0)
                node.Right = InsertRec(node.Right, topic);

            return node;
        }

        // Search for a topic in the BST
        public bool Search(string topic)
        {
            return SearchRec(root, topic);
        }

        private bool SearchRec(Node node, string topic)
        {
            if (node == null)
                return false;

            if (node.Topic == topic)
                return true;

            if (string.Compare(topic, node.Topic) < 0)
                return SearchRec(node.Left, topic);

            return SearchRec(node.Right, topic);
        }

        // In-order traversal to get sorted topics
        public List<string> GetSortedTopics()
        {
            List<string> topics = new List<string>();
            InOrderRec(root, topics);
            return topics;
        }

        private void InOrderRec(Node node, List<string> topics)
        {
            if (node == null)
                return;

            InOrderRec(node.Left, topics);
            topics.Add(node.Topic);
            InOrderRec(node.Right, topics);
        }
    }
}
