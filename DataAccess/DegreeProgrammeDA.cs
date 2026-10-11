using System.Collections.Generic;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    public class DegreeProgrammeDA
    {
        // gets every active degree
        public List<DegreeProgramme> GetAllActiveDegrees()
        {
            List<DegreeProgramme> results = new List<DegreeProgramme>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM DegreeProgramme WHERE isActive = 1";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DegreeProgramme d = new DegreeProgramme();
                    d.degreeID = (int)reader["degreeID"];
                    d.facultyID = (int)reader["facultyID"];
                    d.degreeName = reader["degreeName"].ToString();
                    d.minOverallAverage = (double)reader["minOverallAverage"];
                    d.isActive = (bool)reader["isActive"];
                    results.Add(d);
                }
            }
            return results;
        }

        // Gets every degree under a specific faculty
        public List<DegreeProgramme> GetDegreesByFaculty(int facultyID)
        {
            List<DegreeProgramme> results = new List<DegreeProgramme>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM DegreeProgramme WHERE facultyID = @facultyID AND isActive = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@facultyID", facultyID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    DegreeProgramme d = new DegreeProgramme();
                    d.degreeID = (int)reader["degreeID"];
                    d.facultyID = (int)reader["facultyID"];
                    d.degreeName = reader["degreeName"].ToString();
                    d.minOverallAverage = (double)reader["minOverallAverage"];
                    d.isActive = (bool)reader["isActive"];
                    results.Add(d);
                }
            }
            return results;
        }

        // gets one degree by its ID
        public DegreeProgramme GetDegreeByID(int degreeID)
        {
            DegreeProgramme d = null;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM DegreeProgramme WHERE degreeID = @degreeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@degreeID", degreeID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    d = new DegreeProgramme();
                    d.degreeID = (int)reader["degreeID"];
                    d.facultyID = (int)reader["facultyID"];
                    d.degreeName = reader["degreeName"].ToString();
                    d.minOverallAverage = (double)reader["minOverallAverage"];
                    d.isActive = (bool)reader["isActive"];
                }
            }
            return d;
        }
    }
}