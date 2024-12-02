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
        public BrowseQuestions()
        {

            InitializeComponent();
            WindowState = WindowState.Maximized;
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
    }
}
