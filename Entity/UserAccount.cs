using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011_Semester_Project.Entity
{
    // Holds one row from the UserAccount table.
    // Represents a login for an Administrator or Career Advisor.
    public class UserAccount
    {
        // Properties match the UserAccount table columns.
        public int UserAccountID { get; set; }
        public string UserName { get; set; }
        public string PassHash { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }

        // Empty constructor — needed so the DA can create a blank UserAccount
        // and fill its properties from a database row.
        public UserAccount()
        {
        }

        // Full constructor — convenient when creating a UserAccount in code.
        public UserAccount(int userAccountID, string userName, string passHash, string role, bool isActive)
        {
            UserAccountID = userAccountID;
            UserName = userName;
            PassHash = passHash;
            Role = role;
            IsActive = isActive;
        }
    }
}
