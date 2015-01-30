using DrKCrazyAttendance.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient; 

namespace DrKCrazyAttendance
{
    public class CourseManager
    {
        private static CourseManager instance;

        private CourseManager(string classroom)
        {
            FetchAll(classroom);
        }

        #region properties
        public static CourseManager Instance
        {
            get {
                if (instance == null)
                {
                    instance = new CourseManager(Settings.Default.Classroom);
                }
                return instance;
            }
        }

        public List<Course> Courses
        {
            get;
            private set;
        }
        #endregion
        
        #region methods
        public List<Course> FetchAll(string classroom)
        {
            Courses = new List<Course>();

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            using (conn = DatabaseManager.Connect())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM Courses WHERE classroom = @class";
                    cmd.Prepare();

                    cmd.Parameters.AddWithValue("@class", classroom);

                    using (rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            Course course = GetCourseFromReader(rdr);
                            Courses.Add(course);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    /*ignore*/
                }
            }

            return Courses;
        }

        public Course GetCourseFromReader(MySqlDataReader rdr)
        {
            int id = rdr.GetInt32(0);
            string cRoom = rdr.GetString(1);
            string courseName = rdr.GetString(2);
            string section = rdr.GetString(3);
            string semester = rdr.GetString(4);
            //Convert freindly days into list of days
            List<DayOfWeek> days = Course.GetDaysFromFriendly(rdr.GetString(5));

            DateTime startDate = rdr.GetDateTime(6);
            DateTime endDate = rdr.GetDateTime(7);
            DateTime startTime = rdr.GetDateTime(8);
            DateTime endTime = rdr.GetDateTime(9);
            bool logTardy = rdr.GetBoolean(10);
            TimeSpan gracePeriod = rdr.GetTimeSpan(11);

            return new Course(id, cRoom, courseName, section, semester, days,
                startDate, endDate, startTime, endTime, logTardy, gracePeriod);
        }

        public void Add(Course course)
        {
            if (!Courses.Contains(course)) { 
                Courses.Add(course);

                //insert into db
                string query = "INSERT INTO Courses(classroom, name, section, semester, days," +
                    "startDate, endDate, startTime, endTime, logTardy, gracePeriod) VALUES (@class, @name, @section," +
                    "@semester, @days, @startDate, @endDate, @startTime, @endTime, @logTardy, @gracePeriod)";
                ExecuteCourseQuery(course, query);
                
            }
            else
            {
                //if the course already exists, we need to go update it.
                Update(course);
            }
        }

        public void Remove(Course course)
        {
            Courses.Remove(course);
            string query = "DELETE FROM Courses WHERE id=@id";
            ExecuteCourseQuery(course, query, false);
        }

        public void Update(Course course)
        {
            //refresh the list to display the updated item
            string query = "UPDATE Courses SET classroom=@class, name=@name, section=@section, semester=@semester,"+
                "days=@days, startDate=@startDate, endDate=@endDate, startTime=@startTime, endTime=@endTime,"+
                "logTardy=@logTardy, gracePeriod=@gracePeriod WHERE id=@id";
            ExecuteCourseQuery(course, query);

        }

        private void ExecuteCourseQuery(Course course, string query) {
            ExecuteCourseQuery(course, query, true);
        }

        private void ExecuteCourseQuery(Course course, string query, bool persist)
        {
            MySqlConnection conn = null;
            using (conn = DatabaseManager.Connect())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = query;
                    cmd.Prepare();

                    if (course.Id != 0)
                        cmd.Parameters.AddWithValue("@id", course.Id);
                    if (persist)
                    {
                        cmd.Parameters.AddWithValue("@class", course.ClassRoom);
                        cmd.Parameters.AddWithValue("@name", course.CourseName);
                        cmd.Parameters.AddWithValue("@section", course.Section);
                        cmd.Parameters.AddWithValue("@semester", course.Semester);
                        cmd.Parameters.AddWithValue("@days", course.FriendlyDays);
                        cmd.Parameters.AddWithValue("@startDate", course.StartDate);
                        cmd.Parameters.AddWithValue("@endDate", course.EndDate);
                        cmd.Parameters.AddWithValue("@startTime", course.StartTime);
                        cmd.Parameters.AddWithValue("@endTime", course.EndTime);
                        cmd.Parameters.AddWithValue("@logTardy", course.LogTardy);
                        cmd.Parameters.AddWithValue("@gracePeriod", course.GracePeriod);
                    }
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    /*ignore*/
                }
            }
        }

        public Course GetCourse(string courseName, string section)
        {
            foreach (Course course in Courses)
            {
                if (course.CourseName.Equals(courseName) && course.Section.Equals(section))
                {
                    return course;
                }
            }
            return null;
        }

        public Course GetCourse(int id)
        {
            foreach (Course course in Courses)
            {
                if (course.Id == id)
                {
                    return course;
                }
            }
            return null;
        }
        #endregion

    }
}
