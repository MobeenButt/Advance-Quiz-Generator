using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AdvanceQuizApp.Map
{
    public class Node
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class Edge
    {
        public Node node1 { get; set; }
        public Node node2 { get; set; }
        public double Weight { get; set; }
    }

    public class Graph
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public void display(Canvas canvas)
        {
            canvas.Children.Clear();

            // Draw edges
            foreach (var edge in Edges)
            {
                var line = new Line
                {
                    X1 = edge.node1.Longitude,
                    Y1 = edge.node1.Latitude,
                    X2 = edge.node2.Longitude,
                    Y2 = edge.node2.Latitude,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                canvas.Children.Add(line);

                // Display edge weight
                var label = new TextBlock
                {
                    Text = edge.Weight.ToString(),
                    Foreground = Brushes.Red,
                    FontSize = 12
                };
                Canvas.SetLeft(label, (edge.node1.Longitude + edge.node2.Longitude) / 2);
                Canvas.SetTop(label, (edge.node1.Latitude + edge.node2.Latitude) / 2);
                canvas.Children.Add(label);
            }

            // Draw nodes
            foreach (var node in Nodes)
            {
                var rect = new Rectangle
                {
                    Width = 40,
                    Height = 40,
                    Fill = Brushes.Blue,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                Canvas.SetLeft(rect, node.Longitude - 20);
                Canvas.SetTop(rect, node.Latitude - 20);
                canvas.Children.Add(rect);

                // Label for the node
                var label = new TextBlock
                {
                    Text = node.Name,
                    Foreground = Brushes.Black,
                    FontSize = 12
                };
                Canvas.SetLeft(label, node.Longitude + 25);
                Canvas.SetTop(label, node.Latitude - 10);
                canvas.Children.Add(label);
            }
        }

        public List<Node> FindShortestPathDijkstra(Node startNode, Node endNode)
        {
            var distances = new Dictionary<Node, double>();
            var previousNodes = new Dictionary<Node, Node>();
            var nodes = new List<Node>(Nodes);

            foreach (var node in nodes)
                distances[node] = double.MaxValue;

            distances[startNode] = 0;

            while (nodes.Count > 0)
            {
                nodes.Sort((x, y) => distances[x].CompareTo(distances[y]));
                var currentNode = nodes[0];
                nodes.Remove(currentNode);

                if (currentNode == endNode)
                {
                    var path = new List<Node>();
                    while (previousNodes.ContainsKey(currentNode))
                    {
                        path.Insert(0, currentNode);
                        currentNode = previousNodes[currentNode];
                    }
                    path.Insert(0, startNode);
                    return path;
                }

                foreach (var edge in Edges.Where(e => e.node1 == currentNode || e.node2 == currentNode))
                {
                    var neighbor = edge.node1 == currentNode ? edge.node2 : edge.node1;
                    var alt = distances[currentNode] + edge.Weight;

                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previousNodes[neighbor] = currentNode;
                    }
                }
            }

            return new List<Node>();
        }

        public List<Node> FindPathBFS(Node startNode, Node endNode)
        {
            var visited = new HashSet<Node>();
            var queue = new Queue<List<Node>>();
            queue.Enqueue(new List<Node> { startNode });

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                var currentNode = path.Last();

                if (visited.Contains(currentNode))
                    continue;

                if (currentNode == endNode)
                    return path;

                visited.Add(currentNode);

                foreach (var edge in Edges.Where(e => e.node1 == currentNode || e.node2 == currentNode))
                {
                    var neighbor = edge.node1 == currentNode ? edge.node2 : edge.node1;
                    if (!visited.Contains(neighbor))
                    {
                        var newPath = new List<Node>(path) { neighbor };
                        queue.Enqueue(newPath);
                    }
                }
            }

            return new List<Node>();
        }

        public List<Node> FindPathDFS(Node startNode, Node endNode)
        {
            var visited = new HashSet<Node>();
            var stack = new Stack<List<Node>>();
            stack.Push(new List<Node> { startNode });

            while (stack.Count > 0)
            {
                var path = stack.Pop();
                var currentNode = path.Last();

                if (visited.Contains(currentNode))
                    continue;

                if (currentNode == endNode)
                    return path;

                visited.Add(currentNode);

                foreach (var edge in Edges.Where(e => e.node1 == currentNode || e.node2 == currentNode))
                {
                    var neighbor = edge.node1 == currentNode ? edge.node2 : edge.node1;
                    if (!visited.Contains(neighbor))
                    {
                        var newPath = new List<Node>(path) { neighbor };
                        stack.Push(newPath);
                    }
                }
            }

            return new List<Node>();
        }
        public double GetEdgeWeight(Node node1, Node node2)
        {
            // Find the edge connecting node1 and node2
            var edge = Edges.FirstOrDefault(e =>
                (e.node1 == node1 && e.node2 == node2) ||
                (e.node1 == node2 && e.node2 == node1));

            // Return the weight if edge exists, otherwise return -1 (or any default value indicating no edge)
            return edge != null ? edge.Weight : -1;
        }


    }
}
