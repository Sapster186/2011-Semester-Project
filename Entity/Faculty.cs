using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    public class Faculty
    {
        public int facultyID { get; set; }
        public string facultyName { get; set; }
        public string facultyDesc { get; set; }

        public Faculty()
        {
            facultyName = "";
            facultyDesc = "";
        }

        public Faculty(int facultyID, string facultyName, string facultyDesc)
        {
            this.facultyID = facultyID;
            this.facultyName = facultyName;
            this.facultyDesc = facultyDesc;
        }
    }
}
