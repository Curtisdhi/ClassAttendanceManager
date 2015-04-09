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

        public Attendance(long id, Course course, Student student, string computerIPv4, DateTime timeLog, bool isTardy) 
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
        public long Id
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
            set;
        }

        public bool IsTardy
        {
            get;
            set;
        }
        #endregion

        #region Sql Methods
        public Dictionary<string, object> GetQueryParameters()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            if (Id != 0)
                parameters.Add("@id", Id);
            parameters.Add("@courseId", Course.Id);
            parameters.Add("@studentId", Student.Id);
            parameters.Add("@ip", ComputerIPv4);
            parameters.Add("@timeLog", TimeLog);
            parameters.Add("@isTardy", IsTardy);
            return parameters;
        }

        public static bool HasAttended(Attendance attendance)
        {
            //if datediff is greater than 0, than we never recorded an attendance!
            string query = @"SELECT COUNT(id) FROM Attendances
                WHERE courseId = @cId AND studentId = @stuId
                    AND DATEDIFF(timeLog, @date) = 0";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@cId", attendance.Course.Id);
            parameters.Add("@stuId", attendance.Student.Id);
            parameters.Add("@date", attendance.TimeLog);

            return DatabaseManager.ExecuteScalar(query, parameters) > 0;
        }

        public static void Add(Attendance attendance)
        {
            string query = @"INSERT INTO Attendances (courseId, studentId, computerIPv4, timeLog, isTardy)
                VALUES (@courseId, @studentId, @ip, @timeLog, @isTardy)";
            attendance.Id = DatabaseManager.ExecuteQuery(query, attendance.GetQueryParameters());
        }

        public static void Update(Attendance attendance)
        {
            List<Attendance> attendances = new List<Attendance>();
            attendances.Add(attendance);
            Update(attendances);
        }

        public static void Update(List<Attendance> attendances)
        {
            List<Dictionary<string, object>> parameters = new List<Dictionary<string, object>>();
            string query = @"UPDATE Attendances SET courseId=@courseId, studentId=@studentId, computerIPv4=@ip,
                timeLog=@timeLog, isTardy=@isTardy WHERE id=@id";
            foreach (Attendance attendance in attendances)
            {
                parameters.Add(attendance.GetQueryParameters());
            }
            DatabaseManager.ExecuteQuery(query, parameters);
        }

        public static void Remove(Attendance attendance)
        {
            List<Attendance> attendances = new List<Attendance>();
            attendances.Add(attendance);
            Remove(attendances);
        }

        public static void Remove(List<Attendance> attendances)
        {
            List<Dictionary<string, object>> parameters = new List<Dictionary<string, object>>();
            string query = @"DELETE FROM Attendances WHERE id=@id";
            foreach (Attendance attendance in attendances)
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                parameters.Add(param);
                param.Add("@id", attendance.Id);
            }
            DatabaseManager.ExecuteQuery(query, parameters);
        }

        public static List<Attendance> GetAttendancesByClassroom(string classroom)
        {
            string query = @"SELECT * FROM Attendances AS a 
                INNER JOIN Courses AS c 
                    ON a.courseId = c.id
                INNER JOIN Students AS s
                    ON a.studentId = s.id
                WHERE c.classroom = @classroom
                ORDER BY s.id, a.TimeLog";
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
                WHERE c.instructor = @instructor
                ORDER BY a.studentId, a.TimeLog";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@instructor", instructor);
            DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);

            return GetAttendancesFromTable(table);
        }

        /*
         * Datatable doesn't work well with this function for unknown reasons.
         * We also need to "group by" to eliminate any chance of "dupilicates"
         * that cause major issues with the data reader.
         */
        public static List<Attendance> GetAttendancesByCourseId(long courseId) {
            string query = @"SELECT a.id, a.studentId, a.courseId, a.computerIPv4, a.timeLog, a.isTardy,
                c.id AS id1, c.name, c.section, c.classroom, c.instructor, c.startDate, 
                    c.endDate, c.startTime, c.endTime, c.days, c.logTardy, c.gracePeriod,
                s.id AS id2, s.username 
                    FROM Attendances AS a 
                INNER JOIN Courses AS c 
                    ON a.courseId = c.id
                INNER JOIN Students AS s
                    ON a.studentId = s.id
                WHERE c.id = @id
                GROUP BY a.TimeLog, a.studentId";
                //ORDER BY a.TimeLog, a.studentId";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", courseId);

            List<Attendance> attendances;
            MySqlDataReader rdr;
            using (rdr = DatabaseManager.GetDataReaderFromQuery(query, parameters))
            {
                attendances = GetAttendancesFromReader(rdr);
            }
            DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);

            return attendances;
        }

        public static List<Attendance> GetAttendancesByStudentId(long studentId)
        {
            List<Attendance> attendances;
            string query = @"SELECT * FROM Attendances AS a 
                INNER JOIN Courses AS c 
                    ON a.courseId = c.id
                INNER JOIN Students AS s
                    ON a.studentId = s.id
                WHERE s.id = @id
                ORDER BY a.studentId, a.TimeLog";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", studentId);
            MySqlDataReader rdr = null;
            
            using (rdr = DatabaseManager.GetDataReaderFromQuery(query, parameters)) {
                attendances = GetAttendancesFromReader(rdr);
            }
            //DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);

            return attendances;//GetAttendancesFromTable(table);
        }

        public static List<Attendance> GetAttendancesFromReader(MySqlDataReader rdr)
        {
            List<Attendance> attendances = new List<Attendance>();
            while (rdr.Read())
            {
                List<DayOfWeek> days = Course.GetDaysFromFriendly(rdr["days"].ToString());
                Course course = new Course(long.Parse(rdr["id1"].ToString()),
                    rdr["classroom"].ToString(), rdr["name"].ToString(), rdr["section"].ToString(),
                    rdr["instructor"].ToString(), days,
                    DateTime.Parse(rdr["startDate"].ToString()), DateTime.Parse(rdr["endDate"].ToString()),
                    DateTime.Parse(rdr["startTime"].ToString()), DateTime.Parse(rdr["endTime"].ToString()),
                    bool.Parse(rdr["logTardy"].ToString()), TimeSpan.Parse(rdr["gracePeriod"].ToString())
                    );

                /* get student from sql query */
                Student student = new Student(long.Parse(rdr["id2"].ToString()), rdr["username"].ToString());
                Attendance attendance = new Attendance(long.Parse(rdr["id"].ToString()),
                    course,
                    student,
                    rdr["computerIPv4"].ToString(),
                    DateTime.Parse(rdr["timeLog"].ToString()),
                    bool.Parse(rdr["isTardy"].ToString())
                    );

                attendances.Add(attendance);
            }
            return attendances;
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
            /*foreach (DataColumn col in row.Table.Columns)
            {
                Console.WriteLine(col.ColumnName);
            }*/
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
