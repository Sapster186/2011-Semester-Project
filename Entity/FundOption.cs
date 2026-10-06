using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    // Holds one FundOption row's worth of data.
    public class FundOption
    {
        // Private fields - only this class can change these directly
        private int fundID;
        private string fundName;
        private string fundType;
        private string eligibleSummary;
        private string necessaryDocs;
        private System.DateTime closeDate;
        private string contactInformation;
        private bool isActive;

        // Public properties - how other classes read/write each field safely
        public int FundID { get { return fundID; } set { fundID = value; } }
        public string FundName { get { return fundName; } set { fundName = value; } }
        public string FundType { get { return fundType; } set { fundType = value; } }
        public string EligibleSummary { get { return eligibleSummary; } set { eligibleSummary = value; } }
        public string NecessaryDocs { get { return necessaryDocs; } set { necessaryDocs = value; } }
        public System.DateTime CloseDate { get { return closeDate; } set { closeDate = value; } }
        public string ContactInformation { get { return contactInformation; } set { contactInformation = value; } }
        public bool IsActive { get { return isActive; } set { isActive = value; } }
    }
}