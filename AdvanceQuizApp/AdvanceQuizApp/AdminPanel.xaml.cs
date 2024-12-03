using System.Windows;

namespace AdvanceQuizApp
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logging out...");
            this.Hide();
            LoginPanel loginPanel = new LoginPanel();
            loginPanel.Show();
        }

        private void CreateQuiz_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Create Quiz clicked!");
        }

        private void EditQuiz_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edit Quiz clicked!");
        }

        private void DeleteQuiz_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Delete Quiz clicked!");
        }

        private void ViewCategories_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View Categories clicked!");
        }

        private void ViewAll_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View All Quizzes clicked!");
        }
        private void Button_ManageQuestions_Click(object sender, RoutedEventArgs e)
        {
            Window br = new ManageQuestions();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Visibility = Visibility.Hidden;
        }
        private void Button_AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("adding qe...");
        }
        private void Button_AddTopic_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("adding top...");
        }
    }
}
