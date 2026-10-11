using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    public class LearnerSubjectDA
    {
        // insert one subject and mark for a learner
        public void Insert(LearnerSubject ls)
        {
            SqlConnection conn = DatabaseHelper.GetConnection();

            try
            {
                string sql = "INSERT INTO [dbo].[LearnerSubject] ([learnerID],[subjectID],[mark]) VALUES (@learner,@subject,@mark)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@learner", ls.learnerID);
                cmd.Parameters.AddWithValue("@subject", ls.subjectID);
                cmd.Parameters.AddWithValue("@mark", ls.mark);

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

        // select and return all of a learner's subjects and marks based on their specific ID
        public List<LearnerSubject> GetByLearner(int learnerID)
        {
            List<LearnerSubject> list = new List<LearnerSubject>();
            SqlConnection conn = DatabaseHelper.GetConnection();

            try
            {
                string sql = "SELECT [learnerSubjectID],[learnerID],[subjectID],[mark] FROM [dbo].[LearnerSubject] WHERE [learnerID]=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", learnerID);

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    list.Add(new LearnerSubject(r.GetInt32(0), r.GetInt32(1), r.GetInt32(2), r.GetDouble(3)));
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

        //  Deletes an incomplete record of a learner based on ID
        public void DeleteByLearner(int learnerID)
        {
            SqlConnection conn = DatabaseHelper.GetConnection();

            try
            {
                string sql = "DELETE FROM [dbo].[LearnerSubject] WHERE [learnerID]=@id";

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