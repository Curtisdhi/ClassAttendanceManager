using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrKCrazyAttendance
{
    public class Student
    {
        public Student()
        {

        }
        public Student(int id, string username)
        {
            this.Id = id;
            this.Username = username;
        }

        #region properties
        public int Id
        {
            get;
            private set;
        }

        public string Username
        {
            get;
            private set;
        }
        #endregion

    }
}
