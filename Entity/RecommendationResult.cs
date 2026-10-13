using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    public class RecommendationResult
    {
        public int recommendationID { get; set; }
        public int sessionID { get; set; }
        public int degreeID { get; set; }
        public double subjectMatchScore { get; set; }
        public double markMatchScore { get; set; }
        public double interestMatchScore { get; set; }
        public double desiredFacultyScore { get; set; }
        public double overallScore { get; set; }
        public bool isAlternative { get; set; }
        public string gapObservation { get; set; }

        public RecommendationResult()
        {
            subjectMatchScore = 0;
            markMatchScore = 0;
            interestMatchScore = 0;
            desiredFacultyScore = 0;
            overallScore = 0;
            isAlternative = false;
            gapObservation = "";
        }

        public RecommendationResult(int recommendationID, int sessionID, int degreeID,
                                     double subjectMatchScore, double markMatchScore,
                                     double interestMatchScore, double desiredFacultyScore,
                                     double overallScore, bool isAlternative, string gapObservation)
        {
            this.recommendationID = recommendationID;
            this.sessionID = sessionID;
            this.degreeID = degreeID;
            this.subjectMatchScore = subjectMatchScore;
            this.markMatchScore = markMatchScore;
            this.interestMatchScore = interestMatchScore;
            this.desiredFacultyScore = desiredFacultyScore;
            this.overallScore = overallScore;
            this.isAlternative = isAlternative;
            this.gapObservation = gapObservation;
        }
    }
}