using _2011__Semester_Project.DataAccess;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.Controller
{
    public class LoginController
    {
        public static string CurrentUsername { get; private set; }
        public static string CurrentUserRole { get; private set; }
        private readonly UserAccountDA userAccountDA = new UserAccountDA();

        public bool Login(string username, string password, string selectedRole)
        {
            // A failed login must not keep a previous user's permissions.
            Logout();
            if (selectedRole == "Learner")
            {
                CurrentUserRole = "Learner";
                return true;
            }

            if (selectedRole != "Career Advisor" && selectedRole != "Administrator") return false;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return false;

            UserAccount user = userAccountDA.GetByUsername(username.Trim());
            // Keep the existing password format so the team's current accounts still work.
            if (user == null || !user.IsActive || user.PassHash != password || user.Role != selectedRole)
                return false;

            CurrentUsername = user.UserName;
            CurrentUserRole = user.Role;
            return true;
        }

        public void Logout()
        {
            CurrentUsername = null;
            CurrentUserRole = null;
            LearnerController.StartNewSession();
        }
    }
}
