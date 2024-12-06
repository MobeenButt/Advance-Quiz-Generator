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

namespace AdvanceQuizApp.Admin_Pages
{
    /// <summary>
    /// Interaction logic for ManageQuestionBankPage.xaml
    /// </summary>
    public partial class ManageQuestionBankPage : Window
    {
        public ManageQuestionBankPage()
        {
            InitializeComponent();
        }

        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            //implementation
        }

        private void DeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            //implementation
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
        }

        private void QuestionDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
