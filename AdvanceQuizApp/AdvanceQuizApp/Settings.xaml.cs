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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;


namespace AdvanceQuizApp
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }
        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Window br = new MainWindow();
            br.Show();
            br.WindowState = WindowState.Maximized;


        }
        private void ClearData(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("sai a");
        }

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            try
            {
                // Read the current user details
                string currentUserFile = "CurrentUser.txt";
                if (!File.Exists(currentUserFile))
                {
                    MessageBox.Show("Current user file not found!");
                    return;
                }

                string[] userDetails = File.ReadAllText(currentUserFile).Split(',');
                if (userDetails.Length < 3)
                {
                    MessageBox.Show("Invalid user details format! Ensure the file contains 'username,password,priority'.");
                    return;
                }

                string userName = userDetails[0]; // Username
                string userPassword = userDetails[1];

                string newPassword = newPassBox.Text;
                if (string.IsNullOrWhiteSpace(newPassword))
                {
                    MessageBox.Show("InvalidPassword try any other");
                }
                else
                {
                    LoginManager lt = new LoginManager();
                    lt.EditPassword(userName, newPassword);
                    lt.SaveToFile();
                    MessageBox.Show($"Passwrod for {userName} Changed Successfully, Login Again");
                    this.Close();
                    Window br = new Login();
                    br.Show();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
