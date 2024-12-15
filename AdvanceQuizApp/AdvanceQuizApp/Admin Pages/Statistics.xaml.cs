using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace AdvanceQuizApp.Admin_Pages
{
    public partial class Statistics : Window
    {
        private readonly string _filePath = "avltree.json";

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
                var users = ExtractUserDataFromTree();

                int totalQuestions = 0;
                int correctAnswers = 0;

                foreach (var quizzes in users.Values)
                {
                    foreach (var quiz in quizzes)
                    {
                        totalQuestions += quiz.Questions.Count(q => q.Attempted == 1);
                        correctAnswers += quiz.Questions.Count(q => q.RightOrWrong == 1);
                    }
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

        private Dictionary<string, List<QuizData>> ExtractUserDataFromTree()
        {
            if (!File.Exists(_filePath))
            {
                MessageBox.Show("User data file not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new Dictionary<string, List<QuizData>>();
            }

            try
            {
                string jsonContent = File.ReadAllText(_filePath);
                var treeNode = JsonSerializer.Deserialize<AvlTreeNode>(jsonContent);

                var users = new Dictionary<string, List<QuizData>>();
                TraverseTree(treeNode, users);
                return users;
            }
            catch (JsonException ex)
            {
                MessageBox.Show($"Error parsing JSON data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return new Dictionary<string, List<QuizData>>();
            }
        }

        private void TraverseTree(AvlTreeNode node, Dictionary<string, List<QuizData>> users)
        {
            if (node == null) return;

            if (node.Key != null && node.Data != null)
            {
                users[node.Key] = node.Data;
            }

            TraverseTree(node.Left, users);
            TraverseTree(node.Right, users);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new AdminPanel().Show();
        }
    }

    public class AvlTreeNode
    {
        [JsonPropertyName("Key")]
        public string Key { get; set; }

        [JsonPropertyName("Data")]
        public List<QuizData> Data { get; set; }

        [JsonPropertyName("Height")]
        public int Height { get; set; }

        [JsonPropertyName("Left")]
        public AvlTreeNode Left { get; set; }

        [JsonPropertyName("Right")]
        public AvlTreeNode Right { get; set; }
    }
    public class QuestionData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("attempted")]
        public int Attempted { get; set; } 

        [JsonPropertyName("selectedOption")]
        public string SelectedOption { get; set; }

        [JsonPropertyName("rightOrWrong")]
        public int RightOrWrong { get; set; } 
    }
}
