using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    public class FundProgrammeDA
    {
        // Gets which degree programmes a specific funding option is linked to.
        public List<FundProgramme> GetProgrammesForFundOption(int fundID)
        {
            List<FundProgramme> results = new List<FundProgramme>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM FundProgramme WHERE fundID = @fundID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fundID", fundID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    FundProgramme link = new FundProgramme();
                    link.FundProgrammeID = (int)reader["fundProgrammeID"];
                    link.FundID = (int)reader["fundID"];
                    link.DegreeID = (int)reader["degreeID"];
                    results.Add(link);
                }
            }
            return results;
        }

        public void AddFundProgramme(int fundID, int degreeID)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "INSERT INTO FundProgramme (fundID, degreeID) VALUES (@fundID, @degreeID)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fundID", fundID);
                cmd.Parameters.AddWithValue("@degreeID", degreeID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}