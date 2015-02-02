using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrKCrazyAttendance
{
    public class Course
    {
        public Course()
        {
            Days = new List<DayOfWeek>();
        }

        public Course(string instructor) : this()
        {
            this.Instructor = instructor;
        }

        public Course(int id, string classroom, string courseName, string section,
            string instructor, List<DayOfWeek> days, DateTime startDate, DateTime endDate, DateTime startTime,
            DateTime endTime, bool logTardy, TimeSpan gracePeriod)
        {
            this.Id = id;
            this.Days = days;
            this.ClassRoom = classroom;
            this.CourseName = courseName;
            this.Section = section;
            this.Instructor = instructor;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.GracePeriod = gracePeriod;
            this.LogTardy = logTardy;
        }

        #region Properties
        public int Id
        {
            get;
            private set;
        }

        public List<DayOfWeek> Days
        {
            get;
            set;
        }

        public string FriendlyDays
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                Days.Sort();
                foreach (DayOfWeek day in Days)
                {
                    switch (day)
                    {
                        case DayOfWeek.Monday:
                            sb.Append("M");
                            break;
                        case DayOfWeek.Tuesday:
                            sb.Append("T");
                            break;
                        case DayOfWeek.Wednesday:
                            sb.Append("W");
                            break;
                        case DayOfWeek.Thursday:
                            sb.Append("R");
                            break;
                        case DayOfWeek.Friday:
                            sb.Append("F");
                            break;
                        case DayOfWeek.Saturday:
                            sb.Append("S");
                            break;
                    }
                }
                return sb.ToString();
            }
        }

        public string Year
        {
            get
            {
                return StartDate.Year.ToString();
            }
        }

        public string ClassRoom
        {
            get;
            set;
        }

        public string CourseName
        {
            get;
            set;
        }

        public string Instructor
        {
            get;
            set;
        }

        public string Section
        {
            get;
            set;
        }

        public DateTime StartDate
        {
            get;
            set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }

        public DateTime StartTime
        {
            get;
            set;
        }

        public DateTime EndTime
        {
            get;
            set;
        }


        public TimeSpan GracePeriod
        {
            get;
            set;
        }

        public bool LogTardy
        {
            get;
            set;
        }
        #endregion

        public static List<DayOfWeek> GetDaysFromFriendly(string fDays)
        {
            List<DayOfWeek> days = new List<DayOfWeek>();
            foreach (char day in fDays)
            {
                switch (day)
                {
                    case 'M':
                        days.Add(DayOfWeek.Monday);
                        break;
                    case 'T':
                        days.Add(DayOfWeek.Tuesday);
                        break;
                    case 'W':
                        days.Add(DayOfWeek.Wednesday);
                        break;
                    case 'R':
                        days.Add(DayOfWeek.Thursday);
                        break;
                    case 'F':
                        days.Add(DayOfWeek.Friday);
                        break;
                    case 'S':
                        days.Add(DayOfWeek.Saturday);
                        break;
                }
            }
            return days;
        }

        #region Sql methods
        public Course GetCourse(string courseName, string section)
        {
            throw new NotImplementedException();
        }

        public Course GetCourse(int id)
        {
            throw new NotImplementedException();
        }

        public static List<Course> GetCoursesByInstructor(string instructor)
        {
            List<Course> courses = new List<Course>();

            string query = "SELECT * FROM Courses WHERE intructor = @instructor";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@intructor", instructor);

            DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);
            return GetCoursesFromTable(table);
        }

        public static List<Course> GetCoursesByClassroom(string classroom)
        {
            string query = "SELECT * FROM Courses WHERE classroom = @class";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@class", classroom);

            DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);

            return GetCoursesFromTable(table);
        }

        public static List<Course> GetCoursesFromTable(DataTable table)
        {
            List<Course> courses = new List<Course>();
            foreach (DataRow row in table.Rows)
            {
                courses.Add(GetCourseFromRow(row));
            }
            return courses;
        }

        public static Course GetCourseFromRow(DataRow row)
        {
            int id = int.Parse(row["id"].ToString());
            string cRoom = row["classroom"].ToString();
            string courseName = row["name"].ToString();
            string section = row["section"].ToString();
            string instructor = row["instructor"].ToString();
            //Convert freindly days into list of days
            List<DayOfWeek> days = Course.GetDaysFromFriendly(row["days"].ToString());

            DateTime startDate = DateTime.Parse(row["startDate"].ToString());
            DateTime endDate = DateTime.Parse(row["endDate"].ToString());
            DateTime startTime = DateTime.Parse(row["startTime"].ToString());
            DateTime endTime = DateTime.Parse(row["endTime"].ToString());
            bool logTardy = bool.Parse(row["logTardy"].ToString());
            TimeSpan gracePeriod = TimeSpan.Parse(row["gracePeriod"].ToString());

            return new Course(id, cRoom, courseName, section, instructor, days,
                startDate, endDate, startTime, endTime, logTardy, gracePeriod);
        }

        public Dictionary<string, object> GetQueryParameters()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            if (Id != 0)
                parameters.Add("@id", Id);
            parameters.Add("@class", ClassRoom);
            parameters.Add("@name", CourseName);
            parameters.Add("@instructor", Instructor);
            parameters.Add("@section", Section);
            parameters.Add("@days", FriendlyDays);
            parameters.Add("@startDate", StartDate);
            parameters.Add("@endDate", EndDate);
            parameters.Add("@startTime", StartTime);
            parameters.Add("@endTime", EndTime);
            parameters.Add("@logTardy", LogTardy);
            parameters.Add("@gracePeriod", GracePeriod);
            return parameters;
        }

        public static void Add(Course course)
        {
            List<Course> courses = new List<Course>();
            courses.Add(course);
            Add(courses);
        }

        public static void Add(List<Course> courses)
        {
            List<Dictionary<string, object>> parameters = new List<Dictionary<string,object>>();
            //insert into db
            string query = "INSERT INTO Courses(classroom, name, section, semester, days," +
                "startDate, endDate, startTime, endTime, logTardy, gracePeriod) VALUES (@class, @name, @section," +
                "@semester, @days, @startDate, @endDate, @startTime, @endTime, @logTardy, @gracePeriod)";
            foreach (Course course in courses)
            {
                parameters.Add(course.GetQueryParameters());
            }
            DatabaseManager.ExecuteQuery(query, parameters);
        }

        public static void Remove(Course course)
        {
            List<Course> courses = new List<Course>();
            courses.Add(course);
            Remove(courses);
        }

        public static void Remove(List<Course> courses)
        {
            List<Dictionary<string, object>> parameters = new List<Dictionary<string, object>>();
            string query = "DELETE FROM Courses WHERE id=@id";
            foreach (Course course in courses)
            {
                Dictionary<string, object> param = new Dictionary<string, object>();
                parameters.Add(param);
                param.Add("@id", course.Id);
            }
            DatabaseManager.ExecuteQuery(query, parameters);
        }

        public static void Update(Course course)
        {
            List<Course> courses = new List<Course>();
            courses.Add(course);
            Update(courses);
        }

        public static void Update(List<Course> courses)
        {
            List<Dictionary<string, object>> parameters = new List<Dictionary<string, object>>();
            //refresh the list to display the updated item
            string query = "UPDATE Courses SET classroom=@class, name=@name, section=@section, semester=@semester," +
                "days=@days, startDate=@startDate, endDate=@endDate, startTime=@startTime, endTime=@endTime," +
                "logTardy=@logTardy, gracePeriod=@gracePeriod WHERE id=@id";
            foreach (Course course in courses)
            {
                parameters.Add(course.GetQueryParameters());
            }
            DatabaseManager.ExecuteQuery(query, parameters);

        }

        public static List<string> GetClassrooms()
        {
            List<string> classrooms = new List<string>();
            string query = "SELECT classroom FROM Courses";
            MySqlDataReader rdr = null;
            using (rdr = DatabaseManager.GetDataReaderFromQuery(query))
            {
                while (rdr.Read())
                {
                    classrooms.Add(rdr.GetString(0));
                }
            }
            return classrooms;
        }
        #endregion
        
    }
}
