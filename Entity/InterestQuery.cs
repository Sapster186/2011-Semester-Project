using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011_Semester_Project.Entity
{
    // Holds one row from the InterestQuery table.
    // Each question belongs to an InterestGroup and has a weight used by the scoring engine.
    public class InterestQuery
    {
        public int QueryID { get; set; }
        public int InterestGroupID { get; set; }
        public string QueryText { get; set; }
        public float Weight { get; set; }
        public bool IsActive { get; set; }

        // Empty constructor — used when the DA loads a row from the DB.
        public InterestQuery()
        {
        }

        // Full constructor — used when we create a query in code.
        public InterestQuery(int queryID, int interestGroupID, string queryText, float weight, bool isActive)
        {
            QueryID = queryID;
            InterestGroupID = interestGroupID;
            QueryText = queryText;
            Weight = weight;
            IsActive = isActive;
        }
    }
}