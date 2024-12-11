using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AdvanceQuizApp.ADT;
using AdvanceQuizApp.DataBank;

namespace AdvanceQuizApp
    {
    /// <summary>
    /// Interaction logic for CreateQuiz.xaml
    /// </summary>
    public partial class CreateQuiz : Window
        {
        private TopicBST topicBST; // Binary Search Tree for topics
        private List<string> topics; // List to hold sorted topics

        public CreateQuiz()
            {
            InitializeComponent();
            topicBST = new TopicBST(); // Initialize the BST
            LoadSubjects(); // Load topics into the list box during initialization
            }

        private void LoadSubjects()
            {
            // Load questions from the JSON file
            string filePath = "quizdata.json"; // Ensure this path is correct
            var questions = QuestionLoader.LoadQuestions(filePath);

            // Add topics to the BST
            foreach (var question in questions)
                {
                if (!string.IsNullOrWhiteSpace(question.topic))
                    topicBST.Insert(question.topic);
                }

            // Retrieve sorted topics from the BST
            topics = topicBST.GetSortedTopics();

            // Add topics to the ListBox
            foreach (var topic in topics)
                {
                SubjectsListBox.Items.Add(new ListBoxItem { Content = topic });
                }
            }

        private void BackButton_Click(object sender, RoutedEventArgs e)
            {
            // Logic to go back to the previous window
            this.Close();
            Window br = new MainWindow();
            br.Show();
            }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
            {
            var selectedTopics = SubjectsListBox.SelectedItems.Cast<ListBoxItem>()
                                                             .Select(item => item.Content.ToString())
                                                             .ToList();
            if (selectedTopics.Count == 0)
                {
                MessageBox.Show("Please select at least one topic to create a quiz.");
                return;
                }

            var questions = GetQuestionsForTopics(selectedTopics);
            if (questions.Count == 0)
                {
                MessageBox.Show("No questions available for the selected topics.");
                return;
                }

            // Get the selected difficulty level
            string selectedDifficulty = ((ComboBoxItem)DifficultyComboBox.SelectedItem)?.Content.ToString();

            // Filter questions based on selected difficulty, if any
            if (!string.IsNullOrEmpty(selectedDifficulty))
                {
                questions = questions.Where(q => q.difficulty == selectedDifficulty).ToList();
                }

            // Only modify questions if a number is selected
            var selectedItem = QuestionsComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
                {
                int selectedQuestionCount = int.Parse(selectedItem.Content.ToString());
                // If more questions are available than selected, shuffle and take subset
                if (questions.Count > selectedQuestionCount)
                    {
                    ShuffleQuestions(questions);
                    questions = questions.Take(selectedQuestionCount).ToList();
                    }
                }

            // Pass the questions to the CreateTest window
            CreateTest testWindow = new CreateTest(questions);
            testWindow.Show();
            this.Close();
            }
        private void ShuffleQuestions(List<AdvanceQuizApp.DataBank.Question> questions)
            {
            Random rng = new Random();
            int n = questions.Count;
            while (n > 1)
                {
                n--;
                int k = rng.Next(n + 1);
                var value = questions[k];
                questions[k] = questions[n];
                questions[n] = value;
                }
            }
        private List<AdvanceQuizApp.DataBank.Question> GetQuestionsForTopics(List<string> selectedTopics)
            {
            string filePath = "quizdata.json";
            var allQuestions = QuestionLoader.LoadQuestions(filePath);

            // Filter questions based on selected topics
            var topicQuestions = allQuestions.Where(q => selectedTopics.Contains(q.topic)).ToList();

            return topicQuestions;
            }
        private void CheckNumberOfQuestionsButton_Click(object sender, RoutedEventArgs e)
            {
            var selectedTopics = SubjectsListBox.SelectedItems.Cast<ListBoxItem>()
                                                              .Select(item => item.Content.ToString())
                                                              .ToList();

            if (selectedTopics.Count == 0)
                {
                MessageBox.Show("Please select at least one topic.");
                return;
                }

            var questions = GetQuestionsForTopics(selectedTopics);

            // Get the selected difficulty level
            string selectedDifficulty = ((ComboBoxItem)DifficultyComboBox.SelectedItem)?.Content.ToString();

            // Apply difficulty filter if selected
            if (!string.IsNullOrEmpty(selectedDifficulty))
                {
                questions = questions.Where(q => q.difficulty == selectedDifficulty).ToList();
                }

            // Get the selected number of questions
            var selectedItem = QuestionsComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
                {
                int selectedQuestionCount = int.Parse(selectedItem.Content.ToString());

                // If more questions are available than selected, shuffle and take the subset
                if (questions.Count > selectedQuestionCount)
                    {
                    ShuffleQuestions(questions);
                    questions = questions.Take(selectedQuestionCount).ToList();
                    }
                }

            // Display the total number of questions that will be used in the test
            TotalQuestionsTextBox.Text = questions.Count.ToString();
            }


        }
    }
