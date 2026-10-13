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
    // The ONLY class allowed to talk to the UserAccount table directly.
    // The LoginController calls these methods — never runs SQL itself.
    public class UserAccountDA
    {
        // Looks up one user by their username.
        // Returns null if no active user with that username exists.
        public UserAccount GetByUsername(string userName)
        {
            UserAccount user = null;

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM UserAccount WHERE userName = @userName AND isActive = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userName", userName);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new UserAccount();
                    user.UserAccountID = (int)reader["userAccountID"];
                    user.UserName = reader["userName"].ToString();
                    user.PassHash = reader["passHash"].ToString();
                    user.Role = reader["role"].ToString();
                    user.IsActive = (bool)reader["isActive"];
                }

                reader.Close();
            }

            return user;
        }

        // Gets every user in the table (active and inactive).
        // Used by the admin reference data maintenance screen.
        public List<UserAccount> GetAllUsers()
        {
            List<UserAccount> results = new List<UserAccount>();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "SELECT * FROM UserAccount";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UserAccount user = new UserAccount();
                    user.UserAccountID = (int)reader["userAccountID"];
                    user.UserName = reader["userName"].ToString();
                    user.PassHash = reader["passHash"].ToString();
                    user.Role = reader["role"].ToString();
                    user.IsActive = (bool)reader["isActive"];
                    results.Add(user);
                }

                reader.Close();
            }

            return results;
        }

        // Inserts a brand new user account.
        public void AddUser(UserAccount user)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO UserAccount (userName, passHash, role, isActive)
                                 VALUES (@userName, @passHash, @role, @isActive)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userName", user.UserName);
                cmd.Parameters.AddWithValue("@passHash", user.PassHash);
                cmd.Parameters.AddWithValue("@role", user.Role);
                cmd.Parameters.AddWithValue("@isActive", user.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Updates an existing user's details.
        public void UpdateUser(UserAccount user)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"UPDATE UserAccount SET
                                 userName = @userName,
                                 passHash = @passHash,
                                 role = @role,
                                 isActive = @isActive
                                 WHERE userAccountID = @userAccountID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userAccountID", user.UserAccountID);
                cmd.Parameters.AddWithValue("@userName", user.UserName);
                cmd.Parameters.AddWithValue("@passHash", user.PassHash);
                cmd.Parameters.AddWithValue("@role", user.Role);
                cmd.Parameters.AddWithValue("@isActive", user.IsActive);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Deactivates a user instead of deleting them.
        // Soft delete — keeps history intact.
        public void DeactivateUser(int userAccountID)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = "UPDATE UserAccount SET isActive = 0 WHERE userAccountID = @userAccountID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userAccountID", userAccountID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}