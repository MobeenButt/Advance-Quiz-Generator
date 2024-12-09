using System.Windows;
using AdvanceQuizApp.Admin_Pages;
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
            Login loginPanel = new Login();
            loginPanel.Show();
        }
        private void EditQuiz_Click(object sender, RoutedEventArgs e)
        {

            this.Hide();
            EditQuizPage editQuizPage = new EditQuizPage();
            editQuizPage.Show();
        }

        private void DeleteQuiz_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            DeletePanel deletePanel = new DeletePanel();
            deletePanel.Show();
        }

        private void ViewCategories_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View Categories clicked!");
        }

        private void ViewAll_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View All Quizzes clicked!");
        }

        private void GenerateReports_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ViewStatistics_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("View Statistics clicked!");
        }
        private void Button_ManageQuestions_Click(object sender, RoutedEventArgs e)
        {
            Window br = new ManageQuestions();
            br.Show();
            br.WindowState = WindowState.Maximized;
            this.Visibility = Visibility.Hidden;
        }

    }
}
