using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    public class Learner
    {
        public int learnerID { get; set; }
        public int gradeLevelID { get; set; }
        public string name { get; set; }
        public string schoolName { get; set; }
        public string province { get; set; }
        public string contactDetails { get; set; }   
        public DateTime dateCreated { get; set; }

        public Learner()
        {
            name = "";
            schoolName = "";
            province = "";
            contactDetails = null;
            dateCreated = DateTime.Now;
        }

        public Learner(int learnerID, int gradeLevelID, string name, string schoolName,
                       string province, string contactDetails, DateTime dateCreated)
        {
            this.learnerID = learnerID;
            this.gradeLevelID = gradeLevelID;
            this.name = name;
            this.schoolName = schoolName;
            this.province = province;
            this.contactDetails = contactDetails;
            this.dateCreated = dateCreated;
        }
    }
}
