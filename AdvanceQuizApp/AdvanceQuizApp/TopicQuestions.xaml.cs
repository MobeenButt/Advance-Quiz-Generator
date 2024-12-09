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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdvanceQuizApp.DataBank;
using System.Text.Json;



namespace AdvanceQuizApp
{
    public partial class TopicQuestions : Window
    {
        private string topicName;
        private List<Question> questions;

        public TopicQuestions(string topic)
        {
            InitializeComponent();
            topicName = topic;
            HeaderText.Text = $"{topicName} - All Questions";
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            // Load JSON data
            string jsonData = File.ReadAllText("quizdata.json");
            var data = JsonSerializer.Deserialize<QuizData>(jsonData);


            // Filter questions by topic
            try
            {
                string filePath = "quizdata.json";
                questions = QuestionLoader.LoadQuestions(filePath)
                                          .Where(q => q.topic == topicName)
                                          .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading questions: {ex.Message}");
            }

            // Populate QuestionsPanel
            int index = 1;
            foreach (var question in questions)
            {
                StackPanel questionPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                TextBlock questionText = new TextBlock
                {
                    Text = $"{index}. {question.text}",
                    FontSize = 16,
                    VerticalAlignment = VerticalAlignment.Center,
                    Width = 500
                };
                Button favButton = new Button
                {
                    Content = "⭐",
                    Width = 40,
                    Margin = new Thickness(5)
                };
                favButton.Click += (s, e) => AddToFavourites(question);

                Button arrowButton = new Button
                {
                    Content = ">",
                    Width = 40,
                    Margin = new Thickness(5)
                };
                arrowButton.Click += (s, e) => ShowDetails(question);

                questionPanel.Children.Add(questionText);
                questionPanel.Children.Add(favButton);
                questionPanel.Children.Add(arrowButton);

                QuestionsPanel.Children.Add(questionPanel);
                index++;
            }
        }

        private void AddToFavourites(Question question)
        {
            question.favourite = 1;
            File.WriteAllText("quizdata.json", JsonSerializer.Serialize(new QuizData { Questions = questions }, new JsonSerializerOptions { WriteIndented = true }));
            MessageBox.Show("Question added to favourites.");
        }

        private void ShowDetails(Question question)
        {
            string details = $"Options:\n";
            for (int i = 0; i < question.options.Count; i++)
            {
                details += $"{i + 1}. {question.options[i]}\n";
            }
            details += $"\nAnswer: {question.correctAnswer}";
            MessageBox.Show(details, "Question Details");
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    
}

