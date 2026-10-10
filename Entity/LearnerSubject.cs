using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    public class LearnerSubject
    {
        public int learnerSubjectID { get; set; }
        public int learnerID { get; set; }
        public int subjectID { get; set; }
        public double mark { get; set; }

        public LearnerSubject()
        {
        }

        public LearnerSubject(int learnerSubjectID, int learnerID, int subjectID, double mark)
        {
            this.learnerSubjectID = learnerSubjectID;
            this.learnerID = learnerID;
            this.subjectID = subjectID;
            this.mark = mark;
        }
    }
}
