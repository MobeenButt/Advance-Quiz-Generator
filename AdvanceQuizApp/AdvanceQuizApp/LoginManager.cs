using System.IO;
using AdvanceQuizApp.ADT;
namespace AdvanceQuizApp
{
    class LoginManager
    {
        private const string filePath = "user.txt";
        private Dictionary<string, string> credentials;
        private MyQueue<Tuple<string, string>> loginQueue;
        public LoginManager()
        {
            credentials = new Dictionary<string, string>();
            loginQueue = new MyQueue<Tuple<string, string>>();
            LoadCredentialsFromFile();

        }
        public bool RegisterUser(string name, string pass)
        {
            if (credentials.ContainsKey(name))
            {
                return false;
            }
            credentials[name] = pass;
            SaveToFile();
            return true;
        }
        public bool AddUser(string name, string pass)
        {
            loginQueue.Enqueue(new Tuple<string, string>(name, pass));
            return true;
        }
        public string ProcessLogin()
        {
            if (loginQueue.isEmpty())
            {
                return "No pending login requests.";
            }

            var loginRequest = loginQueue.Dequeue();
            string username = loginRequest.Item1;
            string password = loginRequest.Item2;

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
                foreach (var user in credentials)
                {
                    writer.WriteLine(user.Key + "," + user.Value);
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
                    if (parts.Length == 2)
                    {
                        credentials[parts[0]] = parts[1];
                    }
                }
            }
        }
        public bool ValidateUser(string name, string pass)
        {
            if (credentials.ContainsKey(name))
            {
                return credentials[name] == pass;
            }
            return false;
        }
    }
}
