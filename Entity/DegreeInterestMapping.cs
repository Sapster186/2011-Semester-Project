using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    // Holds one row from the DegreeInterestMapping table.
    // Links a degree programme to an interest group with a weight,
    // so the scoring engine knows how strongly an interest area
    // counts for each degree.
    public class DegreeInterestMapping
    {
        public int MappingID { get; set; }
        public int DegreeProgrammeID { get; set; }
        public int InterestGroupID { get; set; }
        public float Weight { get; set; }

        // Empty constructor — used when the DA loads a row from the DB.
        public DegreeInterestMapping()
        {
        }

        // Full constructor — used when we create a mapping in code.
        public DegreeInterestMapping(int mappingID, int degreeProgrammeID, int interestGroupID, float weight)
        {
            MappingID = mappingID;
            DegreeProgrammeID = degreeProgrammeID;
            InterestGroupID = interestGroupID;
            Weight = weight;
        }
    }
}
