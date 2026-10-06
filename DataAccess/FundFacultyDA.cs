using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    public class FundFacultyDA
    {
        // Gets which faculties a specific funding option is linked to.
        public List<FundFaculty> GetFacultiesForFundOption(int fundID)
        {
            List<FundFaculty> results = new List<FundFaculty>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM FundFaculty WHERE fundID = @fundID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fundID", fundID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    FundFaculty link = new FundFaculty();
                    link.FundFacultyID = (int)reader["fundFacultyID"];
                    link.FundID = (int)reader["fundID"];
                    link.FacultyID = (int)reader["facultyID"];
                    results.Add(link);
                }
            }
            return results;
        }

        // Creates a new faculty link for a funding option.
        public void AddFundFaculty(int fundID, int facultyID)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "INSERT INTO FundFaculty (fundID, facultyID) VALUES (@fundID, @facultyID)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fundID", fundID);
                cmd.Parameters.AddWithValue("@facultyID", facultyID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
