using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace _2011__Semester_Project.DataAccess
{
    public static class DatabaseHelper
    {
        // One shared connection string - everyone's DA classes use this
        // so we only ever have to fix it in one place.
        private static string connectionString =
     @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database\FuturePath.mdf;Integrated Security=True;Connect Timeout=30";

        // Returns a new, ready-to-open connection every time it's called.
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
