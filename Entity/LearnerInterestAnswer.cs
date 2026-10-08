using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    public class LearnerInterestAnswer
    {
        public int answerID { get; set; }
        public int learnerID { get; set; }
        public int queryID { get; set; }
        public string answer { get; set; }

        public LearnerInterestAnswer()
        {
            answer = "";
        }

        public LearnerInterestAnswer(int answerID, int learnerID, int queryID, string answer)
        {
            this.answerID = answerID;
            this.learnerID = learnerID;
            this.queryID = queryID;
            this.answer = answer;
        }
    }
}
