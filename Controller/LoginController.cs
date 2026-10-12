using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2011_Semester_Project.Entity;
using _2011_Semester_Project.DataAccess;

namespace _2011_Semester_Project.Controller
{
    // The brain behind the login form.
    // Verifies a username + password + role against the UserAccount table.
    // Remembers who is logged in so other forms can check the role.
    public class LoginController
    {
        // Session state — who is logged in right now.
        // "static" means the value is shared across the whole application.
        // Any form can read LoginController.CurrentUserRole to know the role.
        public static string CurrentUsername { get; private set; }
        public static string CurrentUserRole { get; private set; }

        // The DA layer we call to look up users.
        private UserAccountDA userAccountDA = new UserAccountDA();

        // Tries to log in. Returns true if successful.
        // On success, sets CurrentUsername and CurrentUserRole.
        public bool Login(string username, string password, string selectedRole)
        {
            // 1. Look up the user in the database.
            UserAccount user = userAccountDA.GetByUsername(username);

            // 2. Refuse if the user doesn't exist or is inactive.
            if (user == null)
            {
                return false;
            }

            // 3. Check the password.
            // NOTE: for now we compare directly (plain text). If time allows,
            // this is where hashing should be added.
            if (user.PassHash != password)
            {
                return false;
            }

            // 4. Check the user's real role matches what they selected.
            // This blocks a "Learner" from logging in as an "Administrator".
            if (user.Role != selectedRole)
            {
                return false;
            }

            // 5. Success — remember who is logged in.
            CurrentUsername = user.UserName;
            CurrentUserRole = user.Role;
            return true;
        }

        // Clears the session — called on logout.
        public void Logout()
        {
            CurrentUsername = null;
            CurrentUserRole = null;
        }
    }
}
