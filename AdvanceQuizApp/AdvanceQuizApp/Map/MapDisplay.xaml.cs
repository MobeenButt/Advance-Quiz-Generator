using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AdvanceQuizApp.ADT;
using System.Linq;

namespace AdvanceQuizApp.Map
{
    public partial class MapDisplay : Window
    {
        private Graph graph;
        private MyStack<List<UIElement>> statestack;

        public MapDisplay()
        {
            InitializeComponent();
            statestack = new MyStack<List<UIElement>>();
            initializeGraph();
            EndLocation.IsReadOnly = true;
            EndLocation.Text = "Quiz Center";
        }

        private void initializeGraph()
        {
            var nodes = new[]
            {
                new Node { Name = "Sialkot", Latitude = 300, Longitude = 400 },
                new Node { Name = "Karachi", Latitude = 700, Longitude = 200 },
                new Node { Name = "Lahore", Latitude = 500, Longitude = 500 },
                new Node { Name = "Islamabad", Latitude = 600, Longitude = 700 },
                new Node { Name = "Peshawar", Latitude = 800, Longitude = 300 },
                new Node { Name = "Quetta", Latitude = 900, Longitude = 100 },
                new Node { Name = "Rawalpindi", Latitude = 400, Longitude = 600 },
                new Node { Name = "Multan", Latitude = 200, Longitude = 800 },
                new Node { Name = "Faisalabad", Latitude = 300, Longitude = 900 },
                new Node { Name = "Quiz Center", Latitude = 500, Longitude = 800 },
                new Node { Name = "Murree", Latitude = 100, Longitude = 100 },
                new Node { Name = "Gwadar", Latitude = 100, Longitude = 500 }
            };

            graph = new Graph();
            graph.Nodes.AddRange(nodes);

            graph.Edges.Add(new Edge { node1 = nodes[0], node2 = nodes[1], Weight = 200 });
            graph.Edges.Add(new Edge { node1 = nodes[1], node2 = nodes[2], Weight = 200 });
            graph.Edges.Add(new Edge { node1 = nodes[0], node2 = nodes[4], Weight = 200 });
            graph.Edges.Add(new Edge { node1 = nodes[4], node2 = nodes[3], Weight = 200 });
            graph.Edges.Add(new Edge { node1 = nodes[1], node2 = nodes[5], Weight = 300 });
            graph.Edges.Add(new Edge { node1 = nodes[5], node2 = nodes[6], Weight = 300 });
            graph.Edges.Add(new Edge { node1 = nodes[3], node2 = nodes[8], Weight = 150 });
            graph.Edges.Add(new Edge { node1 = nodes[4], node2 = nodes[7], Weight = 250 });
            graph.Edges.Add(new Edge { node1 = nodes[7], node2 = nodes[8], Weight = 150 });
            graph.Edges.Add(new Edge { node1 = nodes[8], node2 = nodes[9], Weight = 200 });
            graph.Edges.Add(new Edge { node1 = nodes[5], node2 = nodes[9], Weight = 250 });
            graph.Edges.Add(new Edge { node1 = nodes[6], node2 = nodes[10], Weight = 150 });
            graph.Edges.Add(new Edge { node1 = nodes[10], node2 = nodes[11], Weight = 200 });
            graph.Edges.Add(new Edge { node1 = nodes[9], node2 = nodes[10], Weight = 200 });

            graph.display(MapPanel);

            savecurrentstate();
        }

        private void savecurrentstate()
        {
            var curr = MapPanel.Children.Cast<UIElement>().ToList();
            statestack.Push(new List<UIElement>(curr));
        }

        private void FindPathButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedAlgorithm = (AlgorithmSelector.SelectedItem as ComboBoxItem)?.Content.ToString();
            var startNode = graph.Nodes.FirstOrDefault(n => n.Name.Equals(StartLocation.Text, StringComparison.OrdinalIgnoreCase));
            var endNode = graph.Nodes.FirstOrDefault(n => n.Name.Equals("Quiz Center", StringComparison.OrdinalIgnoreCase));

            if (startNode == null || endNode == null)
            {
                MessageBox.Show("Invalid start or end location.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<Node> path = null;

            switch (selectedAlgorithm)
            {
                case "Dijkstra":
                    path = graph.FindShortestPathDijkstra(startNode, endNode);
                    break;
                case "BFS":
                    path = graph.FindPathBFS(startNode, endNode);
                    break;
                case "DFS":
                    path = graph.FindPathDFS(startNode, endNode);
                    break;
                default:
                    MessageBox.Show("Please select a valid algorithm.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
            }

            DisplayPath(path, selectedAlgorithm);
        }

        private void DisplayPath(List<Node> path, string algorithm)
        {
            if (path.Count == 0)
            {
                MessageBox.Show("No path found.");
                return;
            }

            graph.display(MapPanel);  // Clear previous paths

            for (int i = 0; i < path.Count - 1; i++)
            {
                var currentNode = path[i];
                var nextNode = path[i + 1];

                var edgeLine = new Line
                {
                    X1 = currentNode.Longitude,
                    Y1 = currentNode.Latitude,
                    X2 = nextNode.Longitude,
                    Y2 = nextNode.Latitude,
                    Stroke = GetPathColor(algorithm), // Algorithm-specific path color
                    StrokeThickness = 4
                };

                MapPanel.Children.Add(edgeLine);
                AnimateLineDrawing(edgeLine);

                var distanceText = new TextBlock
                {
                    Text = $"{graph.GetEdgeWeight(currentNode, nextNode)}",
                    Foreground = GetPathColor(algorithm),
                    Background = Brushes.White,
                    FontSize = 14
                };
                Canvas.SetLeft(distanceText, (currentNode.Longitude + nextNode.Longitude) / 2);
                Canvas.SetTop(distanceText, (currentNode.Latitude + nextNode.Latitude) / 2);
                MapPanel.Children.Add(distanceText);
            }

            foreach (var node in path)
            {
                var rect = new Rectangle
                {
                    Width = 40,
                    Height = 40,
                    Fill = Brushes.Green,
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                Canvas.SetLeft(rect, node.Longitude - 20);
                Canvas.SetTop(rect, node.Latitude - 20);
                MapPanel.Children.Add(rect);

                var label = new TextBlock
                {
                    Text = node.Name,
                    Foreground = Brushes.Black,
                    FontSize = 12
                };
                Canvas.SetLeft(label, node.Longitude + 25);
                Canvas.SetTop(label, node.Latitude - 10);
                MapPanel.Children.Add(label);
            }
        }

        private Brush GetPathColor(string algorithm)
        {
            switch (algorithm)
            {
                case "Dijkstra":
                    return Brushes.Blue;  // Dijkstra paths in blue
                case "BFS":
                    return Brushes.Green; // BFS paths in green
                case "DFS":
                    return Brushes.Red;   // DFS paths in red
                default:
                    return Brushes.Gray;  // Default path color
            }
        }

        private void AnimateLineDrawing(Line line)
        {
            var animation = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(2),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd
            };

            var storyboard = new System.Windows.Media.Animation.Storyboard();
            System.Windows.Media.Animation.Storyboard.SetTarget(animation, line);
            System.Windows.Media.Animation.Storyboard.SetTargetProperty(animation, new PropertyPath(Line.StrokeDashOffsetProperty));
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (statestack.Count() > 0)
            {
                MapPanel.Children.Clear();
                var prevState = statestack.Pop();
                foreach (var elem in prevState)
                {
                    MapPanel.Children.Add(elem);
                }
            }
            else
            {
                MessageBox.Show("No previous state found.");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminPanel admin = new AdminPanel();
            admin.Show();
        }
    }
}
