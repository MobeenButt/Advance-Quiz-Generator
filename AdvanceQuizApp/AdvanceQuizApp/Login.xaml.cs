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
namespace AdvanceQuizApp
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private LoginManager manager;
        public Login()
            {
            InitializeComponent();
            manager = new LoginManager();

            }
        private void Login_Click(object sender, RoutedEventArgs e)
            {
            string name = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            manager.AddUser(name, password);
            string result = manager.ProcessLogin();
            if (result == "1")
                {
                this.Hide();
                AdminPanel adminPanel = new AdminPanel();
                adminPanel.Show();
                }
            else if (result == "2")
                {
                string filePath = "CurrentUser.txt";
                string userData = $"{name},{password}";
                File.WriteAllText(filePath, userData);
                this.Hide();
                MainWindow userPanel = new MainWindow();
                userPanel.Show();
                }
            else
                {
                MessageBox.Show(result, "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }

        private void Register_Click(object sender, RoutedEventArgs e)
            {
            string name = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            if (manager.RegisterUser(name, password))
                {
                MessageBox.Show("Registration successful.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            else
                {
                MessageBox.Show("Username already exists.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
