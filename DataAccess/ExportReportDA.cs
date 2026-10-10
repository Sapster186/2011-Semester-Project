using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    public class ExportReportDA
    {
        // Logs one export (date, format and the exported text) in the ExportReport table
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