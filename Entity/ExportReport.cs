using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2011__Semester_Project.Entity
{
    // Records every time a learner exports/saves their Advisory Summary.
    public class ExportReport
    {
        private int reportID;
        private int sessionID;
        private System.DateTime dateOfExport;
        private string fileFormat;
        private string contentReport;

        public int ReportID { get { return reportID; } set { reportID = value; } }
        public int SessionID { get { return sessionID; } set { sessionID = value; } }
        public System.DateTime DateOfExport { get { return dateOfExport; } set { dateOfExport = value; } }
        public string FileFormat { get { return fileFormat; } set { fileFormat = value; } }
        public string ContentReport { get { return contentReport; } set { contentReport = value; } }
    }
}