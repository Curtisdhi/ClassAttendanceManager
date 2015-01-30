using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrKCrazyAttendance
{
    public class Attendance
    {

        public Attendance(int id, Course course, Student student, DateTime logDateTime, bool isTardy) 
        {
            this.Id = id;
            this.Course = course;
            this.Student = student;
            this.LogDateTime = logDateTime;
            this.IsTardy = isTardy;
        }
        public Attendance(Course course, Student student, DateTime logDateTime, bool isTardy) 
            : this (0, course, student, logDateTime, isTardy) 
        {

        }

        #region properties
        public int Id
        {
            get;
            private set;
        }
        public Course Course
        {
            get;
            private set;
        }

        public Student Student
        {
            get;
            private set;
        }

        public DateTime LogDateTime
        {
            get;
            private set;
        }

        public bool IsTardy
        {
            get;
            private set;
        }
        #endregion

        public static List<Attendance> GetAttendancesByClassroom(string classroom)
        {
            string query = @"SELECT FROM Attendances AS a 
                INNER JOIN Courses AS c 
                    ON a.courseId = c.id
                INNER JOIN Students AS s
                    ON a.studentId = s.id
                WHERE c.classroom = @classroom";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@classroom", classroom);
            MySqlDataReader rdr = DatabaseManager.GetDataReaderFromQuery(query, parameters);
            List<Attendance> attendances = GetAttendancesFromSql(rdr);

            return attendances;
        }

        public static List<Attendance> GetAttendancesByCourseId(int courseId) {
            string query = @"SELECT FROM Attendances AS a 
                INNER JOIN Courses AS c 
                    ON a.courseId = c.id
                INNER JOIN Students AS s
                    ON a.studentId = s.id
                WHERE c.id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", courseId);
            MySqlDataReader rdr = DatabaseManager.GetDataReaderFromQuery(query, parameters);
            List<Attendance> attendances = GetAttendancesFromSql(rdr);

            return attendances;
        }

        public static List<Attendance> GetAttendancesByStudentId(int studentId)
        {
            string query = @"SELECT FROM Attendances AS a 
                INNER JOIN Courses AS c 
                    ON a.courseId = c.id
                INNER JOIN Students AS s
                    ON a.studentId = s.id
                WHERE s.id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", studentId);
            MySqlDataReader rdr = DatabaseManager.GetDataReaderFromQuery(query, parameters);
            List<Attendance> attendances = GetAttendancesFromSql(rdr);

            return attendances;
        }

        public static List<Attendance> GetAttendancesFromSql(MySqlDataReader rdr) 
        {
            List<Attendance> attendances = new List<Attendance>();
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                Course course = CourseManager.Instance.GetCourse(rdr.GetInt32(1));
                Student student = StudentManager.GetStudent(rdr.GetInt32(2));
                Attendance attendance = new Attendance(id, course, student, logDateTime, isTardy);
            }
            return attendances;
        }
    }
}
