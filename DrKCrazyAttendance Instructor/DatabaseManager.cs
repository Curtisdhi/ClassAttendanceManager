using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DrKCrazyAttendance_Instructor.Properties; 

namespace DrKCrazyAttendance_Instructor
{
    public class DatabaseManager
    {
        public static MySqlConnection Connect()
        {
            string serverAddr = Settings.Default.SqlServerAddr;
            string database = Settings.Default.SqlDatabase;
            string username = Settings.Default.SqlUsername;
            string password = Settings.Default.SqlPassword;

            /* 
             * Requires "Convert Zero Datetime=True" to properly convert Date and Time sql types to .net Datetime
             * http://stackoverflow.com/questions/5754822/unable-to-convert-mysql-date-time-value-to-system-datetime
             */
            string cs = string.Format("server={0};userid={1};password={2};database={3};Convert Zero Datetime=True",
                serverAddr, username, password, database);
            MySqlConnection connection = new MySqlConnection(cs);

            return connection;
        }
    }
}
