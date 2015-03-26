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

        public AttendanceViewModel(Course course, Student student, List<Attendance> attendances)
        {
            this.Attendances = attendances;
            this.Course = course;
            this.Student = student;
            this.AttendsToCourse = GetAttendsToCourse();
            this.ToggleAttendanceCommand = new RelayCommand(new Action<object>(ToggleAttendance));
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
            values[0] = !values[0];
            prop.Value = values;

            DateTime date = DateTime.Parse(prop.Name);
            Console.WriteLine(Student.Username);

        }
    }
}
