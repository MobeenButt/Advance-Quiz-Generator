using AdvanceQuizApp.ADT;
using AdvanceQuizApp.DataBank;
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

namespace AdvanceQuizApp
{
    /// <summary>
    /// Interaction logic for BrowseQuestions.xaml
    /// </summary>
    public partial class BrowseQuestions
    {
        private TopicBST topicBST;
        private List<string> topics;

        public BrowseQuestions()
        {
            InitializeComponent();
            topicBST = new TopicBST();
            LoadTopics();
            GenerateTopicButtons();
        }

        private void LoadTopics()
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

            topics = topicBST.GetSortedTopics();
            
        }

        private void GenerateTopicButtons()
        {
            StackPanel topicPanel = new StackPanel();
            foreach (var topic in topics)
            {
                Button topicButton = new Button
                {
                    Content = topic,
                    Style = this.Resources["SidebarButtonStyle"] as Style
                };
                topicButton.Click += TopicButton_Click;
                topicPanel.Children.Add(topicButton);
            }

            // Add buttons to the ScrollViewer
            ScrollViewer scrollViewer = this.FindName("scrollViewer") as ScrollViewer;
            scrollViewer.Content = topicPanel;
        }

        private void TopicButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            MessageBox.Show($"You clicked on topic: {clickedButton.Content}");
        }

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Button_CreateQuiz_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Navigating to Create Quiz page...");
        }

        private void Button_ManageQuizzes_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Navigating to Manage Quizzes page...");
        }

        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Navigating to Settings page...");
        }

        private void Button_About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Navigating to About page...");
        }
        private void Button_BrowseQuestions_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            TextBox searchBox = this.FindName("searchBox") as TextBox;
            string searchTopic = searchBox.Text;

            if (topicBST.Search(searchTopic))
                MessageBox.Show($"Topic '{searchTopic}' found!");
            else
                MessageBox.Show($"Topic '{searchTopic}' not found!");
        }
    }
}
