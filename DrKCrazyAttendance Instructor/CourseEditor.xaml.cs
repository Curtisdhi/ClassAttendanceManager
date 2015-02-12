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
        private Course course;
        private Course originalCourse;

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
            get { return course; }
            private set {
                course = value;
                //clone the course so we have the original values to
                //revert back to in the even the user doesn't save.
                //Note in the event something bad happening, this won't be
                //explictly persisted to the DB until the user "saves"
                originalCourse = new Course(value);
            }
        }
        #endregion

        private void ResetForm()
        {
            //Note that "originalCourse" will be a empty default if this form is in
            //non editing mode, but will contain the original values in editing mode.
            chkEnableTardy.IsChecked = originalCourse.LogTardy;
            chkMonday.IsChecked = originalCourse.Days.Contains(DayOfWeek.Monday);
            chkTuesday.IsChecked = originalCourse.Days.Contains(DayOfWeek.Tuesday);
            chkWednesday.IsChecked = originalCourse.Days.Contains(DayOfWeek.Wednesday);
            chkThursday.IsChecked = originalCourse.Days.Contains(DayOfWeek.Thursday);
            chkFriday.IsChecked = originalCourse.Days.Contains(DayOfWeek.Friday);
            chkSaturday.IsChecked = originalCourse.Days.Contains(DayOfWeek.Saturday);
            txtCourse.Text = originalCourse.CourseName;
            txtSection.Text = originalCourse.Section;
            startDatePicker.SelectedDate = (DateTime?)originalCourse.StartDate;
            endDatePicker.SelectedDate = (DateTime?)originalCourse.EndDate;
            startTimePicker.Value = (DateTime?)originalCourse.StartTime;
            endTimePicker.Value = (DateTime?)originalCourse.EndTime;

            classroomChoice.SelectedIndex = classroomChoice.Items.IndexOf(originalCourse.Classroom);

        }

        private void btnResetCourse_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
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
            /*if (classroomChoice.SelectedItem == null)
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
            Course.LogTardy = IsChecked(chkEnableTardy);*/

            //the checkboxes aren't binded, so we must manually deal with it
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
                //update in the DB
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

        private void Window_Closed(object sender, EventArgs e)
        {
            //reset the course to the original settings before closing!
            ResetForm();
        }

    }
}
