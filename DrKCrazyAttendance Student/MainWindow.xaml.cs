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

namespace DrKCrazyAttendance_Student
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Settings.Default.SqlDatabase = "capstone_2";
            Settings.Default.SqlUsername = "capstone";
            Settings.Default.SqlServerAddr = "www.projectgxp.com";
            Settings.Default.SqlPassword = "SYM4GMmlzHmpoGenV4yb";
            string userName = Environment.UserName;
            Student student = Student.GetStudent("Dylan");
            if (student == null)
            {
                //ask for student id
                new StudentIDForm().Show();
            }
            else 
            {
                Course course = Course.GetCoursesByTime("C2427", DateTime.Now.TimeOfDay);
                if (course == null)
                {
                    MessageBox.Show("No course is available.");
                }
                else
                {
                    //register attendance
                    Attendance attendance = new Attendance(course, student, "127.0.0.1", DateTime.Now, false);
                    Attendance.Add(attendance);
                }
            }
        }
    }
}
