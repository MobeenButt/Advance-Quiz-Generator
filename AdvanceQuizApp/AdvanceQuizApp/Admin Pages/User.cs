using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvanceQuizApp.Admin_Pages
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int Priority { get; set; }
        public List<int> FavoriteQuestions { get; set; } // Add this property

        public User(string username, string password, int priority)
        {
            Username = username;
            Password = password;
            Priority = priority;
            FavoriteQuestions = new List<int>();
        }

    }
}
