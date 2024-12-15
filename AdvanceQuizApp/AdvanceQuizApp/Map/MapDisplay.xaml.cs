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
            WindowState = WindowState.Maximized;
            initializeGraph();
        }

        private void initializeGraph()
        {
            var node1 = new Node { Name = "Building A", Latitude = 100, Longitude = 100 };
            var node2 = new Node { Name = "Building B", Latitude = 300, Longitude = 100 };
            var node3 = new Node { Name = "Building C", Latitude = 500, Longitude = 100 };
            var node4 = new Node { Name = "Building D", Latitude = 300, Longitude = 300 };

            graph = new Graph();
            graph.Nodes.Add(node1);
            graph.Nodes.Add(node2);
            graph.Nodes.Add(node3);
            graph.Nodes.Add(node4);

            graph.Edges.Add(new Edge { node1 = node1, node2 = node2, Weight = 200 });
            graph.Edges.Add(new Edge { node1 = node2, node2 = node3, Weight = 200 });
            graph.Edges.Add(new Edge { node1 = node1, node2 = node4, Weight = 300 });
            graph.Edges.Add(new Edge { node1 = node4, node2 = node3, Weight = 300 });

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
            savecurrentstate();
            var startLocation = StartLocation.Text;
            var endLocation = EndLocation.Text;

            if (graph != null && graph.Nodes != null)
            {
                var startNode = graph.Nodes.FirstOrDefault(n => n.Name == startLocation);
                var endNode = graph.Nodes.FirstOrDefault(n => n.Name == endLocation);

                if (startNode != null && endNode != null)
                {
                    var path = graph.FindShortestPath(startNode, endNode);
                    DisplayPath(path);
                }
                else
                {
                    MessageBox.Show("One or both locations not found.");
                }
            }
            else
            {
                MessageBox.Show("Graph not initialized.");
            }
        }

        private void DisplayPath(List<Node> path)
        {
            if (path.Count == 0)
            {
                MessageBox.Show("No path found.");
                return;
            }

            // Draw the entire graph again with default colors
            graph.display(MapPanel);

            // Highlight edges and nodes in the path
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
                    Stroke = Brushes.Red, // Highlighted edge color
                    StrokeThickness = 4
                };
                MapPanel.Children.Add(edgeLine);
            }

            // Highlight nodes in the path
            foreach (var node in path)
            {
                var rect = new Rectangle
                {
                    Width = 40,
                    Height = 40,
                    Fill = Brushes.Green, // Path node color
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                Canvas.SetLeft(rect, node.Longitude - 20); // Center the rectangle
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
                Duration = TimeSpan.FromSeconds(1),
                FillBehavior = System.Windows.Media.Animation.FillBehavior.HoldEnd
            };

            var storyboard = new System.Windows.Media.Animation.Storyboard();
            System.Windows.Media.Animation.Storyboard.SetTarget(animation, line);
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

        private void BackClick(object sender, RoutedEventArgs e)
        {
            Window AdminPanel = new AdminPanel();
            AdminPanel.Show();
            this.Close();
        }
    }
}
