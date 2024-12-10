using System.IO;
using System.Windows;
using System.Windows.Controls;
namespace AdvanceQuizApp
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private LoginManager manager;
        private int priority;
        public Login()
        {
            InitializeComponent();
            manager = new LoginManager();

        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string name = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            if (name=="admin")
            {
                priority = 1;
            }
            else
            {
                priority = 0;
            }
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
                string userData = $"{name},{password},{priority}";
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
            if (name == "admin")
            {
                priority = 1;
            }
            else
            {
                priority = 0;
            }
            if (manager.RegisterUser(name, password,priority))
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
