using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
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
        private void QuizButton_Click(string quizId)
        {
            // Retrieve the quiz data for the clicked quiz
            List<int> questionIds = GetQuestionIdsForQuiz(quizId);

            if (questionIds == null || questionIds.Count == 0)
            {
                MessageBox.Show("No questions found for the selected quiz.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Retrieve questions from quizdata.json
            List<Question> quizQuestions = GetQuestionsFromJson(questionIds);

            if (quizQuestions == null || quizQuestions.Count == 0)
            {
                MessageBox.Show("Failed to load questions for the selected quiz.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Instantiate and display the CreateTest window
            CreateTest createTest = new CreateTest(quizQuestions, quizId);
            createTest.Show();
            this.Close(); // Close the current window if needed
        }

        private List<int> GetQuestionIdsForQuiz(string quizId)
        {
            List<int> questionIds = new List<int>();

            avlTree.Traverse(node =>
            {
                foreach (var quiz in node.Data)
                {
                    if (quiz is JsonElement quizElement && quizElement.TryGetProperty("QuizId", out JsonElement quizIdElement) &&
                        quizIdElement.GetString() == quizId)
                    {
                        if (quizElement.TryGetProperty("Questions", out JsonElement questionsElement) && questionsElement.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var question in questionsElement.EnumerateArray())
                            {
                                if (question.TryGetProperty("id", out JsonElement idElement) && idElement.TryGetInt32(out int questionId))
                                {
                                    questionIds.Add(questionId);
                                }
                            }
                        }
                    }
                }
            });

            return questionIds;
        }

        private List<Question> GetQuestionsFromJson(List<int> questionIds)
        {
            string filePath = "quizdata.json";

            if (!File.Exists(filePath))
            {
                MessageBox.Show("Questions data file not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            string json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, List<Question>>>(json);

            return data["questions"]
                .Where(question => questionIds.Contains(question.id))
                .ToList();
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

                    // Update the Click event to call QuizButton_Click with the quizId
                    quizButton.Click += (s, e) =>
                    {
                        QuizButton_Click(quizId);
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