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
    }
}
