using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using _2011__Semester_Project.DataAccess;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.Controller
{
    // The brain for funding, the Advisory Summary and the reports.
    // Forms ask this class; this class asks the Data Access classes.
    public class ReportController
    {
        private ReportDA reportDA = new ReportDA();
        private ExportReportDA exportReportDA = new ExportReportDA();

        // ---------- ACCESS CONTROL (Role-Based Access Control from the spec) ----------
        // Only Career Advisors and Administrators may use the dashboard and Reports 2 and 3
        public bool HasStaffAccess(string role)
        {
            return role != null &&
                   (role.Equals("Career Advisor", StringComparison.OrdinalIgnoreCase) ||
                    role.Equals("Administrator", StringComparison.OrdinalIgnoreCase));
        }

        // Second line of defence: the controller refuses even if a form forgot to check
        private void RequireStaffAccess(string role)
        {
            if (!HasStaffAccess(role))
                throw new UnauthorizedAccessException("Access denied: only Career Advisors and Administrators can use this function.");
        }

        // ---------- FUNDING LOOKUP ----------
        public DataTable GetFaculties()
        {
            return reportDA.GetFaculties();
        }

        public DataTable GetDegrees(int? facultyID)
        {
            return reportDA.GetDegrees(facultyID);
        }

        public DataTable GetFundTypes()
        {
            return reportDA.GetFundTypes();
        }

        // Active, not-expired funding filtered by faculty, degree and type (null = no filter)
        public DataTable GetFundingOptions(int? facultyID, int? degreeID, string fundType)
        {
            return reportDA.GetOpenFunding(facultyID, degreeID, fundType);
        }

        // ---------- CAREER ADVISOR DASHBOARD ----------
        public DataTable SearchSessions(string name, string role)
        {
            RequireStaffAccess(role);
            // An empty search box means "show everyone"
            return reportDA.SearchSessions(string.IsNullOrWhiteSpace(name) ? null : name.Trim());
        }

        // ---------- ADVISORY SUMMARY (Report 1) ----------
        // Converts a mark to APS points using the scale from the Part 1 report
        private int ApsPoints(double mark)
        {
            if (mark >= 80) return 7;
            if (mark >= 70) return 6;
            if (mark >= 60) return 5;
            if (mark >= 50) return 4;
            if (mark >= 40) return 3;
            if (mark >= 30) return 2;
            return 1;
        }

        // The footer: always shows the Date Generated and the exact disclaimer from the spec
        public string GetFooterText()
        {
            return "Date Generated: " + DateTime.Now.ToString("dd/MM/yyyy") + Environment.NewLine +
                   "DISCLAIMER: THIS REPORT PROVIDES PRELIMINARY GUIDANCE BASED ON SAMPLE DATA AND IS NOT AN " +
                   "OFFICIAL UNIVERSITY ADMISSION DECISION. ALL REQUIREMENTS MUST BE VERIFIED WITH THE INSTITUTION";
        }

        // Builds the summary text in the order of the Part 1 report layout
        public string BuildAdvisorySummaryText(int sessionID)
        {
            DataTable info = reportDA.GetSessionInfo(sessionID);
            if (info.Rows.Count == 0)
                return "No advisory session was found with ID " + sessionID + ".";

            DataRow row = info.Rows[0];
            int learnerID = Convert.ToInt32(row["learnerID"]);
            int degreeID = Convert.ToInt32(row["degreeID"]);
            int facultyID = Convert.ToInt32(row["facultyID"]);
            string outcome = row["eligibleResult"].ToString();
            string grade = row["gradeName"].ToString().Replace("Grade ", "");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FUTURE PATH: Student Advisory Summary");
            sb.AppendLine("Date Generated: " + DateTime.Now.ToString("dd/MM/yyyy"));
            sb.AppendLine();

            // 1. Profile
            sb.AppendLine("1. Profile");
            sb.AppendLine("Name: " + row["LearnerName"] + "    Grade: " + grade);
            sb.AppendLine("School: " + row["schoolName"] + "    Province: " + row["province"]);
            sb.AppendLine("Selected Faculty: " + row["facultyName"]);
            sb.AppendLine("Selected Degree: " + row["degreeName"]);
            sb.AppendLine();

            // 2. Academic Profile (alphabetical, mark and APS points per subject)
            sb.AppendLine("2. Academic Profile");
            DataTable marks = reportDA.GetLearnerMarks(learnerID);
            foreach (DataRow m in marks.Rows)
            {
                double mark = Convert.ToDouble(m["mark"]);
                sb.AppendLine(m["subjectName"] + ": " + mark + "% / APS " + ApsPoints(mark));
            }
            if (marks.Rows.Count == 0) sb.AppendLine("No marks captured.");
            sb.AppendLine();

            // Member B's saved result for the chosen degree (may not exist yet)
            DataTable result = reportDA.GetSessionResult(sessionID, degreeID);

            // 3. Aligned Interest
            sb.AppendLine("3. Aligned Interest");
            if (result.Rows.Count > 0)
            {
                double interest = Convert.ToDouble(result.Rows[0]["interestMatchScore"]);
                // Bands used for the wording: 70 and above is Strong, 40 to 69 is Moderate, below 40 is Low
                string level = interest >= 70 ? "Strong" : (interest >= 40 ? "Moderate" : "Low");
                string group = reportDA.GetTopInterestGroup(degreeID);
                sb.AppendLine("Interest Profile: " + level + " alignment" +
                              (group == "" ? "" : " with " + group) + " (" + Math.Round(interest) + "%).");
            }
            else
            {
                sb.AppendLine("Interest Profile: not available for this session.");
            }
            sb.AppendLine();

            // 4. Eligibility outcome: never shown as a bare label, always with its reasoning
            sb.AppendLine("4. Eligibility Outcome");
            sb.AppendLine(outcome.ToUpper());
            string gap = result.Rows.Count > 0 && result.Rows[0]["gapObservation"] != DBNull.Value
                ? result.Rows[0]["gapObservation"].ToString() : "";
            string reasoning;
            if (gap != "") reasoning = gap;
            else if (outcome == "Strong Match") reasoning = "You meet the requirements for this degree.";
            else reasoning = "No explanation was recorded for this session.";
            sb.AppendLine("Reasoning: " + reasoning);

            // Requirement comparison: the learner's marks against the stored requirements
            DataTable reqs = reportDA.GetDegreeRequirements(degreeID);
            if (reqs.Rows.Count > 0)
            {
                sb.AppendLine("Requirement comparison:");
                foreach (DataRow r in reqs.Rows)
                {
                    double min = Convert.ToDouble(r["minMark"]);
                    string kind = Convert.ToBoolean(r["isNeeded"]) ? "Required" : "Accepted alternative";
                    DataRow[] found = marks.Select("subjectID = " + Convert.ToInt32(r["subjectID"]));
                    string status;
                    if (found.Length == 0)
                        status = "not captured";
                    else
                    {
                        double have = Convert.ToDouble(found[0]["mark"]);
                        status = "you have " + have + "% (" + (have >= min ? "met" : "below minimum") + ")";
                    }
                    sb.AppendLine("- " + r["subjectName"] + " (" + kind + ", minimum " + min + "%): " + status);
                }
            }
            sb.AppendLine();

            // 5. Alternatives (inactive degrees already excluded by the query)
            sb.AppendLine("5. Recommended Alternatives");
            DataTable alternatives = reportDA.GetAlternatives(sessionID);
            foreach (DataRow a in alternatives.Rows)
                sb.AppendLine("- " + a["degreeName"] + " (score " + Math.Round(Convert.ToDouble(a["overallScore"]), 1) + ")");
            if (alternatives.Rows.Count == 0) sb.AppendLine("None.");
            sb.AppendLine();

            // 6. Funding (flagged as sample information to be verified)
            sb.AppendLine("6. Relevant Funding Options (sample information, verify with the institution)");
            DataTable funding = reportDA.GetOpenFunding(facultyID, degreeID, null);
            foreach (DataRow f in funding.Rows)
            {
                string closes = f["Closing Date"] == DBNull.Value
                    ? "no closing date"
                    : "closes " + Convert.ToDateTime(f["Closing Date"]).ToString("dd/MM/yyyy");
                sb.AppendLine("- " + f["Funding Name"] + " (" + f["Type"] + ", " + closes + "): " + f["Eligibility"]);
            }
            if (funding.Rows.Count == 0) sb.AppendLine("No open funding options are linked to this faculty or degree.");

            return sb.ToString();
        }

        // Writes the summary to a text file and records the export in ExportReport
        public void SaveSummaryAsText(int sessionID, string summaryText, string filePath)
        {
            File.WriteAllText(filePath, summaryText);
            LogExport(sessionID, summaryText, "TXT");
        }

        // Records an export (TXT, PDF or PRINT) with today's date
        public void LogExport(int sessionID, string summaryText, string format)
        {
            ExportReport report = new ExportReport();
            report.SessionID = sessionID;
            report.DateOfExport = DateTime.Now;
            report.FileFormat = format;
            report.ContentReport = summaryText;   // the column has no length limit
            exportReportDA.LogExport(report);
        }

        // ---------- REPORT 2: Degree Interest and Eligibility (RPT-SUM-002) ----------
        // Returns the grouped faculty/degree table; the header text comes back in headerText
        public DataTable GetDegreeInterestReport(DateTime startDate, DateTime endDate, string role, out string headerText)
        {
            RequireStaffAccess(role);

            DateTime start = startDate.Date;
            DateTime endExclusive = endDate.Date.AddDays(1);   // so the whole last day is included

            // Totals per outcome
            DataTable counts = reportDA.GetOutcomeCounts(start, endExclusive);
            int total = 0;
            foreach (DataRow r in counts.Rows) total += Convert.ToInt32(r["Total"]);

            // Work out the most selected faculty and degree from the breakdown table
            DataTable selections = reportDA.GetDegreeSelectionTable(start, endExclusive);
            Dictionary<string, int> facultyTotals = new Dictionary<string, int>();
            string topDegree = "None";
            int topDegreeCount = 0;
            foreach (DataRow r in selections.Rows)
            {
                string faculty = r["Faculty"].ToString();
                int count = Convert.ToInt32(r["Total"]);
                if (!facultyTotals.ContainsKey(faculty)) facultyTotals[faculty] = 0;
                facultyTotals[faculty] += count;
                if (count > topDegreeCount) { topDegreeCount = count; topDegree = r["Degree"].ToString(); }
            }
            string topFaculty = "None";
            int topFacultyCount = 0;
            foreach (KeyValuePair<string, int> pair in facultyTotals)
                if (pair.Value > topFacultyCount) { topFacultyCount = pair.Value; topFaculty = pair.Key; }

            // The header, laid out like the Part 1 report
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FUTURE PATH ADVICE SYSTEM");
            sb.AppendLine("Degree Interest and Eligibility Report");
            sb.AppendLine("Report - 002        Report ID: RPT-SUM-002");
            sb.AppendLine("Date Range: " + start.ToString("dd/MM/yyyy") + " - " + endDate.Date.ToString("dd/MM/yyyy"));
            sb.AppendLine("Audience: Career Advisors and Administrators");
            sb.AppendLine("Generated on: " + DateTime.Now.ToString("dd/MM/yyyy"));
            sb.AppendLine();
            sb.AppendLine("Total Sessions: " + total);

            string[] outcomes = { "Strong Match", "Possible Match", "Not Currently Eligible", "More Information Needed" };
            string[] labels = { "Strong matches", "Possible matches", "Not currently eligible", "More information needed" };
            for (int i = 0; i < outcomes.Length; i++)
            {
                int count = 0;
                foreach (DataRow r in counts.Rows)
                    if (r["Outcome"].ToString() == outcomes[i]) count = Convert.ToInt32(r["Total"]);
                double percent = total == 0 ? 0 : count * 100.0 / total;
                sb.AppendLine("- " + labels[i] + ": " + count + " (" + percent.ToString("0") + "%)");
            }

            sb.AppendLine();
            sb.AppendLine("Top Funding Category: " + reportDA.GetTopFundingCategory(start, endExclusive));
            sb.AppendLine("Most common subject gap: " + reportDA.GetMostCommonGap(start, endExclusive));
            sb.AppendLine("Most selected faculty: " + topFaculty);
            sb.AppendLine("Most selected degree: " + topDegree);
            sb.AppendLine();
            sb.AppendLine("DETAILED BREAKDOWN BY FACULTY AND DEGREE");
            headerText = sb.ToString();

            // The table: a GROUP row for each faculty, then its degrees (already highest Total first)
            DataTable report = new DataTable();
            report.Columns.Add("Faculty");
            report.Columns.Add("Degree Programme");
            report.Columns.Add("Total Selections");
            report.Columns.Add("Strong Match");
            report.Columns.Add("Possible Match");
            report.Columns.Add("Not Eligible");
            report.Columns.Add("More Info Needed");
            report.Columns.Add("RowType");   // Group or Data: the form uses it for formatting

            string currentFaculty = null;
            foreach (DataRow r in selections.Rows)
            {
                string faculty = r["Faculty"].ToString();
                if (faculty != currentFaculty)
                {
                    report.Rows.Add("GROUP: " + faculty, "", "", "", "", "", "", "Group");
                    currentFaculty = faculty;
                }
                report.Rows.Add("", r["Degree"], r["Total"], r["Strong"], r["Possible"], r["NotEligible"], r["MoreInfo"], "Data");
            }
            return report;
        }

        // ---------- REPORT 3: Not Currently Eligible Exception (RPT-EXC-001) ----------
        // Learners grouped by grade with a subtotal after each grade and a grand total at the end
        public DataTable GetExceptionReport(string role, out string headerText)
        {
            RequireStaffAccess(role);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("FUTURE PATH ADVICE SYSTEM");
            sb.AppendLine("Learners Not Currently Eligible: Exception Report");
            sb.AppendLine("Report ID: RPT-EXC-001");
            sb.AppendLine("Audience: Career Advisors and Administrators");
            sb.AppendLine("Generated on: " + DateTime.Now.ToString("dd/MM/yyyy"));
            headerText = sb.ToString();

            DataTable data = reportDA.GetExceptionRows();

            DataTable report = new DataTable();
            report.Columns.Add("Grade");
            report.Columns.Add("Learner");
            report.Columns.Add("Degree");
            report.Columns.Add("Outcome");
            report.Columns.Add("Identified Gaps");
            report.Columns.Add("Session Date");
            report.Columns.Add("RowType");   // Data, Subtotal or Total: the form uses it for colours

            string currentGrade = null;
            int gradeCount = 0;
            int grandTotal = 0;

            foreach (DataRow r in data.Rows)
            {
                string grade = r["Grade"].ToString();

                // The grade changed, so close the previous group with a subtotal row
                if (currentGrade != null && grade != currentGrade)
                {
                    report.Rows.Add("", "Subtotal " + currentGrade, "", gradeCount + " learner(s)", "", "", "Subtotal");
                    gradeCount = 0;
                }

                currentGrade = grade;
                report.Rows.Add(grade, r["Learner"], r["Degree"], r["Outcome"], r["Gaps"],
                    Convert.ToDateTime(r["SessionDate"]).ToString("dd/MM/yyyy"), "Data");
                gradeCount++;
                grandTotal++;
            }

            // Close the last group, then add the grand total
            if (currentGrade != null)
                report.Rows.Add("", "Subtotal " + currentGrade, "", gradeCount + " learner(s)", "", "", "Subtotal");

            report.Rows.Add("", "GRAND TOTAL", "", grandTotal + " learner(s)", "", "", "Total");
            return report;
        }

        // The footer line printed under Reports 2 and 3
        public string GetReportFooter(int reportNumber)
        {
            string id = reportNumber == 2 ? "Report - 002" : "RPT-EXC-001";
            return "Confidential - Role-Based Access Control (RBAC) enforced | " + id +
                   " | Date Generated: " + DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
}