using System.Windows;
using System.Windows.Controls;

namespace AdvanceQuizApp
{
    /// <summary>
    /// Interaction logic for LoginPanel.xaml
    /// </summary>
    public partial class LoginPanel : Window
    {
        private LoginManager manager;
        public LoginPanel()
        {
            InitializeComponent();
            manager = new LoginManager();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string name = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            manager.AddUser(name, password);
            string result= manager.ProcessLogin();
            if(result== "1")
            {
                AdminPanel adminPanel = new AdminPanel();
                adminPanel.Show();
            }
            else if (result == "2")
            {
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
            if(manager.RegisterUser(name, password))
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
