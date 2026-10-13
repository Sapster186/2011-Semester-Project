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
    // The ONLY class allowed to talk to the InterestGroup table directly.
    public class InterestGroupDA
    {
        // Gets every interest group in the table.
        public List<InterestGroup> GetAllGroups()
        {
            List<InterestGroup> results = new List<InterestGroup>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM InterestGroup";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    InterestGroup group = new InterestGroup();
                    group.InterestGroupID = (int)reader["interestGroupID"];
                    group.GroupName = reader["groupName"].ToString();
                    group.Description = reader["description"].ToString();
                    results.Add(group);
                }

                reader.Close();
            }

            return results;
        }

        // Gets one group by its ID.
        public InterestGroup GetGroupByID(int interestGroupID)
        {
            InterestGroup group = null;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM InterestGroup WHERE interestGroupID = @interestGroupID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@interestGroupID", interestGroupID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    group = new InterestGroup();
                    group.InterestGroupID = (int)reader["interestGroupID"];
                    group.GroupName = reader["groupName"].ToString();
                    group.Description = reader["description"].ToString();
                }

                reader.Close();
            }

            return group;
        }

        // Inserts a brand new interest group.
        public void AddGroup(InterestGroup group)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO InterestGroup (groupName, description)
                                 VALUES (@groupName, @description)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@groupName", group.GroupName);
                cmd.Parameters.AddWithValue("@description", group.Description);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Updates an existing interest group.
        public void UpdateGroup(InterestGroup group)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"UPDATE InterestGroup SET
                                 groupName = @groupName,
                                 description = @description
                                 WHERE interestGroupID = @interestGroupID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@interestGroupID", group.InterestGroupID);
                cmd.Parameters.AddWithValue("@groupName", group.GroupName);
                cmd.Parameters.AddWithValue("@description", group.Description);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}