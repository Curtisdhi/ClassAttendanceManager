using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrKCrazyAttendance_Instructor
{
    class Courses
    {
        public Courses(string classroom)
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
            Courses.Add(course);
        }

        public void Remove(Course course)
        {
            Courses.Remove(course);
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
