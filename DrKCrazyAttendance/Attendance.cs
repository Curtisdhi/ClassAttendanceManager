using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrKCrazyAttendance
{
    public class Attendance
    {

        public Attendance(int id, Course course, Student student, string computerIPv4, DateTime timeLog, bool isTardy) 
        {
            this.Id = id;
            this.Course = course;
            this.Student = student;
            this.ComputerIPv4 = computerIPv4;
            this.TimeLog = timeLog;
            this.IsTardy = isTardy;
        }
        public Attendance(Course course, Student student, string computerIPv4, DateTime timeLog, bool isTardy) 
            : this (0, course, student, computerIPv4, timeLog, isTardy) 
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

        public string ComputerIPv4
        {
            get;
            private set;
        }

        public DateTime TimeLog
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

        #region Sql Methods
        public static List<Attendance> GetAttendancesByClassroom(string classroom)
        {
            string query = @"SELECT * FROM Attendances AS a 
                INNER JOIN Courses AS c 
                    ON a.courseId = c.id
                INNER JOIN Students AS s
                    ON a.studentId = s.id
                WHERE c.classroom = @classroom";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@classroom", classroom);
            DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);

            return GetAttendancesFromTable(table);
        }

        public static List<Attendance> GetAttendancesByInstructor(string instructor)
        {
            string query = @"SELECT * FROM Attendances AS a 
                INNER JOIN Courses AS c 
                    ON a.courseId = c.id
                INNER JOIN Students AS s
                    ON a.studentId = s.id
                WHERE c.instructor = @instructor";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@instructor", instructor);
            DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);

            return GetAttendancesFromTable(table);
        }

        public static List<Attendance> GetAttendancesByCourseId(int courseId) {
            string query = @"SELECT * FROM Attendances AS a 
                INNER JOIN Courses AS c 
                    ON a.courseId = c.id
                INNER JOIN Students AS s
                    ON a.studentId = s.id
                WHERE c.id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", courseId);
            DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);

            return GetAttendancesFromTable(table);
        }

        public static List<Attendance> GetAttendancesByStudentId(int studentId)
        {
            string query = @"SELECT * FROM Attendances AS a 
                INNER JOIN Courses AS c 
                    ON a.courseId = c.id
                INNER JOIN Students AS s
                    ON a.studentId = s.id
                WHERE s.id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", studentId);
            DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);

            return GetAttendancesFromTable(table);
        }

        public static List<Attendance> GetAttendancesFromTable(DataTable table)
        {
            List<Attendance> attendances = new List<Attendance>();
            foreach (DataRow row in table.Rows)
            {
                attendances.Add(GetAttendanceFromDataRow(row));
            }
            return attendances;
        }

        public static Attendance GetAttendanceFromDataRow(DataRow row) 
        {
            Attendance attendance = null;
            foreach (DataColumn col in row.Table.Columns)
            {
                Console.WriteLine(col.ColumnName);
            }
            /* get course from sql query */
            //Convert freindly days into list of days
            List<DayOfWeek> days = Course.GetDaysFromFriendly(row["days"].ToString());
            Course course = new Course(int.Parse(row["id1"].ToString()), 
                row["classroom"].ToString(), row["name"].ToString(), row["section"].ToString(), 
                row["instructor"].ToString(), days,
                DateTime.Parse(row["startDate"].ToString()), DateTime.Parse(row["endDate"].ToString()), 
                DateTime.Parse(row["startTime"].ToString()), DateTime.Parse(row["endTime"].ToString()), 
                bool.Parse(row["logTardy"].ToString()), TimeSpan.Parse(row["gracePeriod"].ToString())
                );

            /* get student from sql query */
            Student student = new Student(int.Parse(row["id2"].ToString()), row["username"].ToString());
            attendance = new Attendance(int.Parse(row["id"].ToString()), 
                course, 
                student, 
                row["computerIPv4"].ToString(),
                DateTime.Parse(row["timeLog"].ToString()), 
                bool.Parse(row["isTardy"].ToString())
                );
            
            return attendance;
        }
        #endregion
    }
}
