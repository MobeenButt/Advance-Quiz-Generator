﻿using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AdvanceQuizApp
{
    public partial class CreateTest : Window
    {
        private List<AdvanceQuizApp.DataBank.Question> questions;
        private int currentQuestionIndex;
        private DateTime quizStartTime;
        private int correctAnswers = 0;
        //
        public CreateTest(List<AdvanceQuizApp.DataBank.Question> quizQuestions)
            {
            InitializeComponent();
            questions = quizQuestions; // Assign questions passed from CreateQuiz
            currentQuestionIndex = 0;
            quizStartTime = DateTime.Now;

            if (questions != null && questions.Count > 0)
                {
                DisplayQuestion(0);
                }
            else
                {
                MessageBox.Show("No questions available for the test.");
                Close();
                }
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
                string jsonPath = "quizdata.json";
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
            int currentQuestionNumber = index + 1; // 1-based index for question number
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
            FavouriteIcon.Text = question.favourite == 1 ? "♥" : "♡";
            }


        private void MarkFavorite_Click(object sender, RoutedEventArgs e)
            {
            if (questions == null || currentQuestionIndex < 0 || currentQuestionIndex >= questions.Count)
                {
                MessageBox.Show("Invalid question index.");
                return;
                }

            var question = questions[currentQuestionIndex];

            // Toggle the favourite value
            question.favourite = question.favourite == 0 ? 1 : 0;

            // Update the icon
            FavouriteIcon.Text = question.favourite == 1 ? "♥" : "♡";

            // Save changes back to the JSON file without affecting other questions
            try
                {
                // Read the existing JSON file
                string jsonPath = "quizdata.json";
                string jsonString = File.ReadAllText(jsonPath);

                // Deserialize into a list of questions
                var questionData = JsonSerializer.Deserialize<QuestionData>(jsonString);
                if (questionData == null)
                    {
                    throw new Exception("Failed to deserialize the question data.");
                    }

                // Debugging: Check if the question exists in the data
                var existingQuestion = questionData.questions.FirstOrDefault(q => q.id == question.id); // Assuming 'id' is a unique identifier
                if (existingQuestion != null)
                    {
                    // Update the favourite status for the specific question
                    existingQuestion.favourite = question.favourite;
                    }
                else
                    {
                    // If the question is not found, show an error message
                    MessageBox.Show("Question not found in the data.");
                    return;
                    }

                // Serialize the updated data and save it back to the file
                string updatedJson = JsonSerializer.Serialize(questionData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(jsonPath, updatedJson);

                MessageBox.Show("Favorite status updated successfully!");
                }
            catch (Exception ex)
                {
                MessageBox.Show($"Error saving favorite status: {ex.Message}");
                }
            }




        // Event Handler for Next Button
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

            // Get the correct answer
            var correctAnswer = questions[currentQuestionIndex].correctAnswer.Trim();

            // Check if an option is selected
            RadioButton selectedOption = OptionsStackPanel.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked == true);

            if (selectedOption == null)
            {
                MessageBox.Show("Please select an option before checking the answer.");
                return;
            }

            // Check if the selected option matches the correct answer
            if (selectedOption.Content.ToString().Trim() == correctAnswer)
            {
                CorrectAnswerTextBlock.Text = $"Correct Answer: {correctAnswer}";
                CorrectAnswerTextBlock.Foreground = Brushes.Green; // Display in green
                correctAnswers++;
            }
            else
            {
                CorrectAnswerTextBlock.Text = $"Incorrect. Correct Answer: {correctAnswer}";
                CorrectAnswerTextBlock.Foreground = Brushes.Red; // Display in red
            }

            // Show the correct answer
            CorrectAnswerTextBlock.Visibility = Visibility.Visible;
        }
        private void StopQuizButton_Click(object sender, RoutedEventArgs e)
        {
            // Calculate elapsed time
            TimeSpan elapsedTime = DateTime.Now - quizStartTime;

            // Show the results window
            int totalQuestions = questions?.Count ?? 0;
            TestResult endTestWindow = new TestResult(totalQuestions, correctAnswers, elapsedTime);
            endTestWindow.ShowDialog();

            // Close the quiz window after showing results
            this.Close();
        }
    }
    public class QuestionData
    {
        public List<AdvanceQuizApp.DataBank.Question> questions { get; set; }
    }


}