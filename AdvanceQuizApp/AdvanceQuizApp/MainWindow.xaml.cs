using System.Windows;
using System.Windows.Media.Effects;

namespace AdvanceQuizApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
            Window br = new BrowseQuestions();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Visibility = Visibility.Hidden;
        }
        private void Button_ManageQuestions_Click(object sender, RoutedEventArgs e)
        {
            Window br = new ManageQuestions();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Visibility = Visibility.Hidden;
        }

        private void Button_Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logging out...");
            this.Hide();
            LoginPanel loginPanel = new LoginPanel();
            loginPanel.Show();
        }
        
    }
}