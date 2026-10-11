using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    public class DegreeProgramme
    {
        public int degreeID { get; set; }
        public int facultyID { get; set; }
        public string degreeName { get; set; }
        public double minOverallAverage { get; set; }
        public bool isActive { get; set; }

        public DegreeProgramme()
        {
            degreeName = "";
            minOverallAverage = 0;
            isActive = true;
        }

        public DegreeProgramme(int degreeID, int facultyID, string degreeName, double minOverallAverage, bool isActive)
        {
            this.degreeID = degreeID;
            this.facultyID = facultyID;
            this.degreeName = degreeName;
            this.minOverallAverage = minOverallAverage;
            this.isActive = isActive;
        }
    }
}