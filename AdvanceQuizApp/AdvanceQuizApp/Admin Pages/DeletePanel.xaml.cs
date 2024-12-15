using System.IO;
using System.Windows;
using System.Windows.Input;
using AdvanceQuizApp;
using Newtonsoft.Json;

namespace AdvanceQuizApp.Admin_Pages
{
    /// <summary>
    /// Interaction logic for DeletePanel.xaml
    /// </summary>
    public partial class DeletePanel : Window
    {
        private List<Question> Questions = new List<Question>();
        public DeletePanel()
        {
            InitializeComponent();
            LoadQuestions();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminPanel admin = new AdminPanel();
            admin.Show();
        }
        private void DeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            var selectedquestion = (Question)QuestionDataGrid.SelectedItem;
            if (selectedquestion != null)
            {
                RemoveSelectedQuestion(selectedquestion.id);
                QuestionDataGrid.ItemsSource = null;
                QuestionDataGrid.ItemsSource = LoadQuestions();
            }
        }
        private List<Question> LoadQuestions()
        {
            string file = "quizdata.json";
            if (File.Exists(file))
            {
                var json = File.ReadAllText(file);
                var data = JsonConvert.DeserializeObject<Dictionary<string, List<Question>>>(json);
                if (data != null && data.ContainsKey("questions"))
                {
                    Questions.Clear();
                    Questions.AddRange(data["questions"]);
                    return Questions;
                }

            }
            return new List<Question>();
        }
        private void RemoveSelectedQuestion(int questionId)
        {
            string filePath = "quizdata.json";
            var jsonData = File.ReadAllText(filePath);
            var data = JsonConvert.DeserializeObject<Dictionary<string, List<Question>>>(jsonData);

            if (data != null && data.ContainsKey("questions"))
            {
                var questions = data["questions"];
                var questionToRemove = questions.FirstOrDefault(q => q.id == questionId);

                if (questionToRemove != null)
                {
                    questions.Remove(questionToRemove);
                    File.WriteAllText(filePath, JsonConvert.SerializeObject(data, Formatting.Indented));
                    MessageBox.Show("Question deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Question not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            QuestionDataGrid.ItemsSource=LoadQuestions();
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

