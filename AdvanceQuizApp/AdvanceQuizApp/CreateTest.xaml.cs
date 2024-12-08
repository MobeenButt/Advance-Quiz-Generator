using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;

namespace AdvanceQuizApp
{
    public partial class CreateTest : Window
    {
        private List<AdvanceQuizApp.DataBank.Question> questions;
        private int currentQuestionIndex;

        public CreateTest()
        {
            InitializeComponent();
            LoadQuestions();
            DisplayQuestion(0);

        }
        private void Button_ReturnButtton_Click(object sender, RoutedEventArgs e)
        {
            Window win = new MainWindow();
            win.Show();
            win.WindowState = WindowState.Maximized;
            this.Visibility = Visibility.Hidden;
        }
        private void LoadQuestions()
        {
            try
            {
                string jsonPath = "quizdata.json"; // Path to your JSON file
                string jsonString = File.ReadAllText(jsonPath);
                var questionData = JsonSerializer.Deserialize<QuestionData>(jsonString);
                questions = questionData.questions;
                currentQuestionIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading questions: {ex.Message}");
            }
        }

        private void DisplayQuestion(int index)
        {
            if (questions == null || index < 0 || index >= questions.Count)
            {
                MessageBox.Show("Invalid question index.");
                return;
            }

            var question = questions[index];

            // Update Topic and Difficulty
            TopicDifficultyTextBlock.Text = $"Topic: {question.topic} | Difficulty: {question.difficulty}";

            // Update Question Text
            QuestionTextBlock.Text = question.text;

            // Update Question Number Display
            int currentQuestionNumber = question.id; // Assuming `id` is a 1-based index representing the question number
            int totalQuestions = questions.Count;
            QuestionNumberTextBlock.Text = $"Question {currentQuestionNumber} of {totalQuestions}";

            // Update Options
            OptionsStackPanel.Children.Clear(); // Clear previous options
            foreach (var option in question.options)
            {
                var radioButton = new RadioButton
                {
                    GroupName = "Answers",
                    Content = option,
                    FontSize = 16,
                    Margin = new Thickness(5)
                };
                OptionsStackPanel.Children.Add(radioButton);
            }

            // Hide Correct Answer Display
            CorrectAnswerTextBlock.Visibility = Visibility.Collapsed;
        }



        // Event Handler for Next Button
        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestionIndex < questions.Count - 1)
            {
                currentQuestionIndex++;
                DisplayQuestion(currentQuestionIndex);
            }
            else
            {
                MessageBox.Show("You have reached the last question.");
            }
        }

        // Event Handler for Previous Button
        private void PreviousQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestionIndex > 0)
            {
                currentQuestionIndex--;
                DisplayQuestion(currentQuestionIndex);
            }
            else
            {
                MessageBox.Show("You are on the first question.");
            }
        }
        private void CheckAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (questions == null || currentQuestionIndex < 0 || currentQuestionIndex >= questions.Count)
            {
                MessageBox.Show("Invalid question index.");
                return;
            }

            var correctAnswer = questions[currentQuestionIndex].correctAnswer;
            CorrectAnswerTextBlock.Text = $"Correct Answer: {correctAnswer}";
            CorrectAnswerTextBlock.Visibility = Visibility.Visible; // Show the correct answer
        }
    }
    public class QuestionData
    {
        public List<AdvanceQuizApp.DataBank.Question> questions { get; set; }
    }


}