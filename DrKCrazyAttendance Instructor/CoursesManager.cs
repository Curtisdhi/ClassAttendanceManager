using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return Courses;
        }

        public void Add(Course course)
        {
            if (!Courses.Contains(course)) { 
                Courses.Add(course);
                MainWindow.Instance.lstCourses.Items.Add(course);
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
