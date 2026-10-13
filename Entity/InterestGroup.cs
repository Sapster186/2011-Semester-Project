using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    // Holds one row from the InterestGroup table.
    // Groups the questionnaire questions into broad areas like
    // "Computing & IT" or "Commerce & Business".
    public class InterestGroup
    {
        public int InterestGroupID { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }

        // Empty constructor — used when the DA loads a row from the DB.
        public InterestGroup()
        {
        }

        // Full constructor — used when we create a group in code.
        public InterestGroup(int interestGroupID, string groupName, string description)
        {
            InterestGroupID = interestGroupID;
            GroupName = groupName;
            Description = description;
        }
    }
}
