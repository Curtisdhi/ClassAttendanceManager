using DrKCrazyAttendance;
using DrKCrazyAttendance.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DrKCrazyAttendance_Instructor
{
    /// <summary>
    /// Interaction logic for Course.xaml
    /// </summary>
    public partial class CourseEditor : Window
    {
        private bool editing;

        public CourseEditor()
        {
            InitializeComponent();
            Course = new Course(Settings.Default.Instructor);
            editing = false;
            List<string> classrooms = Course.GetClassrooms();
            foreach (string classroom in classrooms)
            {
                classroomChoice.Items.Add(classroom);
            }
        }

        public CourseEditor(Course course) {
            InitializeComponent();
            this.Course = course;
            this.DataContext = this;
            //if course id is 0, must be not be persisted.
            if (Course.Id != 0)
                editing = true;

            //txtCourse.Text = Course.CourseName;
            /*txtSection.Text = Course.Section;
            startDatePicker.SelectedDate = Course.StartDate;
            endDatePicker.SelectedDate = Course.EndDate;
            startTimePicker.Value = Course.StartTime;
            endTimePicker.Value = Course.EndTime;
            chkEnableTardy.IsChecked = Course.LogTardy;
            gracePeriodTS.Value = Course.GracePeriod;*/

            foreach (DayOfWeek day in Course.Days)
            {
                switch (day)
                {
                    case DayOfWeek.Monday:
                        chkMonday.IsChecked = true;
                        break;
                    case DayOfWeek.Tuesday:
                        chkTuesday.IsChecked = true;
                        break;
                    case DayOfWeek.Wednesday:
                        chkWednesday.IsChecked = true;
                        break;
                    case DayOfWeek.Thursday:
                        chkThursday.IsChecked = true;
                        break;
                    case DayOfWeek.Friday:
                        chkFriday.IsChecked = true;
                        break;
                    case DayOfWeek.Saturday:
                        chkSaturday.IsChecked = true;
                        break;
                }
            }

            List<string> classrooms = Course.GetClassrooms();
            foreach (string classroom in classrooms)
            {
                classroomChoice.Items.Add(classroom);
            }
            //classroomChoice.SelectedIndex = classroomChoice.Items.IndexOf(Course.Classroom);

        }

        #region Properties
        public Course Course
        {
            get;
            private set;
        }
        #endregion

        private void resetForm()
        {
            chkEnableTardy.IsChecked = false;
            chkMonday.IsChecked = false;
            chkTuesday.IsChecked = false;
            chkWednesday.IsChecked = false;
            chkThursday.IsChecked = false;
            chkFriday.IsChecked = false;
            chkSaturday.IsChecked = false;
            classroomChoice.SelectedIndex = -1;
            txtCourse.Text = "";
            txtSection.Text = "";
            startDatePicker.SelectedDate = null;
            endDatePicker.SelectedDate = null;
            startTimePicker.Value = null;
            endTimePicker.Value = null;
        }

        private void btnResetCourse_Click(object sender, RoutedEventArgs e)
        {
            resetForm();
        }

        /*
         * A quick method to determine if a checkbox is checked.
         */
        private bool IsChecked(CheckBox chk)
        {
            bool isChecked = false;
            if (chk.IsChecked.HasValue)
                isChecked = (bool)chk.IsChecked;
            return isChecked;
        }

        private DateTime GetDateTime(DateTime? dTime)
        {
            DateTime dateTime = DateTime.MinValue;
            if (dTime.HasValue)
                dateTime = (DateTime)dTime;
            return dateTime;
        }

        private TimeSpan GetTimeSpan(TimeSpan? ts)
        {
            TimeSpan timeSpan = TimeSpan.Zero;
            if (ts.HasValue)
                timeSpan = (TimeSpan)ts;
            return timeSpan;
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (classroomChoice.SelectedItem == null)
            {
                Course.Classroom = classroomChoice.Text;
            }
            else
            {
                Course.Classroom = classroomChoice.SelectedItem.ToString();
            }
            Course.CourseName = txtCourse.Text;
            Course.Section = txtSection.Text;

            Course.StartDate = GetDateTime(startDatePicker.SelectedDate);
            Course.EndDate = GetDateTime(endDatePicker.SelectedDate);
            Course.StartTime = GetDateTime(startTimePicker.Value);
            Course.EndTime = GetDateTime(endTimePicker.Value);

            Course.GracePeriod = GetTimeSpan(gracePeriodTS.Value);
            Course.LogTardy = IsChecked(chkEnableTardy);

            Course.Days.Clear();
            if (IsChecked(chkMonday))
                Course.Days.Add(DayOfWeek.Monday);
            if (IsChecked(chkTuesday))
                Course.Days.Add(DayOfWeek.Tuesday);
            if (IsChecked(chkWednesday))
                Course.Days.Add(DayOfWeek.Wednesday);
            if (IsChecked(chkThursday))
                Course.Days.Add(DayOfWeek.Thursday);
            if (IsChecked(chkFriday))
                Course.Days.Add(DayOfWeek.Friday);
            if (IsChecked(chkSaturday))
                Course.Days.Add(DayOfWeek.Saturday);

            if (editing)
            {
                MainWindow.Instance.lstCourses.Items.Refresh();
                Course.Update(Course);
            }
            else
            {
                MainWindow.Instance.lstCourses.Items.Add(Course);
                Course.Add(Course);
            }

            Close();

        }

        private void chkEnableTardy_Click(object sender, RoutedEventArgs e)
        {
            gracePeriodTS.IsEnabled = IsChecked(chkEnableTardy);
        }

    }
}
