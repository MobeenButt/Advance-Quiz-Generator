using AdvanceQuizApp.ADT;
using AdvanceQuizApp;
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
using Newtonsoft.Json;
using System.IO;

namespace AdvanceQuizApp
{
    /// <summary>
    /// Interaction logic for BrowseQuestions.xaml
    /// </summary>
    public partial class BrowseQuestions
    {
        private TopicBST topicBST;
        private List<string> topics;
        private string topicName;
        private List<Question> questions;

        public BrowseQuestions()
        {
            InitializeComponent();
            topicBST = new TopicBST();
            LoadTopics();
            GenerateTopicButtonsInGrid();
        }

        private void LoadTopics()
        {
            string filePath = "quizdata.json"; 
            var questions = QuestionLoader.LoadQuestions(filePath);

            // Add topics to the BST
            foreach (var question in questions)
            {
                if (!string.IsNullOrWhiteSpace(question.topic))
                    topicBST.Insert(question.topic);
            }

            topics = topicBST.GetSortedTopics();
            
        }

        private void GenerateTopicButtonsInGrid()
        {
            StackPanel topicPanel = new StackPanel();
            foreach (var topic in topics)
            {
                Button topicButton = new Button
                {
                    Content = topic,
                    HorizontalAlignment = HorizontalAlignment.Stretch, 
                    Style = this.Resources["SidebarButtonStyle"] as Style
                };
                topicButton.Click += TopicButton_Click;
                topicPanel.Children.Add(topicButton);
            }

            ScrollViewer scrollViewer = this.FindName("scrollViewer") as ScrollViewer;
            if (scrollViewer != null)
            {
                scrollViewer.Content = topicPanel;
            }
        }


        private void TopicButton_Click(object sender, RoutedEventArgs e)
        {
            Button topicButton = sender as Button;
            string selectedTopic = topicButton.Content.ToString();
            topicName = selectedTopic;

            string filePath = "quizdata.json";
            questions = QuestionLoader.LoadQuestions(filePath)
                                      .Where(q => q.topic == selectedTopic)
                                      .ToList();

            ShowQuestionsRightPanel(questions);
        }

        private void ShowQuestionsRightPanel(List<Question> questions)
        {
            StackPanel rightPanel = this.FindName("QuestionsPanel") as StackPanel;
            rightPanel.Children.Clear();
            TextBlock TopicQuestion = new TextBlock
            {
                Text = $"Questions of \"{topicName}\"",
                FontSize = 18,
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(30,5,0,0),
                Visibility = Visibility.Visible
            };
            rightPanel.Children.Add(TopicQuestion);

            int index = 1;
            foreach (var question in questions)
            {
                Border questionBorder = new Border
                {
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(189, 195, 199)),
                    Margin = new Thickness(0, 0, 0, 20),
                    Padding = new Thickness(10)
                };

                StackPanel questionContent = new StackPanel();

                // Question ki statement
                TextBlock questionText = new TextBlock
                {
                    Text = $"Q#{index}: {question.text}",
                    FontWeight = FontWeights.Bold,
                    FontSize = 14,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 10)
                };

                Button favButton = new Button
                {
                    Content = " Add to ♡ ",
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Style = this.Resources["SidebarButtonStyle"] as Style,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                favButton.Click += (s, e) => AddToFavourites(UserManager.getCurrentUsername(), question.id);
                
               
                TextBlock optionsLabel = new TextBlock
                {
                    Text = "Options:",
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 5)
                };

                
                StackPanel optionsPanel = new StackPanel();
                for (int i = 0; i < question.options.Count; i++)
                {
                    TextBlock optionText = new TextBlock
                    {
                        Text = $"{i + 1}. {question.options[i]}",
                        Margin = new Thickness(10, 0, 0, 0)
                    };
                    optionsPanel.Children.Add(optionText);
                }

                Button showAnswerButton = new Button
                {
                    Content = "Show Answer",
                    Margin = new Thickness(0, 5, 0, 0),

                    HorizontalAlignment = HorizontalAlignment.Left
                };
                showAnswerButton.Click += (s, e) => ShowAnswer(question.correctAnswer); //ye function khali rkha hai ta k hidden e rahy tab tak

                TextBlock answerText = new TextBlock
                {
                    Text = $"Answer: {question.correctAnswer}",
                    Foreground = new SolidColorBrush(Colors.Green),
                    Visibility = Visibility.Collapsed,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(10, 10, 0, 0),
                    Name = $"Answer_{index}"
                };

                showAnswerButton.Click += (s, e) =>
                {
                    answerText.Visibility = Visibility.Visible;
                };

                questionContent.Children.Add(questionText);
                questionContent.Children.Add(favButton);
                questionContent.Children.Add(optionsLabel);
                questionContent.Children.Add(optionsPanel);
                questionContent.Children.Add(showAnswerButton);
                questionContent.Children.Add(answerText);

                questionBorder.Child = questionContent;
                rightPanel.Children.Add(questionBorder);

                index++;
            }
        }

        private void ShowAnswer(string correctAnswer)
        {
            //jab wo ckick kry ga tab ussy nechy waala code run ho ga
        }
        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            Window br = new Settings();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Close();
        }

        private void Button_About_Click(object sender, RoutedEventArgs e)
        {
            Window br = new About();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Close();
        }
        private void Button_BrowseQuestions_Click(object sender, RoutedEventArgs e)
        {
            Window br = new BrowseQuestions();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Close();
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

            if (topicBST.Search(searchTopic)) //exact topic k lia
            {
                string filePath = "quizdata.json";
                questions = QuestionLoader.LoadQuestions(filePath)
                                          .Where(q => q.topic.Equals(searchTopic, StringComparison.OrdinalIgnoreCase))
                                          .ToList();
                topicName = searchTopic;
                ShowQuestionsRightPanel(questions);
            }
            else
            {
                //exact nh milaa to kareeb waly is me ay gen
                var closestMatches = topicBST.FindClosestMatches(searchTopic);

                if (closestMatches.Count > 0)
                { 
                    string message = $"Exact topic not found! Did you mean:\n" +
                                     string.Join("\n", closestMatches);
                    MessageBox.Show(message);

                    
                    string filePath = "quizdata.json";
                    topicName = closestMatches[0];  //sb se pehla closest topic ban jai ga
                    questions = QuestionLoader.LoadQuestions(filePath)
                                              .Where(q => closestMatches.Contains(q.topic))
                                              .ToList();
                    ShowQuestionsRightPanel(questions);
                }
                else
                {
                    MessageBox.Show($"No topics found related to '{searchTopic}'!");
                }
            }
        }


        private void AddToFavourites(string username,int questionId)
        {
            string userFile = "user.txt";

            if (!File.Exists(userFile))
            {
                MessageBox.Show("User file not found!");
                return;
            }

            var allUsers = File.ReadAllLines(userFile).ToList();

            int userIndex = allUsers.FindIndex(line => line.StartsWith($"{username},"));
            if (userIndex == -1)
            {
                MessageBox.Show("User not found in the user file!");
                return;
            }

            string[] userDetails = allUsers[userIndex].Split(',');
            List<string> favouriteIds = userDetails.Length > 3 ? userDetails.Skip(3).Where(id => !string.IsNullOrWhiteSpace(id)).ToList() : new List<string>();

            if (!favouriteIds.Contains(questionId.ToString()))
            {
                favouriteIds.Add(questionId.ToString());

                string updatedUserDetails = $"{userDetails[0]},{userDetails[1]},{userDetails[2]},{string.Join(",", favouriteIds)}";
                allUsers[userIndex] = updatedUserDetails;

                File.WriteAllLines(userFile, allUsers);

                MessageBox.Show("Question added to favourites.");
                
            }
            else
            {
                MessageBox.Show("This question is already in favourites.");
            }
        }

        private void Button_FavouriteQuestions_Click(object sender, RoutedEventArgs e)
        {
            Window br = new FavouriteQuestions(UserManager.getCurrentUsername());
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Close();
        }

        private void Button_Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logging out...");
            this.Hide();
            Logins loginPanel = new Logins();
            loginPanel.Show();
        }

        private void CreateQuiz_Click(object sender, RoutedEventArgs e)
        {
            Window br = new CreateQuiz();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Close();
        }

        private void Button_SavedQuizes_Click(object sender, RoutedEventArgs e)
        {
            Window br = new PreviousQuizes();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Close();
        }

        private void Button_SavedQuestions_Click(object sender, RoutedEventArgs e)
        {
            Window br = new PreviousQuizes();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Close();
        }
    }
}
