using System.IO;
using System.Windows;
using System.Windows.Input;
using AdvanceQuizApp.DataBank;
using Newtonsoft.Json;
namespace AdvanceQuizApp.Admin_Pages
{
    /// <summary>
    /// Interaction logic for EditQuizPage.xaml
    /// </summary>
    public partial class EditQuizPage : Window
    {
        public List<Question> questions = new List<Question>();
        public string quizName;
        public EditQuizPage()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void LoadData()
        {
            string filepath = "quizdata.json";
            if (File.Exists(filepath))
            {
                string json = File.ReadAllText(filepath);
                var QuizData = JsonConvert.DeserializeObject<Dictionary<string, List<Question>>>(json);
                if (QuizData != null && QuizData.ContainsKey("questions"))
                {
                    questions.Clear();
                    questions.AddRange(QuizData["questions"]);
                    QuestionsListView.ItemsSource = questions;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Question newquestion = new Question
            {
                id = questions.Count + 1,
                text =QuestionTextBox.Text,
                options = new List<string> {
                Option1TextBox.Text,
                Option2TextBox.Text,
                Option3TextBox.Text,
                Option4TextBox.Text
                },
                correctAnswer = CorrectAnswerTextBox.Text,
                topic = TopicTextBox.Text,
                difficulty = DifficultyTextBox.Text,
                favourite = 0

            };
            questions.Add(newquestion);
            QuestionsListView.Items.Refresh();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var quizData = new Dictionary<string, List<Question>> { { "questions", questions } };
            string json = JsonConvert.SerializeObject(quizData, Formatting.Indented);

            string filePath = "quizdata.json";
            File.WriteAllText(filePath, json);
            MessageBox.Show("Quiz saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();

        }
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
