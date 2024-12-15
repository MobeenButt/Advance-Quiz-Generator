using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace AdvanceQuizApp.Admin_Pages
{
    public partial class Report : Window
    {
        private string filepath = "avltree.json";
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
                    var rootNode = JsonSerializer.Deserialize<TreeNode>(jsonContent);

                    if (rootNode != null)
                    {
                        data = new Dictionary<string, List<QuizData>>();
                        TraverseTree(rootNode);

                        UserComboBox.Items.Clear();
                        foreach (var user in data.Keys)
                        {
                            UserComboBox.Items.Add(user);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No data found in the file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void TraverseTree(TreeNode node)
        {
            if (node == null) return;

            // Process the current node
            if (!string.IsNullOrEmpty(node.Key) && node.Data != null)
            {
                data[node.Key] = node.Data;
            }

            // Recursively traverse the left and right nodes
            TraverseTree(node.Left);
            TraverseTree(node.Right);
        }

        // Define a class for the AVL Tree nodes
        public class TreeNode
        {
            public string Key { get; set; }
            public List<QuizData> Data { get; set; }
            public int Height { get; set; }
            public TreeNode Left { get; set; }
            public TreeNode Right { get; set; }
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
                    totalQuestions += quiz.Questions.Count(q => q.Attempted == 1);
                    correctAnswers += quiz.Questions.Count(q => q.RightOrWrong == 1);
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
            new AdminPanel().Show();
        }
    }

    public class QuizData
    {
        public string QuizId { get; set; }
        public List<QuestionData> Questions { get; set; }
    }

}
