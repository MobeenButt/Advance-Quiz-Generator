using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace AdvanceQuizApp
{
    public partial class PreviousQuizes : Window
    {
        private AVLTree<string> avlTree;

        public PreviousQuizes()
        {
            InitializeComponent();
            avlTree = new AVLTree<string>();
            LoadAvlTree();
            DisplayUserQuizzes();
        }

        private void LoadAvlTree()
        {
            string filePath = "avltree.json";
            if (!File.Exists(filePath))
            {
                MessageBox.Show("No previous quizzes found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                avlTree.LoadFromJson(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading quizzes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DisplayUserQuizzes()
        {
            try
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
                    MessageBox.Show("Invalid user data format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string currentUserName = userData[0];
                var userNode = avlTree.Find(currentUserName);

                if (userNode == null || userNode.Data.Count == 0)
                {
                    MessageBox.Show("No quizzes found for the current user.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                QuizButtonsPanel.Children.Clear();
                foreach (var quizObj in userNode.Data)
                {
                    if (quizObj is JsonElement quizElement &&
                        quizElement.TryGetProperty("QuizId", out JsonElement quizIdElement))
                    {
                        string quizId = quizIdElement.GetString();
                        CreateQuizButton(quizId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying quizzes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateQuizButton(string quizId)
        {
            Button quizButton = new Button
            {
                Content = $"Quiz {quizId}",
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                MinWidth = 100,
                MinHeight = 30,
                Background = System.Windows.Media.Brushes.LightBlue
            };

            quizButton.Click += (s, e) => QuizButton_Click(quizId);
            QuizButtonsPanel.Children.Add(quizButton);
        }

        private void QuizButton_Click(string quizId)
        {
            try
            {
                var currentUserName = File.ReadAllText("CurrentUser.txt").Split(',')[0];
                var userNode = avlTree.Find(currentUserName);

                if (userNode == null)
                {
                    MessageBox.Show("User data not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var quiz = userNode.Data
                    .Select(q => JsonSerializer.Deserialize<QuizData>(((JsonElement)q).GetRawText()))
                    .FirstOrDefault(q => q.QuizId == quizId);

                if (quiz == null)
                {
                    MessageBox.Show("Quiz not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var questions = GetQuestionsFromJson(quiz.Questions.Select(q => q.id).ToList());

                if (questions == null || !questions.Any())
                {
                    MessageBox.Show("No questions found for this quiz.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Update the questions with the saved answers
                foreach (var question in questions)
                {
                    var savedQuestion = quiz.Questions.First(q => q.id == question.id);
                    question.attempted = savedQuestion.attempted == 1;
                    question.selectedOption = savedQuestion.selectedOption;
                    question.rightOrWrong = savedQuestion.rightOrWrong == 1;
                }

                CreateTest createTest = new CreateTest(questions, quizId);
                createTest.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading quiz: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<Question> GetQuestionsFromJson(List<int> questionIds)
        {
            try
            {
                string filePath = "quizdata.json";
                if (!File.Exists(filePath))
                    return null;

                string json = File.ReadAllText(filePath);
                var data = JsonSerializer.Deserialize<Dictionary<string, List<Question>>>(json);

                return data["questions"]
                    .Where(q => questionIds.Contains(q.id))
                    .ToList();
            }
            catch
            {
                return null;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new MainWindow();
            window.Show();
            this.Close();
        }

        // Helper class to deserialize quiz data
        private class QuizData
        {
            public string QuizId { get; set; }
            public List<QuizQuestion> Questions { get; set; }
        }

        private class QuizQuestion
        {
            public int id { get; set; }
            public int attempted { get; set; }
            public string selectedOption { get; set; }
            public int rightOrWrong { get; set; }
        }

        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Window m = new MainWindow();
                m.Show();
                m.WindowState = WindowState.Maximized;
                this.Close();
            }
        }
    }
}