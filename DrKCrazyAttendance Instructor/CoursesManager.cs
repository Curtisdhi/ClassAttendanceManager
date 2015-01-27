using DrKCrazyAttendance_Instructor.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient; 

namespace DrKCrazyAttendance_Instructor
{
    public class CourseManager
    {
        public CourseManager(string classroom)
        {
            FetchAll(classroom);
            
       
        }

        #region properties
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
                            MainWindow.Instance.lstCourses.Items.Add(course);
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
                MainWindow.Instance.lstCourses.Items.Add(course);

                //insert into db
                MySqlConnection conn = null;
                using (conn = DatabaseManager.Connect())
                {
                    try
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO Courses(classroom, name, section, semester, days," +
                        "startDate, endDate, startTime, endTime, logTardy, gracePeriod) VALUES (@class, @name, @section," +
                        "@semester, @days, @startDate, @endDate, @startTime, @endTime, @logTardy, @gracePeriod)";
                        cmd.Prepare();

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
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception) { 
                        /*ignore*/ 
                    }
                }
            }
            else
            {
                //refresh the list to display the updated item
                MainWindow.Instance.lstCourses.Items.Refresh();
            }
        }

        public void Remove(Course course)
        {
            Courses.Remove(course);
            MainWindow.Instance.lstCourses.Items.Remove(course);
        }

        public void Update(Course course)
        {

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
        #endregion

    }
}
