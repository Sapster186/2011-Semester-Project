using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using _2011__Semester_Project.Entity;
using _2011__Semester_Project.DataAccess;

namespace _2011__Semester_Project.Controller
{
    public class ReportController
    {
        // One of each DA class this controller needs to talk to the database.
        private FundOptionDA fundOptionDA = new FundOptionDA();
        private FundFacultyDA fundFacultyDA = new FundFacultyDA();
        private FundProgrammeDA fundProgrammeDA = new FundProgrammeDA();
        private ExportReportDA exportReportDA = new ExportReportDA();

        // ---------- FUNDING LOOKUP ----------
        // Pass null for any filter you don't want to apply.
        public List<FundOption> GetFilteredFunding(int? facultyID, int? degreeID, string fundType)
        {
            List<FundOption> allActive = fundOptionDA.GetActiveFundOptions();
            List<FundOption> filtered = new List<FundOption>();

            foreach (FundOption option in allActive)
            {
                bool matches = true;

                // Filter by type (skip if no type chosen)
                if (!string.IsNullOrEmpty(fundType) && option.FundType != fundType)
                    matches = false;

                // Filter by faculty (skip if no faculty chosen)
                if (facultyID.HasValue)
                {
                    List<FundFaculty> links = fundFacultyDA.GetFacultiesForFundOption(option.FundID);
                    bool facultyMatch = false;
                    foreach (FundFaculty link in links)
                        if (link.FacultyID == facultyID.Value) facultyMatch = true;
                    if (!facultyMatch) matches = false;
                }

                // Filter by degree (skip if no degree chosen)
                if (degreeID.HasValue)
                {
                    List<FundProgramme> links = fundProgrammeDA.GetProgrammesForFundOption(option.FundID);
                    bool degreeMatch = false;
                    foreach (FundProgramme link in links)
                        if (link.DegreeID == degreeID.Value) degreeMatch = true;
                    if (!degreeMatch) matches = false;
                }

                if (matches) filtered.Add(option);
            }

            return filtered;
        }

        // ---------- ADVISORY SUMMARY (Report 1) ----------
        // Builds the full summary as plain text, ready to show on screen or export.
        public string BuildAdvisorySummaryText(int sessionID)
        {
            StringBuilder sb = new StringBuilder();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open();

                // Get the session + learner + degree + faculty info in one query
                string sessionQuery = @"
                    SELECT s.sessionID, s.eligibleResult, s.overallScore,
                           l.name, l.schoolName, l.province,
                           d.degreeName, f.facultyName
                    FROM AdviceSession s
                    JOIN Learner l ON s.learnerID = l.learnerID
                    JOIN DegreeProgramme d ON s.degreeID = d.degreeID
                    JOIN Faculty f ON d.facultyID = f.facultyID
                    WHERE s.sessionID = @sessionID";

                SqlCommand cmd = new SqlCommand(sessionQuery, conn);
                cmd.Parameters.AddWithValue("@sessionID", sessionID);
                SqlDataReader reader = cmd.ExecuteReader();

                string learnerName = "", schoolName = "", province = "", degreeName = "", facultyName = "", eligibleResult = "";
                double overallScore = 0;

                if (reader.Read())
                {
                    learnerName = reader["name"].ToString();
                    schoolName = reader["schoolName"].ToString();
                    province = reader["province"].ToString();
                    degreeName = reader["degreeName"].ToString();
                    facultyName = reader["facultyName"].ToString();
                    eligibleResult = reader["eligibleResult"].ToString();
                    overallScore = Convert.ToDouble(reader["overallScore"]);
                }
                reader.Close(); // must close before running another query on the same connection

                // Build the text report, section by section
                sb.AppendLine("FUTURE PATH: Student Advisory Summary");
                sb.AppendLine("Date Generated: " + DateTime.Now.ToString("dd/MM/yyyy"));
                sb.AppendLine();
                sb.AppendLine("1. Profile");
                sb.AppendLine("Name: " + learnerName);
                sb.AppendLine("School: " + schoolName);
                sb.AppendLine("Province: " + province);
                sb.AppendLine("Selected Faculty: " + facultyName);
                sb.AppendLine("Selected Degree: " + degreeName);
                sb.AppendLine();

                sb.AppendLine("2. Academic Profile");
                string subjectQuery = @"
                    SELECT sub.subjectName, ls.mark
                    FROM LearnerSubject ls
                    JOIN Subject sub ON ls.subjectID = sub.subjectID
                    WHERE ls.learnerID = (SELECT learnerID FROM AdviceSession WHERE sessionID = @sessionID)
                    ORDER BY sub.subjectName";
                SqlCommand subCmd = new SqlCommand(subjectQuery, conn);
                subCmd.Parameters.AddWithValue("@sessionID", sessionID);
                SqlDataReader subReader = subCmd.ExecuteReader();
                while (subReader.Read())
                    sb.AppendLine(subReader["subjectName"].ToString() + ": " + subReader["mark"].ToString() + "%");
                subReader.Close();
                sb.AppendLine();

                sb.AppendLine("3. Eligibility Outcome");
                sb.AppendLine(eligibleResult.ToUpper());
                sb.AppendLine("Overall Score: " + overallScore);
                sb.AppendLine();

                sb.AppendLine("4. Recommended Alternatives");
                string altQuery = @"
                    SELECT d.degreeName
                    FROM RecommendationResult r
                    JOIN DegreeProgramme d ON r.degreeID = d.degreeID
                    WHERE r.sessionID = @sessionID AND r.isAlternative = 1";
                SqlCommand altCmd = new SqlCommand(altQuery, conn);
                altCmd.Parameters.AddWithValue("@sessionID", sessionID);
                SqlDataReader altReader = altCmd.ExecuteReader();
                bool anyAlt = false;
                while (altReader.Read())
                {
                    sb.AppendLine("- " + altReader["degreeName"].ToString());
                    anyAlt = true;
                }
                altReader.Close();
                if (!anyAlt) sb.AppendLine("None.");
                sb.AppendLine();

                sb.AppendLine("5. Relevant Funding Options");
                List<FundOption> funding = GetFilteredFunding(null, null, null);
                foreach (FundOption option in funding)
                    sb.AppendLine("- " + option.FundName + " (" + option.FundType + "): " + option.EligibleSummary);
                sb.AppendLine();

                sb.AppendLine("DISCLAIMER: THIS REPORT PROVIDES PRELIMINARY GUIDANCE BASED ON SAMPLE DATA");
                sb.AppendLine("AND IS NOT AN OFFICIAL UNIVERSITY ADMISSION DECISION. ALL REQUIREMENTS");
                sb.AppendLine("MUST BE VERIFIED WITH THE INSTITUTION.");
            }

            return sb.ToString();
        }

        // Saves the summary as a .txt file on disk, and logs the export in the database.
        public string ExportSummary(int sessionID, string summaryText)
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FuturePathExports");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            string fileName = "AdvisorySummary_" + sessionID + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
            string fullPath = Path.Combine(folder, fileName);
            File.WriteAllText(fullPath, summaryText);

            ExportReport report = new ExportReport();
            report.SessionID = sessionID;
            report.DateOfExport = DateTime.Now;
            report.FileFormat = "TXT";
            report.ContentReport = summaryText; // VARCHAR(MAX) column, no length limit
            exportReportDA.LogExport(report);

            return fullPath;
        }

        // ---------- REPORT 2: Degree Interest and Eligibility ----------
        public DataTable GetDegreeInterestEligibilityReport(DateTime startDate, DateTime endDate)
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
                    SELECT f.facultyName AS Faculty, d.degreeName AS DegreeProgramme,
                           COUNT(*) AS TotalSelections,
                           SUM(CASE WHEN s.eligibleResult = 'Strong Match' THEN 1 ELSE 0 END) AS StrongMatch,
                           SUM(CASE WHEN s.eligibleResult = 'Possible Match' THEN 1 ELSE 0 END) AS PossibleMatch,
                           SUM(CASE WHEN s.eligibleResult = 'Not Currently Eligible' THEN 1 ELSE 0 END) AS NotEligible
                    FROM AdviceSession s
                    JOIN DegreeProgramme d ON s.degreeID = d.degreeID
                    JOIN Faculty f ON d.facultyID = f.facultyID
                    WHERE s.dateOfSession BETWEEN @startDate AND @endDate
                    GROUP BY f.facultyName, d.degreeName
                    ORDER BY f.facultyName, TotalSelections DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table); // fills a whole table in one go, handy for grids
            }

            return table;
        }

        // ---------- REPORT 3: Not Currently Eligible Exception Report ----------
        public DataTable GetExceptionReport()
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"
                    SELECT l.name AS LearnerName, g.gradeName AS Grade, d.degreeName AS SelectedDegree,
                           s.eligibleResult AS AdvisoryOutcome, s.dateOfSession AS SessionDate
                    FROM AdviceSession s
                    JOIN Learner l ON s.learnerID = l.learnerID
                    JOIN GradeLevel g ON l.gradeLevelID = g.gradeLevelID
                    JOIN DegreeProgramme d ON s.degreeID = d.degreeID
                    WHERE s.eligibleResult = 'Not Currently Eligible' OR s.eligibleResult = 'More Information Needed'
                    ORDER BY g.gradeName, l.name";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);
            }

            return table;
        }
    }
}