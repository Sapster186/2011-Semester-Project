using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    // Links one FundOption to one specific DegreeProgramme.
    public class FundProgramme
    {
        private int fundProgrammeID;
        private int fundID;
        private int degreeID;

        public int FundProgrammeID { get { return fundProgrammeID; } set { fundProgrammeID = value; } }
        public int FundID { get { return fundID; } set { fundID = value; } }
        public int DegreeID { get { return degreeID; } set { degreeID = value; } }
    }
}
