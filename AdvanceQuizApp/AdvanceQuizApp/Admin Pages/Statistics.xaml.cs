using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace AdvanceQuizApp.Admin_Pages
{
    public partial class Statistics : Window
    {
        private readonly string _userDataFilePath = "avltree.json"; // Path to user data JSON file

        public Statistics()
        {
            InitializeComponent();
            LoadStatistics();
        }
        private void BtnRefreshStats_Click(object sender, RoutedEventArgs e)
        {
            LoadStatistics();
        }

        // Method to load statistics from the JSON file
        private void LoadStatistics()
        {
            try
            {
                // Load user data from JSON file
                var users = LoadUsersFromJson();

                int totalQuestions = 0;
                int correctAnswers = 0;

                // Using LINQ to calculate total questions and correct answers
                foreach (var quiz in users.Values.SelectMany(userQuizzes => userQuizzes))
                {
                    totalQuestions += quiz.Questions.Count(q => q.attempted == 1); // Count attempted questions
                    correctAnswers += quiz.Questions.Count(q => q.rightOrWrong == 1); // Count correct answers
                }

                // Calculate the score
                int totalScore = totalQuestions > 0 ? (int)((double)correctAnswers / totalQuestions * 100) : 0;

                // Update UI with statistics
                lblTotalQuestionsValue.Text = totalQuestions.ToString();
                lblCorrectAnswersValue.Text = correctAnswers.ToString();
                lblScoreValue.Text = $"{totalScore}%";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statistics: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to load user data from JSON file
        private Dictionary<string, List<QuizData>> LoadUsersFromJson()
        {
            try
            {
                if (File.Exists(_userDataFilePath))
                {
                    string jsonContent = File.ReadAllText(_userDataFilePath);
                    return JsonSerializer.Deserialize<Dictionary<string, List<QuizData>>>(jsonContent) ?? new Dictionary<string, List<QuizData>>();
                }
                else
                {
                    MessageBox.Show("User data file not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return new Dictionary<string, List<QuizData>>();
                }
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing JSON data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new Dictionary<string, List<QuizData>>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new Dictionary<string, List<QuizData>>();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminPanel admin = new AdminPanel();
            admin.Show();

        }
    }

    // Represents a question data object
    public class QuestionData
    {
        public int id { get; set; }
        public int attempted { get; set; } // 1 if attempted, 0 if not attempted
        public string selectedOption { get; set; }
        public int rightOrWrong { get; set; } // 1 if correct, 0 if wrong
    }

}
