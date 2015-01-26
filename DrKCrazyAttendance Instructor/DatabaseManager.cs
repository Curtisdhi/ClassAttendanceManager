using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient; 

namespace DrKCrazyAttendance_Instructor
{
    class DatabaseManager
    {
        private string serverAddr;
        private string database;
        private string username;
        private string password;
        public DatabaseManager(string serverAddr, string database, string username, string password)
        {
            this.database = database;
            this.username = username;
            this.password = password;
        }

        public MySqlConnection Connect()
        {
            string cs = string.Format("server={0};userid={1};password={2};database={3}",
                serverAddr, username, password, database);
            MySqlConnection connection = new MySqlConnection(cs);

            return connection;
        }
    }
}
