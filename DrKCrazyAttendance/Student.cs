using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
        public Student(long id, string username)
        {
            this.Id = id;
            this.Username = username;
        }

        #region properties
        public long Id
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

        #region Sql Methods
        public static List<Student> GetStudentsByCourse(long courseId) {
            List<Student> students = new List<Student>();
            string query = @"SELECT FROM Students AS s
                                INNER JOIN Attendance AS a
                                ON a.courseId = @courseID";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@courseId", courseId);
            MySqlDataReader rdr = DatabaseManager.GetDataReaderFromQuery(query, parameters);
           
            using (rdr)
            {
                students = GetStudents(rdr);
            }
            return students;
        }

        public static Student GetStudent(string username)
        {
            Student student = null;
            string query = "SELECT FROM Students WHERE username=@username";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@username", username);
            MySqlDataReader rdr = DatabaseManager.GetDataReaderFromQuery(query, parameters);
           
            using (rdr)
            {
                List<Student> students = GetStudents(rdr);
                if (students.Count > 0)
                {
                    student = students[0];
                }
            }
           
            return student;
        }

        public static Student GetStudent(long id)
        {
            Student student = null;
            string query = "SELECT FROM Students WHERE id=@id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);
            MySqlDataReader rdr = DatabaseManager.GetDataReaderFromQuery(query, parameters);
           
            using (rdr)
            {
                List<Student> students = GetStudents(rdr);
                if (students.Count > 0)
                {
                    student = students[0];
                }
            }
           
            return student;
        }

        public static List<Student> GetStudents(MySqlDataReader rdr)
        {
            List<Student> students = null;
            while (rdr.Read())
            {
                Student student = new Student(rdr.GetInt32(0), rdr.GetString(1));
                students.Add(student);
            }
            return students;
        }
    #endregion

    }
}
