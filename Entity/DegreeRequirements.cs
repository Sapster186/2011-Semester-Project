using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    public class DegreeRequirements
    {
        public int requirementsID { get; set; }
        public int degreeID { get; set; }
        public int subjectID { get; set; }
        public double minMark { get; set; }
        public bool isNeeded { get; set; }
        public bool isAlternative { get; set; }
        public string adviceNote { get; set; }

        public DegreeRequirements()
        {
            minMark = 0;
            isNeeded = false;
            isAlternative = false;
            adviceNote = "";
        }

        public DegreeRequirements(int requirementsID, int degreeID, int subjectID, double minMark,
                                   bool isNeeded, bool isAlternative, string adviceNote)
        {
            this.requirementsID = requirementsID;
            this.degreeID = degreeID;
            this.subjectID = subjectID;
            this.minMark = minMark;
            this.isNeeded = isNeeded;
            this.isAlternative = isAlternative;
            this.adviceNote = adviceNote;
        }
    }
}