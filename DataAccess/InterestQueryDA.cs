using _2011__Semester_Project.DataAccess;
using _2011__Semester_Project.Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.DataAccess
{
    // The ONLY class allowed to talk to the InterestQuery table directly.
    public class InterestQueryDA
    {
        // Gets every active question, ordered by their interest group.
        public List<InterestQuery> GetAllActiveQueries()
        {
            List<InterestQuery> results = new List<InterestQuery>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM InterestQuery WHERE isActive = 1 ORDER BY interestGroupID";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    InterestQuery q = new InterestQuery();
                    q.QueryID = (int)reader["interestQueryID"];
                    q.InterestGroupID = (int)reader["interestGroupID"];
                    q.QueryText = reader["questionText"].ToString();
                    q.Weight = Convert.ToSingle(reader["weight"]);
                    q.IsActive = (bool)reader["isActive"];
                    results.Add(q);
                }

                reader.Close();
            }

            return results;
        }

        // Gets ALL questions (active and inactive) — used by the admin form.
        public List<InterestQuery> GetAllQueries()
        {
            List<InterestQuery> results = new List<InterestQuery>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM InterestQuery";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    InterestQuery q = new InterestQuery();
                    q.QueryID = (int)reader["interestQueryID"];
                    q.InterestGroupID = (int)reader["interestGroupID"];
                    q.QueryText = reader["questionText"].ToString();
                    q.Weight = Convert.ToSingle(reader["weight"]);
                    q.IsActive = (bool)reader["isActive"];
                    results.Add(q);
                }

                reader.Close();
            }

            return results;
        }

        // Inserts a new interest question.
        public void AddQuery(InterestQuery q)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO InterestQuery (interestGroupID, questionText, weight, isActive)
                                 VALUES (@interestGroupID, @questionText, @weight, @isActive)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@interestGroupID", q.InterestGroupID);
                cmd.Parameters.AddWithValue("@questionText", q.QueryText);
                cmd.Parameters.AddWithValue("@weight", q.Weight);
                cmd.Parameters.AddWithValue("@isActive", q.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Updates an existing question.
        public void UpdateQuery(InterestQuery q)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"UPDATE InterestQuery SET
                                 interestGroupID = @interestGroupID,
                                 questionText = @questionText,
                                 weight = @weight,
                                 isActive = @isActive
                                 WHERE interestQueryID = @interestQueryID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@interestQueryID", q.QueryID);
                cmd.Parameters.AddWithValue("@interestGroupID", q.InterestGroupID);
                cmd.Parameters.AddWithValue("@questionText", q.QueryText);
                cmd.Parameters.AddWithValue("@weight", q.Weight);
                cmd.Parameters.AddWithValue("@isActive", q.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Deactivates a question instead of deleting it.
        public void DeactivateQuery(int queryID)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "UPDATE InterestQuery SET isActive = 0 WHERE interestQueryID = @queryID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@queryID", queryID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}