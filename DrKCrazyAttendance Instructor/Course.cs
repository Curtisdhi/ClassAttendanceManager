using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrKCrazyAttendance_Instructor
{
    class Course
    {
        public Course(List<DayOfWeek> days, string classroom, string courseName, string section,
            string semester, DateTime startDateTime, DateTime endDateTime, TimeSpan gracePeriod,
            bool logTardy)
        {
            this.Days = days;
            this.ClassRoom = classroom;
            this.CourseName = courseName;
            this.Section = section;
            this.Semester = semester;
            this.StartDateTime = startDateTime;
            this.EndDateTime = endDateTime;
            this.GracePeriod = gracePeriod;
            this.LogTardy = logTardy;
        }

        #region Properties
        public List<DayOfWeek> Days
        {
            get;
            set;
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

        public string Section
        {
            get;
            set;
        }

        public string Semester
        {
            get;
            set;
        }

        public DateTime StartDateTime
        {
            get;
            set;
        }

        public DateTime EndDateTime
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

        public void Insert()
        {

        }

        public void Update()
        {

        }

        public void Delete()
        {

        }

    }
}
