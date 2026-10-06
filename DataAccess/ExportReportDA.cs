using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using _2011__Semester_Project.Entity;

namespace _2011__Semester_Project.DataAccess
{
    public class ExportReportDA
    {
        // Logs one export event to the database.
        public void LogExport(ExportReport report)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                string query = @"INSERT INTO ExportReport 
                    (sessionID, dateOfExport, fileFormat, contentReport)
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