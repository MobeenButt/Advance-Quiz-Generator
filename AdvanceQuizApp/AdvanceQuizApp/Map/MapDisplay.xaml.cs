using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;
using AdvanceQuizApp.ADT;

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
        }

        private void initializeGraph()
        {
            var node1 = new Node { Name = "A", Latitude = 100, Longitude = 100 };
            var node2 = new Node { Name = "B", Latitude = 300, Longitude = 100 };
            var node3 = new Node { Name = "C", Latitude = 500, Longitude = 100 };
            var node4 = new Node { Name = "D", Latitude = 300, Longitude = 300 };
            var node5 = new Node { Name = "E", Latitude = 100, Longitude = 300 };
            var node6 = new Node { Name = "F", Latitude = 500, Longitude = 300 };
            var node7 = new Node { Name = "G", Latitude = 700, Longitude = 200 };

            graph = new Graph();
            graph.Nodes.AddRange(new[] { node1, node2, node3, node4, node5, node6, node7 });

            graph.Edges.Add(new Edge { node1 = node1, node2 = node2, Weight = 200 });
            graph.Edges.Add(new Edge { node1 = node2, node2 = node3, Weight = 200 });
            graph.Edges.Add(new Edge { node1 = node1, node2 = node5, Weight = 200 });
            graph.Edges.Add(new Edge { node1 = node5, node2 = node4, Weight = 200 });
            graph.Edges.Add(new Edge { node1 = node2, node2 = node6, Weight = 300 });
            graph.Edges.Add(new Edge { node1 = node6, node2 = node7, Weight = 300 });

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
            var startNode = graph.Nodes.FirstOrDefault(n => n.Name == StartLocation.Text);
            var endNode = graph.Nodes.FirstOrDefault(n => n.Name == EndLocation.Text);

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

            DisplayPath(path);
        }


        private void DisplayPath(List<Node> path)
        {
            if (path.Count == 0)
            {
                MessageBox.Show("No path found.");
                return;
            }

            // Reset the map to the default state
            graph.display(MapPanel);

            // Highlight nodes and edges along the path
            for (int i = 0; i < path.Count - 1; i++)
            {
                var currentNode = path[i];
                var nextNode = path[i + 1];

                // Highlight the edge in a distinct color
                var edgeLine = new Line
                {
                    X1 = currentNode.Longitude,
                    Y1 = currentNode.Latitude,
                    X2 = nextNode.Longitude,
                    Y2 = nextNode.Latitude,
                    Stroke = Brushes.Blue, // Path edge color
                    StrokeThickness = 4
                };
                MapPanel.Children.Add(edgeLine);
                AnimateLineDrawing(edgeLine);

                var distanceText = new TextBlock
                {
                    Text = $"{graph.GetEdgeWeight(currentNode, nextNode)}",
                    Foreground = Brushes.Blue,
                    Background = Brushes.White,
                    FontSize = 14
                };
                Canvas.SetLeft(distanceText, (currentNode.Longitude + nextNode.Longitude) / 2);
                Canvas.SetTop(distanceText, (currentNode.Latitude + nextNode.Latitude) / 2);
                MapPanel.Children.Add(distanceText);
            }

            // Highlight the nodes along the path
            foreach (var node in path)
            {
                var rect = new Rectangle
                {
                    Width = 40,
                    Height = 40,
                    Fill = Brushes.Green, // Highlighted node color
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                Canvas.SetLeft(rect, node.Longitude - 20);
                Canvas.SetTop(rect, node.Latitude - 20);
                MapPanel.Children.Add(rect);

                // Add label for the node
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

        private void AnimateLineDrawing(Line line)
        {
            var animation = new System.Windows.Media.Animation.DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5), // Faster animation for better experience
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
