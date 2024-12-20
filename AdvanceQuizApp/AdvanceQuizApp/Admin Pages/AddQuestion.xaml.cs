﻿using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                text = QuestionTextBox.Text,

                options = new List<string> {
                Option1TextBox.Text,
                Option2TextBox.Text,
                Option3TextBox.Text,
                Option4TextBox.Text
                },

                correctAnswer = CorrectAnswerTextBox.Text,
                topic = TopicTextBox.Text,
                difficulty = (DifficultyComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                favourite = 0

            };
            questions.Add(newquestion);
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "quizdata.json";
            var existingQuestions = new List<Question>();

            // Load existing questions from the file
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<Dictionary<string, List<Question>>>(json);
                if (data != null && data.ContainsKey("questions"))
                {
                    existingQuestions = data["questions"];
                }
            }

            int maxId = existingQuestions.Any() ? existingQuestions.Max(q => q.id) : 0;

            foreach (var question in questions)
            {
                if (question.id <= maxId)
                {
                    question.id = ++maxId;
                }
            }
            existingQuestions.AddRange(questions);
            var updatedData = new Dictionary<string, List<Question>> { { "questions", existingQuestions } };
            string updatedJson = JsonConvert.SerializeObject(updatedData, Formatting.Indented);
            File.WriteAllText(filePath, updatedJson);
            MessageBox.Show("Quiz updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            questions.Clear();
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

        private void AddQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            int newId = 1;
            if (questions.Count > 0)
            {
                newId = questions.Max(q => q.id) + 1;
            }

            Question newQuestion = new Question
            {
                id = newId,
                text = QuestionTextBox.Text,
                options = new List<string> {
            Option1TextBox.Text,
            Option2TextBox.Text,
            Option3TextBox.Text,
            Option4TextBox.Text
        },
                correctAnswer = CorrectAnswerTextBox.Text,
                topic = TopicTextBox.Text,
                difficulty = (DifficultyComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                favourite = 0
            };

            questions.Add(newQuestion);
            // Clear the input fields
            QuestionTextBox.Clear();
            Option1TextBox.Clear();
            Option2TextBox.Clear();
            Option3TextBox.Clear();
            Option4TextBox.Clear();
            CorrectAnswerTextBox.Clear();
            TopicTextBox.Clear();
            DifficultyComboBox.SelectedIndex = -1;

        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminPanel admin = new AdminPanel();
            admin.Show();
        }

        private void DifficultyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
