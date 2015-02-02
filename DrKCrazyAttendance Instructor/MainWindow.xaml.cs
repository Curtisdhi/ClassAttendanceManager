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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DrKCrazyAttendance_Instructor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsForm settingsForm;
        private CourseEditor editor;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        #region Properties
        //For use as a singleton
        public static MainWindow Instance
        {
            get;
            private set;
        }

        #endregion

        public void LoadCourses()
        {
            lstCourses.Items.Clear();
            List<Course> courses = Course.GetCoursesByInstructor(Settings.Default.Instructor);
            foreach (Course c in courses)
            {
                lstCourses.Items.Add(c);
            }
        }

        private void menuSettings_Click(object sender, RoutedEventArgs e)
        {
            settingsForm = new SettingsForm();
            settingsForm.Show();
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstCourses.SelectedItem != null)
            {
                Course course = (Course)lstCourses.SelectedItem;
                Course.Remove(course);
                lstCourses.Items.Remove(course);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            editor = new CourseEditor();
            editor.Show();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lstCourses.SelectedItem != null)
            {
                editor = new CourseEditor((Course)lstCourses.SelectedItem);
                editor.Show();
            }
        }

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCourses();
            Attendance.GetAttendancesByInstructor(Settings.Default.Instructor);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (settingsForm != null && settingsForm.IsVisible)
            {
                var confirmResult = MessageBox.Show("Are you sure to close the program without saving the settings?",
                                        "Confirm", MessageBoxButton.YesNo);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    settingsForm.Close();
                }
                else
                {
                    //cancel if the user doesn't confirm
                    e.Cancel = true;
                }
            }
        }
    }
}
