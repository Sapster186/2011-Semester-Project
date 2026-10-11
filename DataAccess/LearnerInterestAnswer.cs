using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    public class LearnerInterestAnswerDA
    {
        // insert learner answers
        public void Insert(LearnerInterestAnswer a)
        {
            SqlConnection conn = DatabaseHelper.GetConnection();

            try
            {
                string sql = "INSERT INTO [dbo].[LearnerInterestAnswer] ([learnerID],[queryID],[answer]) VALUES (@learner,@query,@answer)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@learner", a.learnerID);
                cmd.Parameters.AddWithValue("@query", a.queryID);
                cmd.Parameters.AddWithValue("@answer", a.answer);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // selects and returns all learner answers
        public List<LearnerInterestAnswer> GetByLearner(int learnerID)
        {
            List<LearnerInterestAnswer> list = new List<LearnerInterestAnswer>();
            SqlConnection conn = DatabaseHelper.GetConnection();

            try
            {
                string sql = "SELECT [answerID],[learnerID],[queryID],[answer] FROM [dbo].[LearnerInterestAnswer] WHERE [learnerID]=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", learnerID);

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    list.Add(new LearnerInterestAnswer(r.GetInt32(0), r.GetInt32(1), r.GetInt32(2), r.GetString(3)));
                }
                r.Close();
                return list;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Undoes and deletes a record of an incomplete record
        public void DeleteByLearner(int learnerID)
        {
            SqlConnection conn = DatabaseHelper.GetConnection();

            try
            {
                string sql = "DELETE FROM [dbo].[LearnerInterestAnswer] WHERE [learnerID]=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", learnerID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
    }
}