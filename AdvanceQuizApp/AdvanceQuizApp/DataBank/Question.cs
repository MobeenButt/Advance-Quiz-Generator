using System;
using System.IO;
using Newtonsoft.Json;
using System.Text.Json;
using System.Windows;

namespace AdvanceQuizApp
{
    public class Question
    {
        public int id { get; set; }
        public string text { get; set; }
        public List<string> options { get; set; }
        public string correctAnswer { get; set; }
        public string topic { get; set; }
        public string difficulty { get; set; }

        public int favourite { get; set; }
    }
    public class Topic
    {
        public string topicName { get; set; }
        public List<Question> questions { get; set; }
        public List<Topic> subtopics { get; set; }
        public Topic(string topicName)
        {
            this.topicName = topicName;
            questions = new List<Question>();
            subtopics = new List<Topic>();
        }
    }
    public class QuizData
    {
        public List<Question> Questions { get; set; }
    }
    public class QuestionLoader
    {
        public static List<Question> LoadQuestions(string filePath)
        {
            string jsonData = File.ReadAllText(filePath);
            var questions = JsonConvert.DeserializeObject<Dictionary<string, List<Question>>>(jsonData);
            return questions["questions"];
        }
    }
    public static class UserManager
    {

        public static string[] GetCurrentUser()
        {
            string currentUserFile = "CurrentUser.txt";

            if (!File.Exists(currentUserFile))
            {
                throw new FileNotFoundException("Current user file not found!");
            }

            string[] userDetails = File.ReadAllText(currentUserFile).Split(',');
            if (userDetails.Length < 3)
            {
                throw new FormatException("Invalid user details format! Ensure the file contains 'username,password'.");
            }

            return new string[] { userDetails[0], userDetails[1] }; // Return username and password
        }   
        public static string getCurrentUsername()
        {
            return GetCurrentUser()[0];
        }

        public static List<int> GetFavouriteQuestions(string username)
        {
            string filePath = "user.txt";
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');

                    if (parts.Length >= 3 && parts[0] == username)
                    {
                        // Handle the case where favorite questions might be missing or empty
                        if (parts.Length > 3 && !string.IsNullOrEmpty(parts[3]))
                        {
                            List<int> favouriteIds = new List<int>();

                            // Start from index 3 (where the question IDs begin) and iterate through all subsequent parts
                            for (int i = 3; i < parts.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(parts[i]))
                                {
                                    // Split by the separator (`) and add all IDs to the list
                                    favouriteIds.AddRange(parts[i].Split(',').Select(int.Parse));
                                }
                            }

                            return favouriteIds;
                        }
                        return new List<int>(); // No favorites if the field is empty
                    }
                }
            }
            return new List<int>(); // Return empty list if user not found
        }

        public static void RemoveFromFavourites(string username, int questionId)
        {
            // Load the user data
            string[] userLines = File.ReadAllLines("user.txt");
            List<string> updatedUserLines = new List<string>();

            foreach (var line in userLines)
            {
                string[] userDetails = line.Split(',');

                if (userDetails[0] == username) // If the username matches
                {
                    // Extract the current favorite question IDs and remove the specified ID
                    List<int> favouriteIds = userDetails.Length > 3
                        ? userDetails.Skip(3).Select(int.Parse).ToList()
                        : new List<int>();

                    favouriteIds.Remove(questionId); // Remove the question ID

                    // Rebuild the user line with updated favorite IDs
                    string updatedUserLine = $"{userDetails[0]},{userDetails[1]},{userDetails[2]},{string.Join(",", favouriteIds)}";
                    updatedUserLines.Add(updatedUserLine);
                }
                else
                {
                    updatedUserLines.Add(line); // Keep the unchanged user lines
                }
            }

            // Write the updated lines back to the file
            File.WriteAllLines("user.txt", updatedUserLines);
        }


        public static bool aRemoveFromFavourites(string username, int questionId)
        {
            string userFile = "user.txt";

            // Ensure the file exists
            if (!File.Exists(userFile))
            {
                MessageBox.Show("User file not found!");
                return false;
            }

            // Read all users from the file
            var allUsers = File.ReadAllLines(userFile).ToList();

            // Find the user's line
            int userIndex = allUsers.FindIndex(line => line.StartsWith($"{username},"));
            if (userIndex == -1)
            {
                MessageBox.Show("User not found in the user file!");
                return false;
            }

            // Parse the user details
            string[] userDetails = allUsers[userIndex].Split(',');
            if (userDetails.Length < 3)
            {
                MessageBox.Show("Invalid user data format!");
                return false;
            }

            // Extract favorite question IDs
            List<string> favouriteIds = userDetails.Length > 3 ? userDetails.Skip(3).Where(id => !string.IsNullOrWhiteSpace(id)).ToList() : new List<string>();

            // Remove the question ID if it exists
            if (!favouriteIds.Remove(questionId.ToString()))
            {
                MessageBox.Show("This question is not in your favourites!");
                return false;
            }

            // Prepare updated user details
            string updatedUserDetails = favouriteIds.Count > 0
                ? $"{userDetails[0]},{userDetails[1]},{userDetails[2]},{string.Join(",", favouriteIds)}"
                : $"{userDetails[0]},{userDetails[1]},{userDetails[2]}";

            // Update the user's line in the file
            allUsers[userIndex] = updatedUserDetails;

            // Write the updated user list back to the file
            File.WriteAllLines(userFile, allUsers);

            return true;
        }

    }





}
