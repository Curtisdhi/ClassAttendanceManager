using DrKCrazyAttendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrKCrazyAttendance_Instructor
{
    public class AttendanceViewModel
    {
        public AttendanceViewModel(Course course, Student student, List<Attendance> attendances)
        {
            this.Attendances = attendances;
            this.Course = course;
            this.Student = student;
        }

        #region properties
        public List<Attendance> Attendances
        {
            get;
            private set;
        }

        public Student Student
        {
            get;
            private set;
        }

        public Course Course
        {
            get;
            private set;
        }
        #endregion

        //gets all attended dates from all attendances
        public DateTime[] GetAttendedDateTimes()
        {
            var dates = from attends in Attendances
                        select attends.TimeLog;
            return dates.ToArray<DateTime>();
        }
    }
}
