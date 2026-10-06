using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    // This is the ONLY class allowed to talk to the FundOption table directly.
    public class FundOptionDA
    {
        // Gets every funding option that is active AND not past its closing date.
        public List<FundOption> GetActiveFundOptions()
        {
            List<FundOption> results = new List<FundOption>();

            using (SqlConnection conn = DatabaseHelper.GetConnection()) // "using" auto-closes the connection when done
            {
                string query = "SELECT * FROM FundOption WHERE isActive = 1 AND closeDate >= GETDATE()";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader(); // reads rows back one at a time

                while (reader.Read())
                {
                    FundOption option = new FundOption();
                    option.FundID = (int)reader["fundID"];
                    option.FundName = reader["fundName"].ToString();
                    option.FundType = reader["fundType"].ToString();
                    option.EligibleSummary = reader["eligibleSummary"].ToString();
                    option.NecessaryDocs = reader["necessaryDocs"].ToString();
                    option.CloseDate = (DateTime)reader["closeDate"];
                    option.ContactInformation = reader["contactInformation"].ToString();
                    option.IsActive = (bool)reader["isActive"];
                    results.Add(option);
                }
            }

            return results;
        }

        // Adds a brand new funding option (used later by Member C's admin form).
        public void AddFundOption(FundOption option)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO FundOption 
                    (fundName, fundType, eligibleSummary, necessaryDocs, closeDate, contactInformation, isActive)
                    VALUES (@fundName, @fundType, @eligibleSummary, @necessaryDocs, @closeDate, @contactInformation, @isActive)";

                SqlCommand cmd = new SqlCommand(query, conn);
                // Parameters (the @names) keep this safe from SQL injection - never glue SQL strings together manually.
                cmd.Parameters.AddWithValue("@fundName", option.FundName);
                cmd.Parameters.AddWithValue("@fundType", option.FundType);
                cmd.Parameters.AddWithValue("@eligibleSummary", option.EligibleSummary);
                cmd.Parameters.AddWithValue("@necessaryDocs", option.NecessaryDocs);
                cmd.Parameters.AddWithValue("@closeDate", option.CloseDate);
                cmd.Parameters.AddWithValue("@contactInformation", option.ContactInformation);
                cmd.Parameters.AddWithValue("@isActive", option.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery(); // runs an INSERT/UPDATE/DELETE (no rows returned)
            }
        }

        // Edits an existing funding option.
        public void UpdateFundOption(FundOption option)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"UPDATE FundOption SET 
                    fundName = @fundName, fundType = @fundType, eligibleSummary = @eligibleSummary, 
                    necessaryDocs = @necessaryDocs, closeDate = @closeDate, 
                    contactInformation = @contactInformation, isActive = @isActive
                    WHERE fundID = @fundID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fundID", option.FundID);
                cmd.Parameters.AddWithValue("@fundName", option.FundName);
                cmd.Parameters.AddWithValue("@fundType", option.FundType);
                cmd.Parameters.AddWithValue("@eligibleSummary", option.EligibleSummary);
                cmd.Parameters.AddWithValue("@necessaryDocs", option.NecessaryDocs);
                cmd.Parameters.AddWithValue("@closeDate", option.CloseDate);
                cmd.Parameters.AddWithValue("@contactInformation", option.ContactInformation);
                cmd.Parameters.AddWithValue("@isActive", option.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Deactivates instead of deleting, so history is kept (per your Part 1 spec).
        public void DeactivateFundOption(int fundID)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "UPDATE FundOption SET isActive = 0 WHERE fundID = @fundID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@fundID", fundID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

