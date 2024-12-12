using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using AdvanceQuizApp;

namespace AdvanceQuizApp
    {
    public partial class PreviousQuizes : Window
        {
        private AVLTree<string> avlTree;

        public PreviousQuizes()
            {
            InitializeComponent();
            avlTree = new AVLTree<string>();
            LoadAvlTree(); // Load AVL tree from JSON file
            DisplayUserQuizzes();
            }

        // Method to load the AVL tree from avltree.json
        private void LoadAvlTree()
            {
            string filePath = "avltree.json";

            if (File.Exists(filePath))
                {
                string json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, List<object>>>(json);

                foreach (var entry in data)
                    {
                    foreach (var quiz in entry.Value)
                        {
                        avlTree.Insert(entry.Key, quiz);
                        }
                    }
                }
            else
                {
                MessageBox.Show("AVL tree data file not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        // Method to display the quizzes of the current user in the UI
        private void DisplayUserQuizzes()
            {
            string currentUserFile = "CurrentUser.txt";

            if (!File.Exists(currentUserFile))
                {
                MessageBox.Show("Current user file not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
                }

            string[] userData = File.ReadAllText(currentUserFile).Split(',');

            if (userData.Length < 2)
                {
                MessageBox.Show("Invalid current user data.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
                }

            string currentUserName = userData[0];

            List<object> userQuizzes = new List<object>();

            avlTree.Traverse(node =>
            {
                if (node.Key == currentUserName)
                    {
                    userQuizzes.AddRange(node.Data);
                    }
            });

            if (userQuizzes.Count == 0)
                {
                MessageBox.Show("No quizzes found for the current user.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
                }

            QuizButtonsPanel.Children.Clear();

            foreach (var quiz in userQuizzes)
                {
                if (quiz is JsonElement quizElement && quizElement.TryGetProperty("QuizId", out JsonElement quizIdElement))
                    {
                    string quizId = quizIdElement.GetString();

                    Button quizButton = new Button
                        {
                        Content = quizId,
                        Margin = new Thickness(5),
                        Padding = new Thickness(10),
                        Background = System.Windows.Media.Brushes.LightGray
                        };

                    quizButton.Click += (s, e) =>
                    {
                        MessageBox.Show($"Quiz Selected: {quizId}", "Quiz Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    };

                    QuizButtonsPanel.Children.Add(quizButton);
                    }
                }
            }

        // Method for the back button click event to close the current window
        private void BackButton_Click(object sender, RoutedEventArgs e)
            {
            Window window = new MainWindow();
            window.Show();
            this.Close();

            }
        }
    }