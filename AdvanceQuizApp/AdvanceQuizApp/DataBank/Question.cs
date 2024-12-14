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

            return new string[] { userDetails[0], userDetails[1] }; 
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
                        if (parts.Length > 3 && !string.IsNullOrEmpty(parts[3]))
                        {
                            List<int> favouriteIds = new List<int>();

                            for (int i = 3; i < parts.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(parts[i]))
                                {
                                    favouriteIds.AddRange(parts[i].Split(',').Select(int.Parse));
                                }
                            }

                            return favouriteIds;
                        }
                        return new List<int>(); // ye empty Rahe gi agr Nj i mili koi
                    }
                }
            }
            return new List<int>(); // ye bhi
        }

        public static void RemoveFromFavourites(string username, int questionId)
        {
            string[] userLines = File.ReadAllLines("user.txt");
            List<string> updatedUserLines = new List<string>();

            foreach (var line in userLines)
            {
                string[] userDetails = line.Split(',');

                if (userDetails[0] == username) 
                {
                    List<int> favouriteIds = userDetails.Length > 3
                        ? userDetails.Skip(3).Select(int.Parse).ToList()
                        : new List<int>();

                    favouriteIds.Remove(questionId); 

                    string updatedUserLine = $"{userDetails[0]},{userDetails[1]},{userDetails[2]},{string.Join(",", favouriteIds)}";
                    updatedUserLines.Add(updatedUserLine);
                }
                else
                {
                    updatedUserLines.Add(line); 
                }
            }
            File.WriteAllLines("user.txt", updatedUserLines);
        }


        public static bool aRemoveFromFavourites(string username, int questionId)
        {
            string userFile = "user.txt";

            
            if (!File.Exists(userFile))
            {
                MessageBox.Show("User file not found!");
                return false;
            }

            var allUsers = File.ReadAllLines(userFile).ToList();

            int userIndex = allUsers.FindIndex(line => line.StartsWith($"{username},"));
            if (userIndex == -1)
            {
                MessageBox.Show("User not found in the user file!");
                return false;
            }

            string[] userDetails = allUsers[userIndex].Split(',');
            if (userDetails.Length < 3)
            {
                MessageBox.Show("Invalid user data format!");
                return false;
            }

            List<string> favouriteIds = userDetails.Length > 3 ? userDetails.Skip(3).Where(id => !string.IsNullOrWhiteSpace(id)).ToList() : new List<string>();

            if (!favouriteIds.Remove(questionId.ToString()))
            {
                MessageBox.Show("This question is not in your favourites!");
                return false;
            }

            string updatedUserDetails = favouriteIds.Count > 0
                ? $"{userDetails[0]},{userDetails[1]},{userDetails[2]},{string.Join(",", favouriteIds)}"
                : $"{userDetails[0]},{userDetails[1]},{userDetails[2]}";

            allUsers[userIndex] = updatedUserDetails;

            File.WriteAllLines(userFile, allUsers);

            return true;
        }

    }





}
