using System.Windows;
using AdvanceQuizApp.Admin_Pages;
using AdvanceQuizApp.Map;
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
            Logins loginPanel = new Logins();
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
            this.Hide();
            Report report = new Report();
            report.Show();
        }

        private void ViewStatistics_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Statistics statistics = new Statistics();
            statistics.Show();
        }
        private void ManageUsers_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ManageUsers manageUsers = new ManageUsers();
            manageUsers.Show();
        }

        private void Map_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MapDisplay display = new MapDisplay();
            display.Show();
        }
    }
}
