using System.IO;
using System.Windows;
using AdvanceQuizApp.ADT;
namespace AdvanceQuizApp.Admin_Pages
{
    /// <summary>
    /// Interaction logic for ManageUsers.xaml
    /// </summary>
    public partial class ManageUsers : Window
    {
        private string path = "user.txt";
        private PriorityQueue<User> users;
        public ManageUsers()
        {
            InitializeComponent();
            users = new PriorityQueue<User>();
            LoadUsers();
            UsersDataGrid.ItemsSource = users.ToList();
        }
        private void LoadUsers()
        {
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if(parts.Length == 3)
                    {
                        string name= parts[0];
                        string password= parts[1];
                        int priority = int.Parse(parts[2]);
                        users.Enqueue(new User(name, password, priority),priority);
                    }
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            AdminPanel adminPanel=new AdminPanel();
            adminPanel.Show();
        }

        private void EditPassword_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = (User)UsersDataGrid.SelectedItem;
            if (selectedUser != null) 
            {
                string newPassword = NewPasswordTextBox.Text;
                selectedUser.Password = newPassword;
                SaveToFile();
                UsersDataGrid.Items.Refresh();
            }
        }
        private void SaveToFile()
        {
            using (StreamWriter writer = new StreamWriter(path, false)) // Overwrite file
            {
                foreach (var user in users.ToList())
                {
                    writer.WriteLine($"{user.Username},{user.Password},{user.Priority}");
                }
            }
        }
        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = (User)UsersDataGrid.SelectedItem;
            if (selectedUser != null)
            {
                var tempQueue = new PriorityQueue<User>(); 
                foreach (var user in users.ToList())
                {
                    if (user != selectedUser) 
                    {
                        tempQueue.Enqueue(user, user.Priority);
                    }
                }

                users = tempQueue; 
                SaveToFile(); 
                UsersDataGrid.ItemsSource = null;
                UsersDataGrid.ItemsSource = users.ToList();
            }
        }
    }
}
