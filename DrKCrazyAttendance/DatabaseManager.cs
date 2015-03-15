using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DrKCrazyAttendance.Properties;
using System.Data;
using DrKCrazyAttendance.Util; 

namespace DrKCrazyAttendance
{
    public class DatabaseManager
    {
        static DatabaseManager() {
            IsConnectable = true;
        }

        public static bool IsConnectable
        {
            get;
            private set;
        }

        public static MySqlConnection Connect()
        {
            MySqlConnection connection = null;
            if (IsConnectable)
            {
                string serverAddr = SecurityCrypt.AES_Decrypt(Settings.Default.SqlServerAddr);
                string database = SecurityCrypt.AES_Decrypt(Settings.Default.SqlDatabase);
                string username = SecurityCrypt.AES_Decrypt(Settings.Default.SqlUsername);
                string password = SecurityCrypt.AES_Decrypt(Settings.Default.SqlPassword);

                /* 
                 * Requires "Convert Zero Datetime=True" to properly convert Date and Time sql types to .net Datetime
                 * http://stackoverflow.com/questions/5754822/unable-to-convert-mysql-date-time-value-to-system-datetime
                 */
                string cs = string.Format("server={0};userid={1};password={2};database={3};Convert Zero Datetime=True",
                    serverAddr, username, password, database);

                connection = new MySqlConnection(cs);

            }
            return connection;
        }

        public static bool TestConnection()
        {
            MySqlConnection conn;
            using (conn = DatabaseManager.Connect())
            {
                try
                {
                    conn.Open();
                    IsConnectable = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    IsConnectable = false;
                }
            }
            
            return IsConnectable;
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
                try
                {
                    conn.Open();
                    IsConnectable = true;
                }
                catch (MySqlException)
                {
                    IsConnectable = false;
                    throw;
                }
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
                if (rdr != null)
                {
                    table.Load(rdr);
                }
            }
            
            return table;
        }

        /// <summary>
        /// Executes a single query to retrieve a single scalar result
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns>Scalar result</returns>
        public static long ExecuteScalar(string query, Dictionary<string, Object> parameters)
        {
            long scalar = 0;
            MySqlConnection conn;
            MySqlCommand cmd;

            using (conn = DatabaseManager.Connect())
            {
                try
                {
                    try
                    {
                        conn.Open();
                        IsConnectable = true;
                    }
                    catch (MySqlException)
                    {
                        IsConnectable = false;
                    }
                    using (cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Prepare();
                        foreach (string key in parameters.Keys)
                        {
                            cmd.Parameters.AddWithValue(key, parameters[key]);
                        }
                        scalar = Convert.ToInt64(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }

            return scalar;
        }

        /// <summary>
        /// Executes a single query
        /// </summary>
        /// <param name="query">Sql query</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns>last inserted id if the query was an insert.</returns>
        public static long ExecuteQuery(string query, Dictionary<string, Object> parameters)
        {
            long id = 0;
            MySqlConnection conn = null;
            MySqlCommand cmd = null;
            using (conn = DatabaseManager.Connect())
            {
                try
                {
                    try
                    {
                        conn.Open();
                        IsConnectable = true;
                    }
                    catch (MySqlException)
                    {
                        IsConnectable = false;
                    }

                    using (cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Prepare();
                        foreach (string key in parameters.Keys)
                        {
                            cmd.Parameters.AddWithValue(key, parameters[key]);
                        }
                        cmd.ExecuteNonQuery();
                        id = cmd.LastInsertedId;
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error: {0}", ex.ToString());
                    throw;
                }
            }
            return id;
        }

        /// <summary>
        /// Executes multiple queries
        /// Parameters must be symmetric.
        /// </summary>
        /// <param name="query">Sql query</param>
        /// <param name="parameters">Query parameters</param>
        /// <returns>Query success</returns>
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
                    try
                    {
                        conn.Open();
                        IsConnectable = true;
                    }
                    catch (MySqlException)
                    {
                        IsConnectable = false;
                    }
                    tr = conn.BeginTransaction();
                    using (cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Prepare();
                        cmd.Transaction = tr;

                        //add the parameters first thing
                        foreach (string key in parameters.First().Keys)
                        {
                            cmd.Parameters.AddWithValue(key, null);
                        }
                        //we use a list of dictionarys to allow different sets of parameters. 
                        //Such in the event we are given the job of updating several queries in one time.
                        foreach (Dictionary<string, Object> param in parameters)
                        {
                            foreach (string key in param.Keys)
                            {
                                //change the values to match what we were given
                                cmd.Parameters[key].Value = param[key];
                            }
                            cmd.ExecuteNonQuery();
                        }
                        success = true;
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
                    throw;
                }
            }
            return success;
        }

    }

}
