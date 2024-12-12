using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvanceQuizApp
    {
    public class QuizDataa
        {
        public int QuizId { get; set; }  // Add this property
        public List<Question> Questions { get; set; }
        }
    }
