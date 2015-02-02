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

        private void menuSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsForm settings = new SettingsForm();
            settings.Show();
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
            CourseEditor editor = new CourseEditor();
            editor.Show();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lstCourses.SelectedItem != null)
            {
                CourseEditor editor = new CourseEditor((Course)lstCourses.SelectedItem);
                editor.Show();
            }
        }

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Course> courses = Course.GetCoursesByInstructor(Settings.Default.Instructor);
            foreach (Course c in courses)
            {
                lstCourses.Items.Add(c);
            }
            Attendance.GetAttendancesByInstructor(Settings.Default.Instructor);
        }
    }
}
