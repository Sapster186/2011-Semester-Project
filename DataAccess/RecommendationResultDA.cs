using System.Collections.Generic;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    public class RecommendationResultDA
    {
        // Saves a recommendation result for a session
        public void AddRecommendation(RecommendationResult r)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO RecommendationResult
                                 (sessionID, degreeID, subjectMatchScore, markMatchScore, interestMatchScore,
                                  desiredFacultyScore, overallScore, isAlternative, gapObservation)
                                 VALUES (@sessionID, @degreeID, @subjectMatchScore, @markMatchScore, @interestMatchScore,
                                         @desiredFacultyScore, @overallScore, @isAlternative, @gapObservation)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sessionID", r.sessionID);
                cmd.Parameters.AddWithValue("@degreeID", r.degreeID);
                cmd.Parameters.AddWithValue("@subjectMatchScore", r.subjectMatchScore);
                cmd.Parameters.AddWithValue("@markMatchScore", r.markMatchScore);
                cmd.Parameters.AddWithValue("@interestMatchScore", r.interestMatchScore);
                cmd.Parameters.AddWithValue("@desiredFacultyScore", r.desiredFacultyScore);
                cmd.Parameters.AddWithValue("@overallScore", r.overallScore);
                cmd.Parameters.AddWithValue("@isAlternative", r.isAlternative);
                cmd.Parameters.AddWithValue("@gapObservation", r.gapObservation);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // gets all recommendations for a session
        public List<RecommendationResult> GetRecommendationsBySession(int sessionID)
        {
            List<RecommendationResult> results = new List<RecommendationResult>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM RecommendationResult WHERE sessionID = @sessionID ORDER BY overallScore DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sessionID", sessionID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    RecommendationResult r = new RecommendationResult();
                    r.recommendationID = (int)reader["recommendationID"];
                    r.sessionID = (int)reader["sessionID"];
                    r.degreeID = (int)reader["degreeID"];
                    r.subjectMatchScore = (double)reader["subjectMatchScore"];
                    r.markMatchScore = (double)reader["markMatchScore"];
                    r.interestMatchScore = (double)reader["interestMatchScore"];
                    r.desiredFacultyScore = (double)reader["desiredFacultyScore"];
                    r.overallScore = (double)reader["overallScore"];
                    r.isAlternative = (bool)reader["isAlternative"];
                    r.gapObservation = reader["gapObservation"].ToString();
                    results.Add(r);
                }
            }
            return results;
        }
    }
}