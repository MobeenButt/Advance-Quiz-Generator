using System.IO;
using System.Windows;
using System.Windows.Controls;
using AdvanceQuizApp.DataBank;
using Newtonsoft.Json;

namespace AdvanceQuizApp.Admin_Pages
{
    /// <summary>
    /// Interaction logic for DeletePanel.xaml
    /// </summary>
    public partial class DeletePanel : Window
    {
        private List<Question> Quizzes = new List<Question>();
        public DeletePanel()
        {
            InitializeComponent();
            LoadQuizzes();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminPanel admin = new AdminPanel();
            admin.Show();
        }
        private void QuizDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void LoadQuizzes()
        {
            string filepath = "quizdata.json";
            if (File.Exists(filepath))
            {
                string json = File.ReadAllText(filepath);
                var quizData = JsonConvert.DeserializeObject<Dictionary<string, List<Question>>>(json);
                if (quizData != null && quizData.ContainsKey("questions"))
                {
                    Quizzes = quizData["questions"];
                }
            }
            QuizDataGrid.ItemsSource = Quizzes;
        }
        private void DeleteQuiz_Click(object sender, RoutedEventArgs e)
        {
            var selectedQuiz = QuizDataGrid.SelectedItem as Question;

            if (selectedQuiz != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete the quiz '{selectedQuiz.topic}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Remove the selected quiz from the list
                    Quizzes.Remove(selectedQuiz);

                    // Update the JSON file
                    SaveQuizzesToFile();

                    // Refresh the DataGrid
                    QuizDataGrid.ItemsSource = null;
                    QuizDataGrid.ItemsSource = Quizzes;

                    MessageBox.Show("Quiz deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a quiz to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveQuizzesToFile()
        {
            var quizData = new Dictionary<string, List<Question>> { { "questions", Quizzes } };
            string json = JsonConvert.SerializeObject(quizData, Formatting.Indented);
            File.WriteAllText("quizdata.json", json);
        }
    }

}

