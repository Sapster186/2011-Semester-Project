using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    public class AdviceSession
    {
        public int sessionID { get; set; }
        public int learnerID { get; set; }
        public int degreeID { get; set; }
        public DateTime dateOfSession { get; set; }
        public string eligibleResult { get; set; }
        public double overallScore { get; set; }

        public AdviceSession()
        {
            dateOfSession = DateTime.Now;
            eligibleResult = "";
            overallScore = 0;
        }

        public AdviceSession(int sessionID, int learnerID, int degreeID, DateTime dateOfSession,
                              string eligibleResult, double overallScore)
        {
            this.sessionID = sessionID;
            this.learnerID = learnerID;
            this.degreeID = degreeID;
            this.dateOfSession = dateOfSession;
            this.eligibleResult = eligibleResult;
            this.overallScore = overallScore;
        }
    }
}