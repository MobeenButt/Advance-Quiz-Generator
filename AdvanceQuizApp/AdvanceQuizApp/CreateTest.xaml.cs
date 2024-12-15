using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AdvanceQuizApp
    {
    public partial class CreateTest : Window
        {
        private List<AdvanceQuizApp.Question> questions;
        private int currentQuestionIndex;
        private DateTime quizStartTime;
        private int correctAnswers = 0;
        private string quizid;
        //
        public CreateTest(List<AdvanceQuizApp.Question> quizQuestions, string QuizId)
            {
            InitializeComponent();
            questions = quizQuestions;
            currentQuestionIndex = 0;
            quizStartTime = DateTime.Now;
            quizid = QuizId;

            // Load quiz state from AVL tree
            LoadQuizState();

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

        //private void Button_ReturnButtton_Click(object sender, RoutedEventArgs e)
        //    {
        //    Window win = new MainWindow();
        //    win.Show();
        //    win.WindowState = WindowState.Maximized;
        //    this.Visibility = Visibility.Hidden;
        //    }
        //private void LoadQuestions()
        //    {
        //    try
        //        {
        //        string jsonPath = "quizdata.json";
        //        string jsonString = File.ReadAllText(jsonPath);
        //        var questionData = JsonSerializer.Deserialize<QuestionData>(jsonString);
        //        questions = questionData.questions;
        //        currentQuestionIndex = 0;
        //        }
        //    catch (Exception ex)
        //        {
        //        MessageBox.Show($"Error loading questions: {ex.Message}");
        //        }
        //    }
        private void Button_ReturnButton_Click(object sender, RoutedEventArgs e)
            {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.WindowState = WindowState.Maximized;
            Close();
            }

        private void LoadQuizState()
            {
            try
                {
                string currentUserFile = "CurrentUser.txt";
                if (!File.Exists(currentUserFile))
                    throw new FileNotFoundException("Current user file not found!");

                string currentUser = File.ReadAllText(currentUserFile).Split(',')[0];

                string avlFilePath = "avltree.json";
                if (!File.Exists(avlFilePath))
                    throw new FileNotFoundException("AVL tree file not found!");

                // Load the AVL tree from the file
                AVLTree<string> quizTree = new AVLTree<string>();
                quizTree.LoadFromJson(avlFilePath);

                // Search for the user's data in the AVL tree
                var userNode = quizTree.Find(currentUser);

                if (userNode == null || userNode.Data == null || userNode.Data.Count == 0)
                    {
                    MessageBox.Show($"No quizzes found for the user '{currentUser}'.");
                    return;
                    }

                // Find the quiz data for the specific quiz ID

                var quizData = userNode.Data
                    .Cast<JsonElement>()
                    .FirstOrDefault(q => q.GetProperty("QuizId").GetString() == quizid);

                if (quizData.ValueKind == JsonValueKind.Undefined)
                    {
                    MessageBox.Show($"Quiz with ID '{quizid}' not found. Starting a new quiz.");
                    // Initialize quiz state for a new quiz
                    foreach (var question in questions)
                        {
                        question.attempted = false;
                        question.selectedOption = null;
                        question.rightOrWrong = false;
                        }
                    return;
                    }

                // Load the state of each question from the quiz data
                foreach (var questionState in quizData.GetProperty("Questions").EnumerateArray())
                    {
                    var questionId = questionState.GetProperty("id").GetInt32();
                    var matchingQuestion = questions.FirstOrDefault(q => q.id == questionId);
                    if (matchingQuestion != null)
                        {
                        matchingQuestion.attempted = questionState.GetProperty("attempted").GetInt32() == 1;
                        matchingQuestion.selectedOption = questionState.GetProperty("selectedOption").GetString();
                        matchingQuestion.rightOrWrong = questionState.GetProperty("rightOrWrong").GetInt32() == 1;
                        }
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show($"Error loading quiz state: {ex.Message}");
                }
            }


        private void DisplayQuestion(int index)
            {
            try
                {
                if (questions == null || index < 0 || index >= questions.Count)
                    {
                    MessageBox.Show("Invalid question index.");
                    return;
                    }

                // Fetch and display the question details
                var question = questions[index];

                TopicDifficultyTextBlock.Text = $"Topic: {question.topic} | Difficulty: {question.difficulty}";
                QuestionTextBlock.Text = question.text;

                int currentQuestionNumber = index + 1;
                QuestionNumberTextBlock.Text = $"Question {currentQuestionNumber} of {questions.Count}";

                OptionsStackPanel.Children.Clear();

                for (int i = 0; i < question.options.Count; i++)
                    {
                    var optionText = question.options[i];
                    var radioButton = new RadioButton
                        {
                        GroupName = "Answers",
                        Content = optionText,
                        FontSize = 16,
                        Margin = new Thickness(5)
                        };

                    // Check if this question was attempted and this option was selected
                    if (question.attempted && question.selectedOption == optionText)
                        {
                        radioButton.IsChecked = true;
                        }

                    int optionIndex = i; // Capture the index for the lambda
                    radioButton.Checked += (s, e) =>
                    {
                        question.selectedOption = optionText;
                        question.attempted = true;

                        foreach (var child in OptionsStackPanel.Children)
                            {
                            if (child is RadioButton rb)
                                {
                                rb.IsEnabled = false;
                                }
                            }
                    };

                    OptionsStackPanel.Children.Add(radioButton);
                    }

                // Update favorite icon
                FavouriteIcon.Text = question.favourite == 1 ? "♥" : "♡";

                // If the question was attempted, show the correct answer
                if (question.attempted)
                    {
                    var correctAnswer = question.correctAnswer.Trim();
                    if (question.selectedOption == correctAnswer)
                        {
                        CorrectAnswerTextBlock.Text = $"Correct Answer: {correctAnswer}";
                        CorrectAnswerTextBlock.Foreground = Brushes.Green;
                        }
                    else
                        {
                        CorrectAnswerTextBlock.Text = $"Incorrect. Correct Answer: {correctAnswer}";
                        CorrectAnswerTextBlock.Foreground = Brushes.Red;
                        }

                    CorrectAnswerTextBlock.Visibility = Visibility.Visible;
                    }
                else
                    {
                    CorrectAnswerTextBlock.Text = "";
                    CorrectAnswerTextBlock.Visibility = Visibility.Collapsed;
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show($"Error displaying question: {ex.Message}");
                }
            }


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

        private void SaveQuizButton_Click(object sender, RoutedEventArgs e)
            {
            try
                {
                string currentUserFile = "CurrentUser.txt";
                if (!File.Exists(currentUserFile))
                    throw new FileNotFoundException("Current user file not found!");

                string userName = File.ReadAllText(currentUserFile).Split(',')[0];

                // Create quiz data
                var quizData = new
                    {
                    QuizId = quizid,
                    Questions = questions.Select(q => new
                        {
                        q.id,
                        attempted = q.attempted ? 1 : 0,
                        selectedOption = q.selectedOption,
                        rightOrWrong = q.rightOrWrong ? 1 : 0
                        }).ToList()
                    };

                // Load existing AVLTree or create a new one
                string avlFilePath = "avltree.json";
                AVLTree<string> avlTree = new AVLTree<string>();

                if (File.Exists(avlFilePath))
                    {
                    avlTree.LoadFromJson(avlFilePath); // Load the AVLTree from the JSON file
                    }

                // Insert the new quiz data into the AVLTree under the username
                avlTree.Insert(userName, quizData);

                // Save the updated AVLTree back to the JSON file
                avlTree.SaveToJson(avlFilePath);

                MessageBox.Show("Quiz saved successfully!");
                }
            catch (Exception ex)
                {
                MessageBox.Show($"Error saving quiz: {ex.Message}");
                }
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

            var question = questions[currentQuestionIndex];
            var correctAnswer = question.correctAnswer.Trim();

            // Check if an option is selected
            RadioButton selectedOption = OptionsStackPanel.Children.OfType<RadioButton>()
                .FirstOrDefault(r => r.IsChecked == true);

            if (selectedOption == null)
                {
                MessageBox.Show("Please select an option before checking the answer.");
                return;
                }

            // Update selectedOption (in case it's not handled yet)
            question.selectedOption = selectedOption.Content.ToString().Trim();
            question.attempted = true; // Mark question as attempted

            // Update rightOrWrong based on the selected answer
            if (question.selectedOption == correctAnswer)
                {
                CorrectAnswerTextBlock.Text = $"Correct Answer: {correctAnswer}";
                CorrectAnswerTextBlock.Foreground = Brushes.Green;
                question.rightOrWrong = true; // Correct answer
                correctAnswers++;
                }
            else
                {
                CorrectAnswerTextBlock.Text = $"Incorrect. Correct Answer: {correctAnswer}";
                CorrectAnswerTextBlock.Foreground = Brushes.Red;
                question.rightOrWrong = false; // Incorrect answer
                }

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
        public List<AdvanceQuizApp.Question> questions { get; set; }
        }


    }