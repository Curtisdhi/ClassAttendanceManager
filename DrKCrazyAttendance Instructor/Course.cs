using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrKCrazyAttendance_Instructor
{
    public class Course
    {
        public Course()
        {
            Days = new List<DayOfWeek>();
        }

        public Course(string classroom) : this()
        {
            this.ClassRoom = classroom;
        }

        public Course(int id, string classroom, string courseName, string section,
            string semester, List<DayOfWeek> days, DateTime startDate, DateTime endDate, DateTime startTime,
            DateTime endTime, bool logTardy, TimeSpan gracePeriod)
        {
            this.Id = id;
            this.Days = days;
            this.ClassRoom = classroom;
            this.CourseName = courseName;
            this.Section = section;
            this.Semester = semester;
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

        
    }
}
