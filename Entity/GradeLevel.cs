using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    public class GradeLevel
    {
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

        // grade level translates to grade name 
        public override string ToString()
        {
            return gradeName;
        }
    }
}
