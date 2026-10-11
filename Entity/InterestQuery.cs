using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    public class InterestQuery
    {
        public int QueryID { get; set; } 
        public int InterestGroupID { get; set; } 
        public string QueryText { get; set; } 
        public float Weight { get; set; } 
    }
}
