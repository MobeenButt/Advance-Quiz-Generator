using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace AdvanceQuizApp.Admin_Pages
{
    public partial class Report : Window
    {
        private string filepath = "avltree.json"; // Path to user data JSON file
        private Dictionary<string, List<QuizData>> _userQuizData;

        public Report()
        {
            InitializeComponent();
            LoadUsersFromJson();
        }

        // Load user data from JSON
        private void LoadUsersFromJson()
        {
            try
            {
                if (File.Exists(filepath))
                {
                    string jsonContent = File.ReadAllText(filepath);
                    _userQuizData = JsonSerializer.Deserialize<Dictionary<string, List<QuizData>>>(jsonContent) ?? new Dictionary<string, List<QuizData>>();

                    // Populate ComboBox with usernames
                    UserComboBox.Items.Clear();
                    foreach (var user in _userQuizData.Keys)
                    {
                        UserComboBox.Items.Add(user);
                    }
                }
                else
                {
                    MessageBox.Show("User data file not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Button click handler to generate report for selected user
        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (UserComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a user.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string selectedUser = UserComboBox.SelectedItem.ToString();
            GenerateUserReport(selectedUser);
        }

        // Generate the report based on selected user
        private void GenerateUserReport(string userName)
        {
            if (_userQuizData.ContainsKey(userName))
            {
                var quizzes = _userQuizData[userName];
                int totalQuestions = 0;
                int correctAnswers = 0;

                foreach (var quiz in quizzes)
                {
                    var questions = quiz.Questions;
                    totalQuestions += questions.Count(q => q.attempted == 1); // Count attempted questions
                    correctAnswers += questions.Count(q => q.rightOrWrong == 1); // Count correct answers
                }

                // Calculate score
                int score = totalQuestions > 0 ? (int)((double)correctAnswers / totalQuestions * 100) : 0;

                // Display report
                lblTotalQuestionsValue.Text = totalQuestions.ToString();
                lblCorrectAnswersValue.Text = correctAnswers.ToString();
                lblScoreValue.Text = $"{score}%";

                // Generate chart
                GenerateChart(totalQuestions, correctAnswers);
            }
            else
            {
                MessageBox.Show($"No data found for user: {userName}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Method to generate the chart
        private void GenerateChart(int totalQuestions, int correctAnswers)
        {
            var plotModel = new PlotModel { Title = "Quiz Report" };

            // Create a bar chart for total questions and correct answers
            var barSeries = new BarSeries
            {
                ItemsSource = new List<BarItem>
                {
                    new BarItem { Value = totalQuestions },
                    new BarItem { Value = correctAnswers }
                },
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0}"
            };

            plotModel.Series.Add(barSeries);

            // Set the plot model to the chart
            quizChart.Model = plotModel;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminPanel admin = new AdminPanel();
            admin.Show();
        }
    }

    public class QuizData
    {
        public string QuizId { get; set; }
        public List<QuestionData> Questions { get; set; }
    }
}