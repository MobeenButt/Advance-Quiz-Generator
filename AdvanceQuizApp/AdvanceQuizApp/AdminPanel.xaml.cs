using System.Windows;
using System.Windows.Controls;
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
            LoginPanel loginPanel = new LoginPanel();
            loginPanel.Show();
        }
        private void EditQuiz_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Edit Quiz clicked!");
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

        private void ManageQuestionBank_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ManageQuestionBankPage manageQuestionBankPage = new ManageQuestionBankPage();
            manageQuestionBankPage.Show();

        }

        private void GenerateReports_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ViewStatistics_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("View Statistics clicked!");
        }
    }
}
