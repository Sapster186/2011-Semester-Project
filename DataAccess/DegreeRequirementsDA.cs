using System.Collections.Generic;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    public class DegreeRequirementsDA
    {
        // gets every requirement for a specific degree
        public List<DegreeRequirements> GetRequirementsByDegree(int degreeID)
        {
            List<DegreeRequirements> results = new List<DegreeRequirements>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM DegreeRequirements WHERE degreeID = @degreeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@degreeID", degreeID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DegreeRequirements r = new DegreeRequirements();
                    r.requirementsID = (int)reader["requirementsID"];
                    r.degreeID = (int)reader["degreeID"];
                    r.subjectID = (int)reader["subjectID"];
                    r.minMark = (double)reader["minMark"];
                    r.isNeeded = (bool)reader["isNeeded"];
                    r.isAlternative = (bool)reader["isAlternative"];
                    r.adviceNote = reader["adviceNote"].ToString();
                    results.Add(r);
                }
            }
            return results;
        }

        // gets every requirement row in the database (used by the engine when scanning all degrees)
        public List<DegreeRequirements> GetAllRequirements()
        {
            List<DegreeRequirements> results = new List<DegreeRequirements>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM DegreeRequirements";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DegreeRequirements r = new DegreeRequirements();
                    r.requirementsID = (int)reader["requirementsID"];
                    r.degreeID = (int)reader["degreeID"];
                    r.subjectID = (int)reader["subjectID"];
                    r.minMark = (double)reader["minMark"];
                    r.isNeeded = (bool)reader["isNeeded"];
                    r.isAlternative = (bool)reader["isAlternative"];
                    r.adviceNote = reader["adviceNote"].ToString();
                    results.Add(r);
                }
            }
            return results;
        }
    }
}