using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    public class Subject
    {
        public int subjectID { get; set; }
        public string subjectName { get; set; }
        public bool isMaths { get; set; }
        public bool isMathsLit { get; set; }
        public bool isScience { get; set; }

        public Subject()
        {
            subjectName = "";
        }

        public Subject(int subjectID, string subjectName, bool isMaths, bool isMathsLit, bool isScience)
        {
            this.subjectID = subjectID;
            this.subjectName = subjectName;
            this.isMaths = isMaths;
            this.isMathsLit = isMathsLit;
            this.isScience = isScience;
        }

        public override string ToString()
        {
            return subjectName;
        }
    }
}
