using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    // ExportReportDA — writes rows to the ExportReport table.
    //
    // Every time the learner saves, exports or prints an Advisory Summary
    // (or a report), one row is inserted here recording the date, the format
    // (TXT / PDF / PRINT) and the full text. This is the audit trail required
    // by the Report 1 spec — it also proves in the demo that the export
    // actually happened.
    public class ExportReportDA
    {
        // Inserts one export record.
        public void LogExport(ExportReport report)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO ExportReport (sessionID, dateOfExport, fileFormat, contentReport)
                                 VALUES (@sessionID, @dateOfExport, @fileFormat, @contentReport)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@sessionID", report.SessionID);
                cmd.Parameters.AddWithValue("@dateOfExport", report.DateOfExport);
                cmd.Parameters.AddWithValue("@fileFormat", report.FileFormat);
                cmd.Parameters.AddWithValue("@contentReport", report.ContentReport);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}