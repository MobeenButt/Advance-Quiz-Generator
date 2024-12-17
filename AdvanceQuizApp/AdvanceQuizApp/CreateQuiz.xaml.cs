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
using AdvanceQuizApp;

namespace AdvanceQuizApp
{
    public partial class CreateQuiz : Window
    {
        private TopicBST topicBST; 
        private List<string> topics; 

        public CreateQuiz()
        {
            InitializeComponent();
            topicBST = new TopicBST(); 
            LoadSubjects(); 
        }

        private void LoadSubjects()
        {
            string filePath = "quizdata.json"; 
            var questions = QuestionLoader.LoadQuestions(filePath);

            foreach (var question in questions)
            {
                if (!string.IsNullOrWhiteSpace(question.topic))
                    topicBST.Insert(question.topic);
            }

            topics = topicBST.GetSortedTopics();

            foreach (var topic in topics)
            {
                SubjectsListBox.Items.Add(new ListBoxItem { Content = topic });
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Window br = new MainWindow();
            br.Show();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedTopics = SubjectsListBox.SelectedItems.Cast<ListBoxItem>()
                                                             .Select(item => item.Content.ToString())
                                                             .ToList();
            if (QuizID.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Please add an id of the quiz first");
                return;
            }
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

            string selectedDifficulty = ((ComboBoxItem)DifficultyComboBox.SelectedItem)?.Content.ToString();

            if (!string.IsNullOrEmpty(selectedDifficulty))
            {
                questions = questions.Where(q => q.difficulty == selectedDifficulty).ToList();
            }

            var selectedItem = QuestionsComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                int selectedQuestionCount = int.Parse(selectedItem.Content.ToString());
                if (questions.Count > selectedQuestionCount)
                {
                    ShuffleQuestions(questions);
                    questions = questions.Take(selectedQuestionCount).ToList();
                }
            }
            string quizId = QuizID.Text.ToString();
            CreateTest testWindow = new CreateTest(questions, quizId);
            testWindow.Show();
            this.Close();
        }
        private void ShuffleQuestions(List<AdvanceQuizApp.Question> questions)
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
        private List<AdvanceQuizApp.Question> GetQuestionsForTopics(List<string> selectedTopics)
        {
            string filePath = "quizdata.json";
            var allQuestions = QuestionLoader.LoadQuestions(filePath);

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

            string selectedDifficulty = ((ComboBoxItem)DifficultyComboBox.SelectedItem)?.Content.ToString();

            if (!string.IsNullOrEmpty(selectedDifficulty))
            {
                questions = questions.Where(q => q.difficulty == selectedDifficulty).ToList();
            }

            var selectedItem = QuestionsComboBox.SelectedItem as ComboBoxItem;
            if (selectedItem != null)
            {
                int selectedQuestionCount = int.Parse(selectedItem.Content.ToString());

                if (questions.Count > selectedQuestionCount)
                {
                    ShuffleQuestions(questions);
                    questions = questions.Take(selectedQuestionCount).ToList();
                }
            }

            TotalQuestionsTextBox.Text = questions.Count.ToString();
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
