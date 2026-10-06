using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    // Links one FundOption to one Faculty (a bursary might apply to a whole faculty).
    public class FundFaculty
    {
        private int fundFacultyID;
        private int fundID;
        private int facultyID;

        public int FundFacultyID { get { return fundFacultyID; } set { fundFacultyID = value; } }
        public int FundID { get { return fundID; } set { fundID = value; } }
        public int FacultyID { get { return facultyID; } set { facultyID = value; } }
    }
}
