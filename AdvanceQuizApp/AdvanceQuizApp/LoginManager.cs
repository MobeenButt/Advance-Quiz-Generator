﻿using System.IO;
using AdvanceQuizApp.ADT;
using AdvanceQuizApp.Admin_Pages;
namespace AdvanceQuizApp
{
    class LoginManager
    {
        private const string filePath = "user.txt";
        private Dictionary<string, User> users;
        private MyQueue<User> loginQueue;
        public LoginManager()
        {
            users = new Dictionary<string, User>();
            loginQueue = new MyQueue<User>();
            LoadCredentialsFromFile();

        }
        public bool RegisterUser(string name, string pass,int priority)
        {
            if (users.ContainsKey(name))
            {
                return false;
            }
            if(name=="admin")
            {
                priority = 1;   
            }
            users[name]= new User(name, pass, priority);
            SaveToFile();
            return true;
        }
        public bool AddUser(string name, string pass)
        {
            //baki sb ki priority 0 hogi
            loginQueue.Enqueue(new User(name, pass, 0));
            SaveToFile();
            return true;
        }
        public string ProcessLogin()
        {
            if (loginQueue.isEmpty())
            {
                return "No pending login requests.";
            }

            var loginRequest = loginQueue.Dequeue();
            string username = loginRequest.Username;
            string password = loginRequest.Password;

            if (ValidateUser(username, password))
            {
                if (username == "admin" && password == "admin123")
                {
                    return "1";
                }
                else
                {
                    return "2";
                }
            }
            else
            {
                return "Invalid username or password.";
            }
        }
        private void SaveToFile()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var user in users)
                {
                    writer.WriteLine(user.Key + "," + user.Value.Password + "," + user.Value.Priority);
                }
            }
        }
        private void LoadCredentialsFromFile()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                      string name= parts[0];
                        string pass = parts[1];
                        int priority = int.Parse(parts[2]);
                        users[name] = new User(name, pass, priority);
                    }
                }
            }
        }
        public bool ValidateUser(string name, string pass)
        {
            if (users.ContainsKey(name))
            {
                return users[name].Password == pass;
            }
            return false;
        }
    }
}
