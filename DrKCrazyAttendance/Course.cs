using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrKCrazyAttendance
{
    public class Course : IDataErrorInfo
    {
        public Course()
        {
            Days = new List<DayOfWeek>();
        }

        public Course(string instructor) : this()
        {
            this.Instructor = instructor;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
        }

        public Course(long id, string classroom, string courseName, string section,
            string instructor, List<DayOfWeek> days, DateTime startDate, DateTime endDate, DateTime startTime,
            DateTime endTime, bool logTardy, TimeSpan gracePeriod)
        {
            this.Id = id;
            this.Days = days;
            this.Classroom = classroom;
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

        /// <summary>
        /// Clones a Course object
        /// </summary>
        /// <param name="course"></param>
        public Course(Course course) : this(0, course.Classroom, course.CourseName, course.Section,
            course.Instructor, new List<DayOfWeek>(course.Days), course.StartDate, course.EndDate,
            course.StartTime, course.EndTime, course.LogTardy, course.GracePeriod)
        {

        }

        #region Properties
        public long Id
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

        public string Classroom
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

        #region Methods
        //gets every datetime with in the range of start and end dates
        public DateTime[] GetClassMeetings()
        {
            int numberOfDays = EndDate.Subtract(StartDate).Days + 1;
            DateTime[] dates = Enumerable.Range(0, numberOfDays)
                      .Select(i => StartDate.AddDays(i))
                      .Where(d => Days.Contains(d.DayOfWeek))
                      .ToArray<DateTime>();
            return dates;
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
        public Course GetCourse(string name, string section)
        {
            Course course = null;

            string query = @"SELECT * FROM Courses WHERE name = @name AND section = @section";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@name", name);
            parameters.Add("@section", section);

            DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);
            List<Course> courses = GetCoursesFromTable(table);
            if (courses.Count > 0)
            {
                course = courses[0];
            }
            return course;
        }

        public Course GetCourse(long id)
        {
            Course course = null;

            string query = @"SELECT * FROM Courses WHERE id = @id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@id", id);

            DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);
            List<Course> courses = GetCoursesFromTable(table);
            if (courses.Count > 0)
            {
                course = courses[0];
            }
            return course;
        }

        public static List<Course> GetCoursesByInstructor(string instructor)
        {
            string query = @"SELECT * FROM Courses WHERE instructor = @instructor ORDER BY classroom, name, section";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@instructor", instructor);

            DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);
            return GetCoursesFromTable(table);
        }

        public static List<Course> GetCoursesByClassroom(string classroom)
        {
            string query = @"SELECT * FROM Courses WHERE classroom = @class ORDER BY name, section";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@class", classroom);

            DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);

            return GetCoursesFromTable(table);
        }

        public static Course GetCoursesByTime(string classroom, DateTime time)
        {
            Course course = null;
            string query = @"SELECT * FROM Courses 
                    WHERE classroom = @class AND (@time BETWEEN startTime AND endTime) 
                    ORDER BY name, section";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@class", classroom);
            parameters.Add("@time", time.TimeOfDay.ToString());

            DataTable table = DatabaseManager.GetDataTableFromQuery(query, parameters);

            List<Course> courses = GetCoursesFromTable(table);
            foreach (Course c in courses)
            {
                //return the first course that meets on this day
                if (c.Days.Contains(time.DayOfWeek))
                {
                    course = c;
                    break;
                }
            }

            return course;
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
            parameters.Add("@class", Classroom);
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
            //insert into db
            string query = @"INSERT INTO Courses(classroom, name, section, instructor, days, 
                startDate, endDate, startTime, endTime, logTardy, gracePeriod) VALUES 
                (@class, @name, @section, @instructor, @days, @startDate, @endDate, @startTime, 
                @endTime, @logTardy, @gracePeriod)";
            foreach (Course course in courses)
            {
                //set the id to the last inserted id
                course.Id = DatabaseManager.ExecuteQuery(query, course.GetQueryParameters());
            }
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
            string query = @"DELETE FROM Courses WHERE id=@id";
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
            string query = @"UPDATE Courses SET classroom=@class, name=@name, section=@section, instructor=@instructor,
                days=@days, startDate=@startDate, endDate=@endDate, startTime=@startTime, endTime=@endTime,
                logTardy=@logTardy, gracePeriod=@gracePeriod WHERE id=@id";
            foreach (Course course in courses)
            {
                parameters.Add(course.GetQueryParameters());
            }
            DatabaseManager.ExecuteQuery(query, parameters);

        }

        public static List<string> GetClassrooms()
        {
            List<string> classrooms = new List<string>();
            string query = @"SELECT DISTINCT classroom FROM Courses ORDER BY classroom";
            MySqlDataReader rdr = null;
            using (rdr = DatabaseManager.GetDataReaderFromQuery(query))
            {
                try
                {
                    if (rdr != null)
                    {
                        while (rdr.Read())
                        {
                            classrooms.Add(rdr.GetString(0));
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Mysql Error {0}", ex);
                }
            }
            return classrooms;
        }
        #endregion

        #region IDataError
        string IDataErrorInfo.Error { 
            get { throw new NotImplementedException(); } 
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string result = null;
                switch (propertyName.ToLower()) { 
                    case "classroom":
                        if (string.IsNullOrWhiteSpace(Classroom))
                        {
                            result = "Classroom is required";
                        }
                        break;
                    case "coursename":
                        if (string.IsNullOrWhiteSpace(CourseName))
                        {
                            result = "Course name is required";
                        }
                        break;
                    case "section":
                        if (string.IsNullOrWhiteSpace(Section))
                        {
                            result = "Section is required";
                        }
                        break;
                    case "startdate":
                        if (StartDate == DateTime.MinValue)
                        {
                            result = "Start Date is required";
                        }
                        break;
                    case "enddate":
                        if (EndDate == DateTime.MinValue)
                        {
                            result = "End Date is required";
                        }
                        break;
                    case "graceperiod":
                        if (LogTardy && GracePeriod == TimeSpan.MinValue)
                        {
                            result = "Grace Peroid is required";
                        }
                        break;
                }

                return result;
            }
        }
        #endregion
    }
}
