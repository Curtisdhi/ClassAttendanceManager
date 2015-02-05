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
            this.AttendsToCourse = GetAttendsToCourse();
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

        public Dictionary<string, bool> AttendsToCourse
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

        //get a boolean array that represents the relationship between the course meetings
        //and the student's attendance datetimes.
        public Dictionary<string, bool> GetAttendsToCourse()
        {
            Dictionary<string, bool> attendance = new Dictionary<string, bool>();
            DateTime[] meetings = Course.GetClassMeetings();
            //this array is assumed to be in order, but is never be assumed
            //to be the same length as classmeetings.
            DateTime[] attends = GetAttendedDateTimes();
            
            for (int i = 0, a = 0; i < meetings.Length; i++)
            {
                bool attended = false;
                if (a < attends.Length)
                {
                    attended = meetings[i] == attends[a];
                    if (attended)
                    {
                        //if true, the student attended this date, so advance.
                        a++;
                    }
                }
                attendance.Add(meetings[i].ToString(), attended);
            }
            return attendance;
        }
    }
}
