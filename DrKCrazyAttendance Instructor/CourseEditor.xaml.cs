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
            DataContext = this;
            editing = false;
            List<string> classrooms = Course.GetClassrooms();
            foreach (string classroom in classrooms)
            {
                classroomChoice.Items.Add(classroom);
            }
        }

        public CourseEditor(Course course) {
            InitializeComponent();
            Course = course;
            DataContext = this;
            //if course id is 0, must be not be persisted.
            if (Course.Id != 0)
                editing = true;

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
            String course = txtCourse.Text;
            String classroom = classroomChoice.Text;
            String section = txtSection.Text;


            if (!length(course))
            {
                MessageBox.Show("The course must be 8 characters long exp.CISP1010", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (!upperCases(course))
            {
                MessageBox.Show("The course must start with 4 uppercase letters exp.CISP1010", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else if(!leng(classroom))
            {
                MessageBox.Show("The classroom must be 5 characters long exp.C2424", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!digits(classroom))
            {
                MessageBox.Show("The classroom must end with 4 digits exp.C2424", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!upperCase(classroom))
            {
                MessageBox.Show("The classroom number must start with a uppercase letter", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!upperCase(section))
            {
                MessageBox.Show("The section number must start with a uppercase letter", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!sdigit(section))
            {
                MessageBox.Show("The section number must end with 2 digits", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (!len(section))
            {
                MessageBox.Show("The course must be 3 characters long exp.A01", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (String.IsNullOrEmpty(txtCourse.Text))
            {
                MessageBox.Show("Please Enter a Course ID", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (String.IsNullOrEmpty(txtSection.Text))
            {
                MessageBox.Show("Please Enter a Section", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            else if (String.IsNullOrEmpty(classroomChoice.Text))
            {
                MessageBox.Show("Please Enter a Classroom", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


            else
            {

                if (!IsChecked(chkMonday) && !IsChecked(chkTuesday) && !IsChecked(chkWednesday) && !IsChecked(chkThursday)
                    && !IsChecked(chkFriday) && !IsChecked(chkSaturday))
                {
                    MessageBox.Show("Please Select at least one Day", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
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
            }
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

        private void control_Error(object sender, ValidationErrorEventArgs e)
        {
            
        }

        public static Boolean length(String course)
        {
            if (course.Length == 8)
            {

            }
            else
            {
                return false;
            }
            return true;
        }

             public static Boolean leng(String classroom)
        {
            if (classroom.Length == 5)
            {
                
            }
            else
            {
                return false;
            }
            return true;

        }

             public static Boolean len(String section)
             {
                 if (section.Length == 3)
                 {

                 }
                 else
                 {
                     return false;
                 }
                 return true;

             }
             public static Boolean upperCase(String classroom)
             {

                     if (Char.IsUpper(classroom[0]))
                     {
                         return true;
                     }
                 return false;
             }

             public static Boolean digits(String classroom)
             {

                     if (Char.IsDigit(classroom[1+3]))
                     {
                         return true;
                     }
                 return false;
             }

             public static Boolean upperCases(String course)
             {

                 if (Char.IsUpper(course[0+3]))
                 {
                     return true;
                 }
                 return false;
             }

             public static Boolean sdigit(String section)
             {

                 if (Char.IsDigit(section[1 + 1]))
                 {
                     return true;
                 }
                 return false;
             }
    }
}
