using System;
using System.IO;
using Newtonsoft.Json;
using System.Text.Json;

namespace AdvanceQuizApp.DataBank
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
}
