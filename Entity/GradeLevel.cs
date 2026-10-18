using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    public class GradeLevel
    {
        // All attributes connected to grade level
        public int gradeLevelID { get; set; }
        public string gradeName { get; set; }

       
        public GradeLevel()
        {
            gradeName = "";
        }

        
        public GradeLevel(int gradeLevelID, string gradeName)
        {
            this.gradeLevelID = gradeLevelID;
            this.gradeName = gradeName;
        }

        // This converts the numerical grade level to a grade name
        // ie 1 = Grade 11 and 2 = Grade 12
        public override string ToString()
        {
            return gradeName;
        }
    }
}
