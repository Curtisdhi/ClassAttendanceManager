using DrKCrazyAttendance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DrKCrazyAttendance_Instructor.ViewModels
{
    public class AttendanceViewModel
    {
        public ICommand ToggleAttendanceCommand { get; set; }
        public ICommand ToggleTardinessCommand { get; set; }

        public AttendanceViewModel(Course course, Student student, List<Attendance> attendances)
        {
            this.Attendances = attendances;
            this.Course = course;
            this.Student = student;
            this.AttendsToCourse = GetAttendsToCourse();
            this.ToggleAttendanceCommand = new RelayCommand(new Action<object>(ToggleAttendance));
            this.ToggleTardinessCommand = new RelayCommand(new Action<object>(ToggleTardiness));
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

        public ObservableCollection<Property> AttendsToCourse
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
        public ObservableCollection<Property> GetAttendsToCourse()
        {
            ObservableCollection<Property> attendance = new ObservableCollection<Property>();
            DateTime[] meetings = Course.GetClassMeetings();
            //this array is assumed to be in order, but is never be assumed
            //to be the same length as classmeetings.
            //DateTime[] attends = GetAttendedDateTimes();
            
            for (int i = 0, a = 0; i < meetings.Length; i++)
            {
                bool[] attended = new bool[2];
                if (a < Attendances.Count)
                {
                    //Compare only the dates
                    attended[0] = meetings[i].Date.Equals(Attendances[a].TimeLog.Date);
                    if (attended[0])
                    {
                        //if true, the student attended this date, so advance.
                        attended[1] = Attendances[a].IsTardy;
                        a++;
                    }
                }
                attendance.Add(new Property(meetings[i].ToString(), attended, this));
            }
            return attendance;
        }

        private void ToggleAttendance(object sender)
        {
            //WARNING: This is a hack to make this shit work........
            //This code COULD and WILL break if VALUE is not a BOOL array
            Property prop = (Property)sender;
            bool[] values = (bool[])prop.Value;
            //toggle attendance
            values[0] = !values[0];
            //set tardiness to false
            values[1] = false;
            prop.Value = values;

            DateTime date = GetDateWithCourseTime(DateTime.Parse(prop.Name));
            Console.WriteLine(date);

            //add attendance
            if (values[0])
            {
                Attendance a = new Attendance(Course, Student, "", date, values[1]);
                Attendances.Add(a);
                Attendance.Add(a);
            }
            //remove attendance
            else
            {
                Attendance a = GetAttendanceByDate(date);
                Attendances.Remove(a);
                Attendance.Remove(a);
            }

        }

        private void ToggleTardiness(object sender)
        {
            //WARNING: This is a hack to make this shit work........
            //This code COULD and WILL break if VALUE is not a BOOL array
            Property prop = (Property)sender;
            bool[] values = (bool[])prop.Value;

            //update attendance record IF it exists
            if (values[0])
            {
                //toggle tardiness
                values[1] = !values[1];
                prop.Value = values;

                DateTime date = GetDateWithCourseTime(DateTime.Parse(prop.Name));
                Attendance a = GetAttendanceByDate(date);
                a.IsTardy = values[1];
                Attendance.Update(a);
                
            }

        }

        private Attendance GetAttendanceByDate(DateTime date)
        {
            return (from at in Attendances
                    where at.TimeLog.Date == date.Date
                    select at).FirstOrDefault();
        }

        private DateTime GetDateWithCourseTime(DateTime date)
        {
            return date.Add(Course.StartTime.TimeOfDay);
        }

        
    }
}
