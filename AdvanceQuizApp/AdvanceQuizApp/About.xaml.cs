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
using System.IO;
using System.Diagnostics;  


namespace AdvanceQuizApp
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
        {
        public About()
            {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            }

        private void BackButton_Click(object sender, RoutedEventArgs e)
            {
            this.Hide();
            MainWindow mw = new MainWindow();
            mw.Show();

            }
        private void ReportButton_Click(object sender, RoutedEventArgs e)
            {
            try
                {
                string reportPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FinalReport.pdf");

                if (System.IO.File.Exists(reportPath))
                    {
                    Process.Start(new ProcessStartInfo
                        {
                        FileName = reportPath,
                        UseShellExecute = true
                        });
                    }
                else
                    {
                    MessageBox.Show("Report file not found. Please make sure FinalReport.docx exists in the application directory.",
                        "Report Not Found", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            catch (Exception ex)
                {
                MessageBox.Show($"Error opening report: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            } 
        private void WindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
        }
    }
}
