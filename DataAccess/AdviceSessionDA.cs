using System.Collections.Generic;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    public class AdviceSessionDA
    {
        // saves any new advice session and returns the new sessionID
        public int AddSession(AdviceSession session)
        {
            int newID = 0;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO AdviceSession (learnerID, degreeID, dateOfSession, eligibleResult, overallScore)
                                 VALUES (@learnerID, @degreeID, @dateOfSession, @eligibleResult, @overallScore);
                                 SELECT SCOPE_IDENTITY();";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@learnerID", session.learnerID);
                cmd.Parameters.AddWithValue("@degreeID", session.degreeID);
                cmd.Parameters.AddWithValue("@dateOfSession", session.dateOfSession);
                cmd.Parameters.AddWithValue("@eligibleResult", session.eligibleResult);
                cmd.Parameters.AddWithValue("@overallScore", session.overallScore);

                conn.Open();
                newID = System.Convert.ToInt32(cmd.ExecuteScalar());
            }
            return newID;
        }

        public AdviceSession GetSessionByID(int sessionID)
        {
            AdviceSession s = null;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM AdviceSession WHERE sessionID = @sessionID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sessionID", sessionID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    s = new AdviceSession();
                    s.sessionID = (int)reader["sessionID"];
                    s.learnerID = (int)reader["learnerID"];
                    s.degreeID = (int)reader["degreeID"];
                    s.dateOfSession = (System.DateTime)reader["dateOfSession"];
                    s.eligibleResult = reader["eligibleResult"].ToString();
                    s.overallScore = (double)reader["overallScore"];
                }
            }
            return s;
        }
    }
}