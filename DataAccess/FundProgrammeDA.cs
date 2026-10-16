using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    // FundProgrammeDA — data access for the FundProgramme link table.
    //
    // FundProgramme is one of the two funding link tables (the other is
    // FundFaculty). One row means "this funding option applies to this
    // specific degree". A funding option can be linked to a faculty
    // (FundFaculty), a degree (FundProgramme), or both — see the ERD.
    //
    // Used by ReportDA.GetOpenFunding to decide which funding options
    // should appear for a chosen degree (or for a faculty that contains it).
    public class FundProgrammeDA
    {
        // Returns the links for one funding option — i.e. which degrees it
        // applies to. Called when opening/editing a funding option.
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

        // Creates a link between a funding option and a degree.
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