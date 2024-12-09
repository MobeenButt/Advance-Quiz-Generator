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
            this.Hide();
            Window br = new Settings();
            br.Show();
            br.WindowState = WindowState.Maximized;
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

        private void Button_Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logging out...");
            this.Hide();
            Login loginPanel = new Login();
            loginPanel.Show();
        }
        private void CreateTest_Click(object sender, RoutedEventArgs e)
        {
            Window br = new CreateTest();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Visibility = Visibility.Hidden;
        }
        private void Button_SearchQuestions_Click(object sender, RoutedEventArgs e)
        {
            var blurEffect = new BlurEffect
            {
                Radius = 10
            };

            this.Effect = blurEffect;

            Searchwindow searchWindow = new Searchwindow
            {
                Owner = this
            };
            searchWindow.ShowDialog();

            this.Effect = null;
        }

    }

}    