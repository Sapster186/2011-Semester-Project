using System;
using System.Data;
using System.Data.SqlClient;

namespace _2011__Semester_Project.DataAccess
{
    // Read-only queries behind the funding screen, summary, dashboard and reports.
    // Only Data Access classes talk to the database; the controller calls these methods.
    public class ReportDA
    {
        // Runs a SELECT and returns the rows as a DataTable.
        // Parameters are passed as pairs: "@name", value, "@name2", value2 ...
        private DataTable RunQuery(string sql, params object[] parameters)
        {
            DataTable table = new DataTable();
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                for (int i = 0; i < parameters.Length; i += 2)
                {
                    // A null value is sent as DBNull, which SQL treats as "no value"
                    cmd.Parameters.AddWithValue((string)parameters[i], parameters[i + 1] ?? DBNull.Value);
                }
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(table);   // Fill opens and closes the connection by itself
            }
            return table;
        }

        // ---------- Funding Information Form ----------
        public DataTable GetFaculties()
        {
            return RunQuery("SELECT facultyID, facultyName FROM Faculty ORDER BY facultyName");
        }

        // Active degrees only (inactive ones are excluded), optionally for one faculty
        public DataTable GetDegrees(int? facultyID)
        {
            string sql = @"
                SELECT degreeID, degreeName
                FROM DegreeProgramme
                WHERE isActive = 1 AND (@facultyID IS NULL OR facultyID = @facultyID)
                ORDER BY degreeName";
            return RunQuery(sql, "@facultyID", facultyID);
        }

        // The funding types that exist (the lookup list for the Type filter)
        public DataTable GetFundTypes()
        {
            return RunQuery("SELECT DISTINCT fundType FROM FundOption WHERE isActive = 1 ORDER BY fundType");
        }

        // Funding that is active and not past its closing date.
        // Faculty filter: linked to the faculty, or to a degree in that faculty.
        // Degree filter: linked to the degree, or to the whole faculty the degree belongs to.
        // Pass null for any filter you don't want.
        public DataTable GetOpenFunding(int? facultyID, int? degreeID, string fundType)
        {
            string sql = @"
                SELECT f.fundName AS [Funding Name], f.fundType AS [Type], f.eligibleSummary AS [Eligibility],
                       f.necessaryDocs AS [Documents Needed], f.closeDate AS [Closing Date],
                       f.contactInformation AS [Contact],
                       'Sample information - verify with institution' AS [Status]
                FROM FundOption f
                WHERE f.isActive = 1
                  AND (f.closeDate IS NULL OR f.closeDate >= CAST(GETDATE() AS DATE))
                  AND (@fundType IS NULL OR f.fundType = @fundType)
                  AND (@facultyID IS NULL
                       OR EXISTS (SELECT 1 FROM FundFaculty ff WHERE ff.fundID = f.fundID AND ff.facultyID = @facultyID)
                       OR EXISTS (SELECT 1 FROM FundProgramme fp JOIN DegreeProgramme d ON d.degreeID = fp.degreeID
                                  WHERE fp.fundID = f.fundID AND d.facultyID = @facultyID))
                  AND (@degreeID IS NULL
                       OR EXISTS (SELECT 1 FROM FundProgramme fp WHERE fp.fundID = f.fundID AND fp.degreeID = @degreeID)
                       OR EXISTS (SELECT 1 FROM FundFaculty ff JOIN DegreeProgramme d ON d.facultyID = ff.facultyID
                                  WHERE ff.fundID = f.fundID AND d.degreeID = @degreeID))
                ORDER BY f.fundName";
            return RunQuery(sql, "@facultyID", facultyID, "@degreeID", degreeID, "@fundType", fundType);
        }

        // ---------- Career Advisor Dashboard ----------
        // One row per session, with the outcome and its gap observation
        public DataTable SearchSessions(string nameFilter)
        {
            string sql = @"
                SELECT s.sessionID AS SessionID, l.name AS Learner, g.gradeName AS Grade,
                       d.degreeName AS Degree, s.eligibleResult AS Outcome,
                       ISNULL(r.gapObservation, '') AS [Gap Observation]
                FROM AdviceSession s
                JOIN Learner l ON l.learnerID = s.learnerID
                JOIN GradeLevel g ON g.gradeLevelID = l.gradeLevelID
                JOIN DegreeProgramme d ON d.degreeID = s.degreeID
                OUTER APPLY (SELECT TOP 1 rr.gapObservation FROM RecommendationResult rr
                             WHERE rr.sessionID = s.sessionID AND rr.degreeID = s.degreeID AND rr.isAlternative = 0) r
                WHERE (@name IS NULL OR l.name LIKE '%' + @name + '%')
                ORDER BY s.dateOfSession DESC";
            return RunQuery(sql, "@name", nameFilter);
        }

        // ---------- Advisory Summary (Report 1) ----------
        public DataTable GetSessionInfo(int sessionID)
        {
            string sql = @"
                SELECT s.sessionID, s.learnerID, s.degreeID, s.eligibleResult, s.overallScore,
                       l.name AS LearnerName, l.schoolName, l.province, g.gradeName,
                       d.degreeName, d.facultyID, f.facultyName
                FROM AdviceSession s
                JOIN Learner l ON l.learnerID = s.learnerID
                JOIN GradeLevel g ON g.gradeLevelID = l.gradeLevelID
                JOIN DegreeProgramme d ON d.degreeID = s.degreeID
                JOIN Faculty f ON f.facultyID = d.facultyID
                WHERE s.sessionID = @sessionID";
            return RunQuery(sql, "@sessionID", sessionID);
        }

        // Subjects and marks, sorted alphabetically (as the spec requires)
        public DataTable GetLearnerMarks(int learnerID)
        {
            string sql = @"
                SELECT ls.subjectID, sub.subjectName, ls.mark
                FROM LearnerSubject ls
                JOIN Subject sub ON sub.subjectID = ls.subjectID
                WHERE ls.learnerID = @learnerID
                ORDER BY sub.subjectName";
            return RunQuery(sql, "@learnerID", learnerID);
        }

        // The stored requirements for the degree (for the marks comparison)
        public DataTable GetDegreeRequirements(int degreeID)
        {
            string sql = @"
                SELECT dr.subjectID, sub.subjectName, dr.minMark, dr.isNeeded, dr.isAlternative
                FROM DegreeRequirements dr
                JOIN Subject sub ON sub.subjectID = dr.subjectID
                WHERE dr.degreeID = @degreeID
                ORDER BY dr.isNeeded DESC, sub.subjectName";
            return RunQuery(sql, "@degreeID", degreeID);
        }

        // Member B's saved interest score and gap explanation for the chosen degree (may be empty)
        public DataTable GetSessionResult(int sessionID, int degreeID)
        {
            string sql = @"
                SELECT TOP 1 interestMatchScore, gapObservation
                FROM RecommendationResult
                WHERE sessionID = @sessionID AND degreeID = @degreeID AND isAlternative = 0";
            return RunQuery(sql, "@sessionID", sessionID, "@degreeID", degreeID);
        }

        // The interest group that suits the degree best (used for the Aligned Interest text)
        public string GetTopInterestGroup(int degreeID)
        {
            string sql = @"
                SELECT TOP 1 ig.groupName
                FROM DegreeInterestMapping m
                JOIN InterestGroup ig ON ig.interestGroupID = m.interestGroupID
                WHERE m.degreeID = @degreeID
                ORDER BY m.weight DESC";
            DataTable t = RunQuery(sql, "@degreeID", degreeID);
            return t.Rows.Count == 0 ? "" : t.Rows[0]["groupName"].ToString();
        }

        // Alternatives for the session, best first. Inactive degrees are excluded (spec rule).
        public DataTable GetAlternatives(int sessionID)
        {
            string sql = @"
                SELECT d.degreeName, r.overallScore
                FROM RecommendationResult r
                JOIN DegreeProgramme d ON d.degreeID = r.degreeID
                WHERE r.sessionID = @sessionID AND r.isAlternative = 1 AND d.isActive = 1
                ORDER BY r.overallScore DESC";
            return RunQuery(sql, "@sessionID", sessionID);
        }

        // ---------- Report 2: Degree Interest and Eligibility ----------
        // The end date is "exclusive": midnight after the last chosen day, so that day is included
        public DataTable GetOutcomeCounts(DateTime start, DateTime endExclusive)
        {
            string sql = @"
                SELECT eligibleResult AS Outcome, COUNT(*) AS Total
                FROM AdviceSession
                WHERE dateOfSession >= @start AND dateOfSession < @end
                GROUP BY eligibleResult";
            return RunQuery(sql, "@start", start, "@end", endExclusive);
        }

        // Funding type plus faculty linked most often to the degrees learners chose.
        // A tie is broken alphabetically.
        public string GetTopFundingCategory(DateTime start, DateTime endExclusive)
        {
            string sql = @"
                SELECT TOP 1 f.fundType + ' (' + fa.facultyName + ')' AS Category
                FROM AdviceSession s
                JOIN DegreeProgramme d ON d.degreeID = s.degreeID
                JOIN Faculty fa ON fa.facultyID = d.facultyID
                JOIN FundFaculty ff ON ff.facultyID = d.facultyID
                JOIN FundOption f ON f.fundID = ff.fundID
                WHERE s.dateOfSession >= @start AND s.dateOfSession < @end
                  AND f.isActive = 1 AND (f.closeDate IS NULL OR f.closeDate >= CAST(GETDATE() AS DATE))
                GROUP BY f.fundType, fa.facultyName
                ORDER BY COUNT(*) DESC, f.fundType";
            DataTable t = RunQuery(sql, "@start", start, "@end", endExclusive);
            return t.Rows.Count == 0 ? "None" : t.Rows[0]["Category"].ToString();
        }

        // The required subject learners most often miss or fall below the minimum in
        public string GetMostCommonGap(DateTime start, DateTime endExclusive)
        {
            string sql = @"
                SELECT TOP 1 sub.subjectName AS SubjectName
                FROM AdviceSession s
                JOIN DegreeRequirements dr ON dr.degreeID = s.degreeID AND dr.isNeeded = 1
                JOIN Subject sub ON sub.subjectID = dr.subjectID
                LEFT JOIN LearnerSubject ls ON ls.learnerID = s.learnerID AND ls.subjectID = dr.subjectID
                WHERE s.dateOfSession >= @start AND s.dateOfSession < @end
                  AND (ls.mark IS NULL OR ls.mark < dr.minMark)
                GROUP BY sub.subjectName
                ORDER BY COUNT(*) DESC, sub.subjectName";
            DataTable t = RunQuery(sql, "@start", start, "@end", endExclusive);
            return t.Rows.Count == 0 ? "None" : t.Rows[0]["SubjectName"].ToString();
        }

        // Selections per faculty and degree: grouped by faculty, highest Total first inside each faculty
        public DataTable GetDegreeSelectionTable(DateTime start, DateTime endExclusive)
        {
            string sql = @"
                SELECT f.facultyName AS Faculty, d.degreeName AS Degree, COUNT(*) AS Total,
                       SUM(CASE WHEN s.eligibleResult = 'Strong Match' THEN 1 ELSE 0 END) AS Strong,
                       SUM(CASE WHEN s.eligibleResult = 'Possible Match' THEN 1 ELSE 0 END) AS Possible,
                       SUM(CASE WHEN s.eligibleResult = 'Not Currently Eligible' THEN 1 ELSE 0 END) AS NotEligible,
                       SUM(CASE WHEN s.eligibleResult = 'More Information Needed' THEN 1 ELSE 0 END) AS MoreInfo
                FROM AdviceSession s
                JOIN DegreeProgramme d ON d.degreeID = s.degreeID
                JOIN Faculty f ON f.facultyID = d.facultyID
                WHERE s.dateOfSession >= @start AND s.dateOfSession < @end
                GROUP BY f.facultyName, d.degreeName
                ORDER BY f.facultyName, COUNT(*) DESC, d.degreeName";
            return RunQuery(sql, "@start", start, "@end", endExclusive);
        }

        // ---------- Report 3: Not Currently Eligible Exception ----------
        // Only Not Currently Eligible or More Information Needed; sorted by grade, then learner name
        public DataTable GetExceptionRows()
        {
            string sql = @"
                SELECT g.gradeName AS Grade, l.name AS Learner, d.degreeName AS Degree,
                       s.eligibleResult AS Outcome, ISNULL(r.gapObservation, '') AS Gaps,
                       s.dateOfSession AS SessionDate
                FROM AdviceSession s
                JOIN Learner l ON l.learnerID = s.learnerID
                JOIN GradeLevel g ON g.gradeLevelID = l.gradeLevelID
                JOIN DegreeProgramme d ON d.degreeID = s.degreeID
                OUTER APPLY (SELECT TOP 1 rr.gapObservation FROM RecommendationResult rr
                             WHERE rr.sessionID = s.sessionID AND rr.degreeID = s.degreeID AND rr.isAlternative = 0) r
                WHERE s.eligibleResult IN ('Not Currently Eligible', 'More Information Needed')
                ORDER BY g.gradeName, l.name";
            return RunQuery(sql);
        }
    }
}