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

    public void Insert(string topic)
    {
        root = InsertRec(root, topic);
    }

    private Node InsertRec(Node node, string topic)
    {
        if (node == null)
            return new Node(topic);

        if (string.Compare(topic, node.Topic, StringComparison.OrdinalIgnoreCase) < 0)
            node.Left = InsertRec(node.Left, topic);
        else if (string.Compare(topic, node.Topic, StringComparison.OrdinalIgnoreCase) > 0)
            node.Right = InsertRec(node.Right, topic);

        return node;
    }

    public bool Search(string topic)
    {
        return SearchRec(root, topic);
    }

    private bool SearchRec(Node node, string topic)
    {
        if (node == null)
            return false;

        if (string.Equals(node.Topic, topic, StringComparison.OrdinalIgnoreCase))
            return true;

        if (string.Compare(topic, node.Topic, StringComparison.OrdinalIgnoreCase) < 0)
            return SearchRec(node.Left, topic);

        return SearchRec(node.Right, topic);
    }

    public List<string> FindClosestMatches(string query)
    {
        List<string> allTopics = GetSortedTopics();
        List<string> matches = new List<string>();

        string lowerQuery = query.ToLower();

        
        matches.AddRange(allTopics.Where(topic => topic.ToLower().Contains(lowerQuery)));

        //sb se karerb kareeb waly topics find kary ga jo characters hoy topic me
        if (matches.Count == 0)
        {
            foreach (var topic in allTopics)
            {
                if (string.Compare(topic.ToLower(), lowerQuery) > 0)
                {
                    matches.Add(topic);
                    break;
                }
            }

            for (int i = allTopics.Count - 1; i >= 0; i--)
            {
                if (string.Compare(allTopics[i].ToLower(), lowerQuery) < 0)
                {
                    matches.Insert(0, allTopics[i]);
                    break;
                }
            }
        }

        return matches;
    }
    public List<string> GetSortedTopics()  //inoder kiya hai
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
