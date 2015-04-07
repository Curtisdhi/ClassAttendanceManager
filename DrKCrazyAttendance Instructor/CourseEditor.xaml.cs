using DrKCrazyAttendance;
using DrKCrazyAttendance.Properties;
using DrKCrazyAttendance_Instructor.ViewModels;
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
            CourseViewModel = new CourseViewModel(new Course(Settings.Default.Instructor));
            DataContext = CourseViewModel;
            editing = false;
            List<string> classrooms = Course.GetClassrooms();
            foreach (string classroom in classrooms)
            {
                classroomChoice.Items.Add(classroom);
            }
        }

        public CourseEditor(Course course)
        {
            InitializeComponent();
            CourseViewModel = new CourseViewModel(course);
            DataContext = CourseViewModel;
            //if course id is 0, must be not be edited.
            if (CourseViewModel.Id != 0)
                editing = true;

            //initalize form
            ResetForm();

            List<string> classrooms = Course.GetClassrooms();
            foreach (string classroom in classrooms)
            {
                classroomChoice.Items.Add(classroom);
            }

        }

        #region Properties
        public CourseViewModel CourseViewModel
        {
            get;
            private set;
        }

        public bool Persisted
        {
            get;
            private set;
        }
        #endregion

        private void ResetForm()
        {
            //Note that "originalCourse" will be a empty default if this form is in
            //non editing mode, but will contain the original values in editing mode.
            chkEnableTardy.IsChecked = CourseViewModel.OriginalCourse.LogTardy;
            gracePeriodTS.IsEnabled = CourseViewModel.OriginalCourse.LogTardy;

            foreach (DayOfWeek day in CourseViewModel.OriginalCourse.Days)
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

            txtCourse.Text = CourseViewModel.OriginalCourse.CourseName;
            txtSection.Text = CourseViewModel.OriginalCourse.Section;
            startDatePicker.SelectedDate = CourseViewModel.OriginalCourse.StartDate;
            endDatePicker.SelectedDate = CourseViewModel.OriginalCourse.EndDate;
            startTimePicker.Value = CourseViewModel.OriginalCourse.StartTime;
            endTimePicker.Value = CourseViewModel.OriginalCourse.EndTime;

            classroomChoice.Text = CourseViewModel.OriginalCourse.Classroom;

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

            string errors = IsValid();
            if (!string.IsNullOrEmpty(errors))
            {
                MessageBox.Show(errors, "Validation errors", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //the checkboxes aren't binded, so we must manually deal with it
                CourseViewModel.Days.Clear();
                if (IsChecked(chkMonday))
                    CourseViewModel.Days.Add(DayOfWeek.Monday);
                if (IsChecked(chkTuesday))
                    CourseViewModel.Days.Add(DayOfWeek.Tuesday);
                if (IsChecked(chkWednesday))
                    CourseViewModel.Days.Add(DayOfWeek.Wednesday);
                if (IsChecked(chkThursday))
                    CourseViewModel.Days.Add(DayOfWeek.Thursday);
                if (IsChecked(chkFriday))
                    CourseViewModel.Days.Add(DayOfWeek.Friday);
                if (IsChecked(chkSaturday))
                    CourseViewModel.Days.Add(DayOfWeek.Saturday);
                try
                {
                    if (editing)
                    {
                        //update in the DB
                        Course.Update(CourseViewModel.Course);
                    }
                    else
                    {
                        Course.Add(CourseViewModel.Course);
                        MainWindow.Instance.lstCourses.Items.Add(CourseViewModel.Course);
                    }
                    Persisted = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to persist course to the database.\n" + ex.Message,
                        "Sql Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Persisted = false;
                }
                finally
                {
                    Close();
                }
            }

        }

        private string IsValid()
        {
            string errors = "";

            if (!CourseViewModel.IsValid)
            {
                errors += "Please correct the errors before continuing.\n";
            }

            if (!IsChecked(chkMonday) && !IsChecked(chkTuesday) && !IsChecked(chkWednesday) && !IsChecked(chkThursday)
                && !IsChecked(chkFriday) && !IsChecked(chkSaturday))
            {
                errors += "Please check at least one Day\n";
            }

            return errors;
        }

        private void chkEnableTardy_Click(object sender, RoutedEventArgs e)
        {
            gracePeriodTS.IsEnabled = IsChecked(chkEnableTardy);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //reset the course to the original settings before closing!
            if (!Persisted)
            {
                ResetForm();
            }
            else
            {
                MainWindow.Instance.lstCourses.Items.Refresh();
            }
        }

        private void txtCourse_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            //four characters and 4 digits
            int length = box.Text.Length;

            if (length > 0)
            {
                if (length <= 4)
                {
                    if (!char.IsLetter(box.Text[length - 1]))
                    {
                        box.Text = box.Text.Substring(0, length - 1);
                        e.Handled = true;
                    }
                    else
                    {
                        box.Text = box.Text.ToUpper();
                        e.Handled = true;
                    }
                }
                else if (length <= 8)
                {
                    if (!char.IsDigit(box.Text[length - 1]))
                    {
                        box.Text = box.Text.Substring(0, length - 1);
                        e.Handled = true;
                    }
                }
                else
                {
                    box.Text = box.Text.Substring(0, length - 1);
                    e.Handled = true;
                }
            }
            box.CaretIndex = box.Text.Length;
        }

        private void txtSection_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            //four characters and 4 digits
            int length = box.Text.Length;


            if (length > 0)
            {
                if (length <= 1)
                {
                    if (!char.IsLetter(box.Text[length - 1]))
                    {
                        box.Text = box.Text.Substring(0, length - 1);
                        e.Handled = true;
                    }
                    else
                    {
                        box.Text = box.Text.ToUpper();
                        e.Handled = true;
                    }
                }
                else if (length <= 3)
                {
                    if (!char.IsDigit(box.Text[length - 1]))
                    {
                        box.Text = box.Text.Substring(0, length - 1);
                        e.Handled = true;
                    }
                }
                else
                {
                    box.Text = box.Text.Substring(0, length - 1);
                    e.Handled = true;
                }
            }
            box.CaretIndex = box.Text.Length;
        }

        private void classroomChoice_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            //1 letter and 4 digits
            int length = box.Text.Length;

            StringBuilder sb = new StringBuilder();

            if (length > 0)
            {
                if (length <= 1)
                {
                    if (!char.IsLetter(box.Text[length - 1]))
                    {
                        box.Text = box.Text.Substring(0, length - 1);
                        e.Handled = true;
                    }
                    else
                    {
                        box.Text = box.Text.ToUpper();
                        e.Handled = true;
                    }
                }
                else if (length <= 5)
                {
                    if (!char.IsDigit(box.Text[length - 1]))
                    {
                        box.Text = box.Text.Substring(0, length - 1);
                        e.Handled = true;
                    }
                }
                else
                {
                    box.Text = box.Text.Substring(0, length - 1);
                    e.Handled = true;
                }
            }
            TextBox txt = box.Template.FindName("PART_EditableTextBox", box) as TextBox;
            txt.CaretIndex = box.Text.Length;
        }
    }
}
