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
        private void LoadStatistics()
        {
            try
            {
                var users = LoadUsersFromJson();
                int totalQuestions = 0;
                int correctAnswers = 0;

                foreach (var quiz in users.Values.SelectMany(userQuizzes => userQuizzes))
                {
                    totalQuestions += quiz.Questions.Count(q => q.attempted == 1); 
                    correctAnswers += quiz.Questions.Count(q => q.rightOrWrong == 1); 
                }
                int totalScore = totalQuestions > 0 ? (int)((double)correctAnswers / totalQuestions * 100) : 0;

                lblTotalQuestionsValue.Text = totalQuestions.ToString();
                lblCorrectAnswersValue.Text = correctAnswers.ToString();
                lblScoreValue.Text = $"{totalScore}%";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statistics: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
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
    public class QuestionData
    {
        public int id { get; set; }
        public int attempted { get; set; } 
        public string selectedOption { get; set; }
        public int rightOrWrong { get; set; }
    }

}
