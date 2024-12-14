using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace AdvanceQuizApp.Admin_Pages
{
    public partial class Report : Window
    {
        private List<UserData> _userData;

        public Report()
        {
            InitializeComponent();
            LoadUsersFromJson();
        }

        private void LoadUsersFromJson()
        {
            try
            {
                // Specify the path to the JSON file
                string jsonFilePath = "userData.json";

                // Read and deserialize JSON data
                string jsonContent = File.ReadAllText(jsonFilePath);
                _userData = JsonSerializer.Deserialize<List<UserData>>(jsonContent);

                // Populate ComboBox with user names
                foreach (var user in _userData)
                {
                    UserComboBox.Items.Add(user.UserName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error");
            }
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (UserComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a user to generate a report.", "Error");
                return;
            }

            string selectedUser = UserComboBox.SelectedItem.ToString();

            // Find the selected user data
            var user = _userData.Find(u => u.UserName == selectedUser);

            if (user != null && user.HasData)
            {
                MessageBox.Show($"Report generated for {user.UserName}.", "Report Generated");
                // Add logic to display or save the generated report
            }
            else
            {
                MessageBox.Show($"No data available for {selectedUser}.", "No Data");
            }
        }

        private void UserComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }

    public class UserData
    {
        public string UserName { get; set; }
        public bool HasData { get; set; }
    }
}
