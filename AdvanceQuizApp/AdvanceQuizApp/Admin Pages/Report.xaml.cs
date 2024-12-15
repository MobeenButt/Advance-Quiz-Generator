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
        private Dictionary<string, List<QuizData>> data;

        public Report()
        {
            InitializeComponent();
            LoadUsersFromJson();
        }
        private void LoadUsersFromJson()
        {
            try
            {
                if (File.Exists(filepath))
                {
                    string jsonContent = File.ReadAllText(filepath);
                    data = JsonSerializer.Deserialize<Dictionary<string, List<QuizData>>>(jsonContent) ?? new Dictionary<string, List<QuizData>>();

                    UserComboBox.Items.Clear();
                    foreach (var user in data.Keys)
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
        private void GenerateUserReport(string userName)
        {
            if (data.ContainsKey(userName))
            {
                var quizzes = data[userName];
                int totalQuestions = 0;
                int correctAnswers = 0;

                foreach (var quiz in quizzes)
                {
                    var questions = quiz.Questions;
                    totalQuestions += questions.Count(q => q.attempted == 1); // Count attempted questions
                    correctAnswers += questions.Count(q => q.rightOrWrong == 1); // Count correct answers
                }
                int score = totalQuestions > 0 ? (int)((double)correctAnswers / totalQuestions * 100) : 0;
                lblTotalQuestionsValue.Text = totalQuestions.ToString();
                lblCorrectAnswersValue.Text = correctAnswers.ToString();
                lblScoreValue.Text = $"{score}%";
                GenerateChart(totalQuestions, correctAnswers);
            }
            else
            {
                MessageBox.Show($"No data found for user: {userName}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GenerateChart(int totalQuestions, int correctAnswers)
        {
            var plotModel = new PlotModel { Title = "Quiz Report" };
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