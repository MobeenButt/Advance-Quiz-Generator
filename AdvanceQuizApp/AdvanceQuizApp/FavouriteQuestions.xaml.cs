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
using System.IO;

namespace AdvanceQuizApp
{
    /// <summary>
    /// Interaction logic for FavouriteQuestions.xaml
    /// </summary>
    public partial class FavouriteQuestions : Window
    {
        private List<Question> favouriteQuestions;
        private string username;
        private string[] userDetails;
        private Question selectedQuestion;
        public FavouriteQuestions(string username)
        {
            InitializeComponent();

            userDetails = UserManager.GetCurrentUser();
            this.username = userDetails[0];


            List<int> favouriteIds = UserManager.GetFavouriteQuestions(username);
            favouriteQuestions = QuestionLoader.LoadQuestions("quizdata.json")
                                              .Where(q => favouriteIds.Contains(q.id))
                                              .ToList();


            if (favouriteQuestions.Count == 0)
            {
                MessageBox.Show("No favourite questions found for this user.");
            }

            QuestionList.ItemsSource = favouriteQuestions.Select(q => q.text).ToList();
            
        }

        

        private void QuestionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (QuestionList.SelectedIndex >= 0 && QuestionList.SelectedIndex < favouriteQuestions.Count)
            {
                selectedQuestion = favouriteQuestions[QuestionList.SelectedIndex];
                DisplayQuestionDetails(selectedQuestion);
                
                QuestionDetailsPanel.Visibility = Visibility.Visible;
                QuestionTopic.Text = $"Question Topic: {selectedQuestion.topic}";
                QuestionTopic.Visibility = Visibility.Visible;
            }
            else
            {
                QuestionDetailsPanel.Visibility = Visibility.Hidden;
            }
        }


        private void DisplayQuestionDetails(Question question)
        {
            QuestionText.Text = question.text;
            OptionsList.ItemsSource = question.options;
            CorrectAnswerText.Visibility = Visibility.Hidden; 
        }


        private void ShowAnswer_Click(object sender, RoutedEventArgs e)
        {
            CorrectAnswerText.Text = $"Correct Answer :- {selectedQuestion.correctAnswer}";
            CorrectAnswerText.Visibility = Visibility.Visible;
        }

        private void RemoveFromFavourites_Click(object sender, RoutedEventArgs e)
        {
            int index = QuestionList.SelectedIndex;
            if (index >= 0 && index < favouriteQuestions.Count)
            {
                Question selectedQuestion = favouriteQuestions[index];

                UserManager.RemoveFromFavourites(username, selectedQuestion.id);

                favouriteQuestions.Remove(selectedQuestion);

                QuestionList.ItemsSource = favouriteQuestions.Select(q => q.text).ToList();

                if (favouriteQuestions.Count == 0)
                {
                    QuestionDetailsPanel.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                MessageBox.Show("No question selected to remove!");
            }
        }

        private void GoHome_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Window ma = new MainWindow();
            ma.Show();
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
