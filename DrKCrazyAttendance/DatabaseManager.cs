using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DrKCrazyAttendance.Properties;
using System.Data; 

namespace DrKCrazyAttendance
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

        public static MySqlDataReader GetDataReaderFromQuery(string query)
        {
            return GetDataReaderFromQuery(query, new Dictionary<string, object>());
        }

        public static MySqlDataReader GetDataReaderFromQuery(string query, Dictionary<string, Object> parameters)
        {
            MySqlConnection conn;
            MySqlCommand cmd;
            MySqlDataReader rdr = null;

            conn = DatabaseManager.Connect();
            try
            {
                conn.Open();
                cmd = new MySqlCommand(query, conn);
                cmd.Prepare();
                foreach (string key in parameters.Keys)
                {
                    cmd.Parameters.AddWithValue(key, parameters[key]);
                }
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            return rdr;
        }

        public static DataTable GetDataTableFromQuery(string query, Dictionary<string, Object> parameters) 
        {
            DataTable table = new DataTable();
            MySqlDataReader rdr = null;
            using (rdr = GetDataReaderFromQuery(query, parameters))
            {
                table.Load(rdr);
            }
            
            return table;
        }

        public static bool ExecuteQuery(string query, List<Dictionary<string, Object>> parameters)
        {
            bool success = false;
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            MySqlTransaction tr = null; 
            using (conn = DatabaseManager.Connect())
            {
                try
                {
                    conn.Open();
                    tr = conn.BeginTransaction();
                    using (cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Prepare();
                        cmd.Transaction = tr;
                        //we use a list of dictionarys to allow different sets of parameters. 
                        //Such in the event we are given the job of updating several queries in one time.
                        foreach (Dictionary<string, Object> param in parameters)
                        {
                            foreach (string key in param.Keys)
                            {
                                cmd.Parameters.AddWithValue(key, param[key]);
                            }
                            cmd.ExecuteNonQuery();
                        }
                        tr.Commit();

                    }
                }
                catch (MySqlException ex)
                {
                    try
                    {
                        if (tr != null)
                            tr.Rollback();
                    }
                    catch (MySqlException ex1)
                    {
                        Console.WriteLine("Error: {0}", ex1.ToString());
                    }

                    Console.WriteLine("Error: {0}", ex.ToString());
                }
            }
            return success;
        }
    }

}
