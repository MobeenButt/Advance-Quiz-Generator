using System.Windows;
using System.Windows.Media.Effects;

namespace AdvanceQuizApp
{
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

        

        private void Button_Settings_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Window br = new Settings();
            br.Show();
            br.WindowState = WindowState.Maximized;
        }

        private void Button_About_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            About about = new About();
            about.Show();
        }
        private void Button_BrowseQuestions_Click(object sender, RoutedEventArgs e)
        {
            Window br = new BrowseQuestions();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Visibility = Visibility.Hidden;
        }
        private void CreateQuiz_Click(object sender, RoutedEventArgs e)
        {
            Window br = new CreateQuiz();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Visibility = Visibility.Hidden;
        }

        private void Button_Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logging out...");
            this.Hide();
            Logins loginPanel = new Logins();
            loginPanel.Show();
        }
        
        private void Button_SavedQuestions_Click(Object sender, RoutedEventArgs e)
        {
            Window br = new PreviousQuizes();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Visibility = Visibility.Hidden;

        }

        private void Button_FavouriteQuestions_Click(object sender, RoutedEventArgs e)
        {
            Window br = new FavouriteQuestions(UserManager.getCurrentUsername());
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Close();
        }
    }

}    