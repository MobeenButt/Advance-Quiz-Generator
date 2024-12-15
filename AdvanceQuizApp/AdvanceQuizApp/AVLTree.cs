using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;

public class AVLTree<T> where T : IComparable<T>
    {
    public class Node
        {
        public T Key { get; set; }
        public List<object> Data { get; set; } = new List<object>();
        public int Height { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }

        public Node(T key)
            {
            Key = key;
            }
        }

    public Node Root { get; private set; }

    // Insert function with modifications
    public void Insert(T key, object data)
        {
        Root = Insert(Root, key, data);
        }

    private Node Insert(Node node, T key, object data)
        {
        if (node == null)
            return new Node(key) { Data = { data } };

        int compare = key.CompareTo(node.Key);

        if (compare < 0)
            node.Left = Insert(node.Left, key, data);
        else if (compare > 0)
            node.Right = Insert(node.Right, key, data);
        else
            node.Data.Add(data); // Add data to the same key

        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        return Balance(node);
        }

    // Find function
    public Node Find(T key)
        {
        return Find(Root, key);
        }

    private Node Find(Node node, T key)
        {
        if (node == null) return null;

        int compare = key.CompareTo(node.Key);
        if (compare < 0)
            return Find(node.Left, key);
        else if (compare > 0)
            return Find(node.Right, key);
        else
            return node;
        }

    // Balancing functions
    private Node Balance(Node node)
        {
        int balance = GetBalance(node);

        if (balance > 1)
            {
            if (GetBalance(node.Left) < 0)
                node.Left = RotateLeft(node.Left);
            return RotateRight(node);
            }

        if (balance < -1)
            {
            if (GetBalance(node.Right) > 0)
                node.Right = RotateRight(node.Right);
            return RotateLeft(node);
            }

        return node;
        }

    private Node RotateLeft(Node node)
        {
        Node newRoot = node.Right;
        node.Right = newRoot.Left;
        newRoot.Left = node;

        UpdateHeight(node);
        UpdateHeight(newRoot);

        return newRoot;
        }

    private Node RotateRight(Node node)
        {
        Node newRoot = node.Left;
        node.Left = newRoot.Right;
        newRoot.Right = node;

        UpdateHeight(node);
        UpdateHeight(newRoot);

        return newRoot;
        }

    private void UpdateHeight(Node node)
        {
        node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));
        }

    private int GetHeight(Node node)
        {
        return node?.Height ?? 0;
        }

    private int GetBalance(Node node)
        {
        return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

    // Traversal functions
    public List<T> InOrderTraversal()
        {
        List<T> result = new List<T>();
        InOrderTraversal(Root, result);
        return result;
        }

    private void InOrderTraversal(Node node, List<T> result)
        {
        if (node == null) return;
        InOrderTraversal(node.Left, result);
        result.Add(node.Key);
        InOrderTraversal(node.Right, result);
        }

    public void Traverse(Action<Node> action)
        {
        TraverseInOrder(Root, action);
        }

    private void TraverseInOrder(Node node, Action<Node> action)
        {
        if (node == null) return;
        TraverseInOrder(node.Left, action);
        action(node);
        TraverseInOrder(node.Right, action);
        }

    // Serialization to JSON file
    public void SaveToJson(string filePath)
        {
        var options = new JsonSerializerOptions
            {
            WriteIndented = true
            };

        string json = JsonSerializer.Serialize(Root, options);
        File.WriteAllText(filePath, json);
        }

    // Deserialization from JSON file
    public void LoadFromJson(string filePath)
        {
        if (!File.Exists(filePath))
            throw new FileNotFoundException("The specified file does not exist.");

        string json = File.ReadAllText(filePath);

        var options = new JsonSerializerOptions
            {
            WriteIndented = true
            };

        Root = JsonSerializer.Deserialize<Node>(json, options);
        }

    // Utility functions
    public bool IsEmpty()
        {
        return Root == null;
        }

    public void Clear()
        {
        Root = null;
        }

    public int Size()
        {
        return GetSize(Root);
        }

    private int GetSize(Node node)
        {
        if (node == null) return 0;
        return 1 + GetSize(node.Left) + GetSize(node.Right);
        }

    public Node GetMinValueNode()
        {
        return GetMinValueNode(Root);
        }

    private Node GetMinValueNode(Node node)
        {
        while (node?.Left != null)
            node = node.Left;
        return node;
        }

    public Node GetMaxValueNode()
        {
        return GetMaxValueNode(Root);
        }

    private Node GetMaxValueNode(Node node)
        {
        while (node?.Right != null)
            node = node.Right;
        return node;
        }
    }
