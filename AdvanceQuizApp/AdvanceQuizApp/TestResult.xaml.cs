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
    /// Interaction logic for TestResult.xaml
    /// </summary>
    public partial class TestResult : Window
        {
        public int totalquestion;
        public int correctanswer;
        public TimeSpan elapsedtime;

        public TestResult(int totalquestions, int correctanswers, TimeSpan time)
            {
            InitializeComponent();
            this.totalquestion = totalquestions;
            this.correctanswer = correctanswers;
            this.elapsedtime = time;
            LoadResults();
            }
        private void LoadResults()
            {
            TotalQuestionsTextBlock.Text = $"Total Questions: {totalquestion}";
            CorrectAnswersTextBlock.Text = $"Correct Answers: {correctanswer}";
            TotalScoreTextBlock.Text = $"Total Score: {correctanswer}/{totalquestion}";
            ElapsedTimeTextBlock.Text = $"Time Taken: {elapsedtime.Minutes:D2}:{elapsedtime.Seconds:D2}";
            }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
            {
            MainWindow t = new MainWindow();
            t.Show();
            this.Close();

            }
        }
    }
