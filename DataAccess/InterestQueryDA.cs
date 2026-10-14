using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    public class InterestQueryDA
    {
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
                    q.QueryID = (int)reader["queryID"];
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
                    q.QueryID = (int)reader["queryID"];
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

        public void UpdateQuery(InterestQuery q)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"UPDATE InterestQuery SET
                                 interestGroupID = @interestGroupID,
                                 questionText = @questionText,
                                 weight = @weight,
                                 isActive = @isActive
                                 WHERE queryID = @queryID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@queryID", q.QueryID);
                cmd.Parameters.AddWithValue("@interestGroupID", q.InterestGroupID);
                cmd.Parameters.AddWithValue("@questionText", q.QueryText);
                cmd.Parameters.AddWithValue("@weight", q.Weight);
                cmd.Parameters.AddWithValue("@isActive", q.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeactivateQuery(int queryID)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "UPDATE InterestQuery SET isActive = 0 WHERE queryID = @queryID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@queryID", queryID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
