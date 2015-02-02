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
        private Course course;
        private bool editing;

        public CourseEditor()
        {
            InitializeComponent();
            course = new Course(Settings.Default.Instructor);
            editing = false;
            List<string> classrooms = Course.GetClassrooms();
            foreach (string classroom in classrooms)
            {
                classroomChoice.Items.Add(classroom);
            }
        }

        public CourseEditor(Course course) {
            InitializeComponent();
            editing = true;

            this.course = course;
            txtCourse.Text = course.CourseName;
            txtSection.Text = course.Section;
            startDatePicker.SelectedDate = course.StartDate;
            endDatePicker.SelectedDate = course.EndDate;
            startTimePicker.Value = course.StartTime;
            endTimePicker.Value = course.EndTime;
            chkEnableTardy.IsChecked = course.LogTardy;
            gracePeriodTS.Value = course.GracePeriod;

            foreach (DayOfWeek day in course.Days)
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
            classroomChoice.SelectedIndex = classroomChoice.Items.IndexOf(course.Classroom);

        }

        private void resetForm()
        {
            chkEnableTardy.IsChecked = false;
            chkMonday.IsChecked = false;
            chkTuesday.IsChecked = false;
            chkWednesday.IsChecked = false;
            chkThursday.IsChecked = false;
            chkFriday.IsChecked = false;
            chkSaturday.IsChecked = false;
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
            course.CourseName = txtCourse.Text;
            course.Section = txtSection.Text;
            course.StartDate = GetDateTime(startDatePicker.SelectedDate);
            course.EndDate = GetDateTime(endDatePicker.SelectedDate);
            course.StartTime = GetDateTime(startTimePicker.Value);
            course.EndTime = GetDateTime(endTimePicker.Value);

            course.GracePeriod = GetTimeSpan(gracePeriodTS.Value);
            course.LogTardy = IsChecked(chkEnableTardy);

            course.Days.Clear();
            if (IsChecked(chkMonday))
                course.Days.Add(DayOfWeek.Monday);
            if (IsChecked(chkTuesday))
                course.Days.Add(DayOfWeek.Tuesday);
            if (IsChecked(chkWednesday))
                course.Days.Add(DayOfWeek.Wednesday);
            if (IsChecked(chkThursday))
                course.Days.Add(DayOfWeek.Thursday);
            if (IsChecked(chkFriday))
                course.Days.Add(DayOfWeek.Friday);
            if (IsChecked(chkSaturday))
                course.Days.Add(DayOfWeek.Saturday);

            if (editing)
            {
                MainWindow.Instance.lstCourses.Items.Refresh();
                Course.Update(course);
            }
            else
            {
                MainWindow.Instance.lstCourses.Items.Add(course);
                Course.Add(course);
            }

            Close();
        }

        private void chkEnableTardy_Click(object sender, RoutedEventArgs e)
        {
            gracePeriodTS.IsEnabled = IsChecked(chkEnableTardy);
        }
    }
}
