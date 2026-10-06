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
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FuturePathDB;Integrated Security=True;";

        // Returns a new, ready-to-open connection every time it's called.
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
