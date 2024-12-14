using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AdvanceQuizApp.ADT;
namespace AdvanceQuizApp.Map
{
    public class Node
    {
        public string Name { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<Edge> Edges { get; set; }
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
            }

            // Draw nodes as rectangles (representing buildings)
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
                Canvas.SetLeft(rect, node.Longitude - 20); // Adjust to center the rectangle
                Canvas.SetTop(rect, node.Latitude - 20);
                canvas.Children.Add(rect);

                // Label for node
                var label = new TextBlock
                {
                    Text = node.Name,
                    Foreground = Brushes.Black,
                    FontSize = 12
                };
                Canvas.SetLeft(label, node.Longitude + 25); // Adjust label positioning
                Canvas.SetTop(label, node.Latitude - 10);
                canvas.Children.Add(label);
            }
        }

        public List<Node> FindShortestPath(Node startNode, Node endNode)
        {
            var distances = new Dictionary<Node, double>();
            var previousNodes = new Dictionary<Node, Node>();
            var nodes = new List<Node>(Nodes);

            foreach (var node in nodes)
            {
                distances[node] = double.MaxValue;
            }
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

            return new List<Node>(); // No path found
        }
    }
}
