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
    /// Interaction logic for DeletePanel.xaml
    /// </summary>
    public partial class DeletePanel : Window
    {
        public DeletePanel()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminPanel admin = new AdminPanel();
            admin.Show();
        }

        private void QuizDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DeleteQuiz_Click(object sender, RoutedEventArgs e)
        {
            //implemtation to delete quiz
        }
    }
}
