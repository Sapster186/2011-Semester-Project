using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    
    public class LearnerDA
    {
        // insert a new learner and return their unique id
        public int Insert(Learner l)
        {
            SqlConnection conn = DatabaseHelper.GetConnection();

            try
            {
                string sql = "INSERT INTO [dbo].[Learner] ([gradeLevelID],[name],[schoolName],[province],[contactDetails],[dateCreated]) " +
                             "VALUES (@grade,@name,@school,@province,@contact,@created); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@grade", l.gradeLevelID);
                cmd.Parameters.AddWithValue("@name", l.name);
                cmd.Parameters.AddWithValue("@school", l.schoolName);
                cmd.Parameters.AddWithValue("@province", l.province);
                cmd.Parameters.AddWithValue("@contact", string.IsNullOrWhiteSpace(l.contactDetails) ? (object)DBNull.Value : l.contactDetails);
                cmd.Parameters.AddWithValue("@created", l.dateCreated);

                conn.Open();
                return (int)cmd.ExecuteScalar();
            }
            finally
            {
                
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public Learner GetById(int learnerID)
        {
            SqlConnection conn = DatabaseHelper.GetConnection();
             
            // fetches learner based on unique idnumber
            try
            {
                string sql = "SELECT [learnerID],[gradeLevelID],[name],[schoolName],[province],[contactDetails],[dateCreated] FROM [dbo].[Learner] WHERE [learnerID]=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", learnerID);

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();

                if (!r.Read())
                {
                    r.Close();
                    return null;
                }

                Learner learner = new Learner(r.GetInt32(0), r.GetInt32(1), r.GetString(2), r.GetString(3), r.GetString(4),
                                              r.IsDBNull(5) ? null : r.GetString(5), r.GetDateTime(6));
                r.Close();
                return learner;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        // Deletes an unfinished record based on the ID
        public void Delete(int learnerID)
        {
            SqlConnection conn = DatabaseHelper.GetConnection();

            try
            {
                string sql = "DELETE FROM [dbo].[Learner] WHERE [learnerID]=@id";

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