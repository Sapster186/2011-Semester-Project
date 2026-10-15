using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    // Selects all grade, subkect and interest questions
    public class LookupDA
    {
        public List<GradeLevel> GetGrades()
        {
            List<GradeLevel> list = new List<GradeLevel>();
            SqlConnection conn = DatabaseHelper.GetConnection();

            try
            {
                string sql = "SELECT [gradeLevelID],[gradeName] FROM [dbo].[GradeLevel] ORDER BY [gradeLevelID]";

                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    list.Add(new GradeLevel(r.GetInt32(0), r.GetString(1)));
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

        public List<Subject> GetSubjects()
        {
            List<Subject> list = new List<Subject>();
            SqlConnection conn = DatabaseHelper.GetConnection();

            try
            {
                string sql = "SELECT [subjectID],[subjectName],[isMaths],[isMathsLit],[isScience] FROM [dbo].[Subject] ORDER BY [subjectName]";

                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    list.Add(new Subject(r.GetInt32(0), r.GetString(1), r.GetBoolean(2), r.GetBoolean(3), r.GetBoolean(4)));
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

        public List<InterestQuery> GetActiveQuestions()
        {
            List<InterestQuery> list = new List<InterestQuery>();
            SqlConnection conn = DatabaseHelper.GetConnection();

            try
            {
                string sql = "SELECT [interestQueryID],[interestGroupID],[queryText],[weight] FROM [dbo].[InterestQuery] WHERE [isActive]=1 ORDER BY [interestQueryID]";
                SqlCommand cmd = new SqlCommand(sql, conn);

                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();

                while (r.Read())
                {
                    InterestQuery q = new InterestQuery();
                    q.QueryID = r.GetInt32(0);
                    q.InterestGroupID = r.GetInt32(1);
                    q.QueryText = r.GetString(2);
                    q.Weight = (float)r.GetDouble(3);
                    list.Add(q);
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
    }
}