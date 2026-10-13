using System.Collections.Generic;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    public class FacultyDA
    {
        // Gets every faculty in the database
        public List<Faculty> GetAllFaculties()
        {
            List<Faculty> results = new List<Faculty>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM Faculty";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Faculty f = new Faculty();
                    f.facultyID = (int)reader["facultyID"];
                    f.facultyName = reader["facultyName"].ToString();
                    f.facultyDesc = reader["facultyDesc"].ToString();
                    results.Add(f);
                }
            }
            return results;
        }

        // gets one faculty by its ID
        public Faculty GetFacultyByID(int facultyID)
        {
            Faculty f = null;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM Faculty WHERE facultyID = @facultyID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@facultyID", facultyID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    f = new Faculty();
                    f.facultyID = (int)reader["facultyID"];
                    f.facultyName = reader["facultyName"].ToString();
                    f.facultyDesc = reader["facultyDesc"].ToString();
                }
            }
            return f;
        }
    }
}
