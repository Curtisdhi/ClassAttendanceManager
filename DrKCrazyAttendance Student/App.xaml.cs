using DrKCrazyAttendance;
using DrKCrazyAttendance.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DrKCrazyAttendance_Student
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Settings.Default.SqlDatabase = "capstone_2";
            Settings.Default.SqlUsername = "capstone";
            Settings.Default.SqlServerAddr = "www.projectgxp.com";
            Settings.Default.SqlPassword = "SYM4GMmlzHmpoGenV4yb";
            Settings.Default.Classroom = "C2427";
            string userName = Environment.UserName;

            DateTime now = DateTime.Now;
            Course course = Course.GetCoursesByTime(Settings.Default.Classroom, now);

            Student student = Student.GetStudent(userName);
            if (student == null)
            {
                //ask for student id
                bool? success = new StudentIDForm(userName).ShowDialog();
                if (success != null && (bool)success)
                {
                    //refetch student
                    student = Student.GetStudent(userName);
                    if (student == null){
                        MessageBox.Show("Sorry, some error has occurered, and you will not be counted as attended. Please try agian.");
                    }
                }
            }

            if (student != null && RegisterAttendance(course, student, now))
            {
                MainWindow = new MainWindow(course, student);
                MainWindow.Show();
            }
            else
            {
                Shutdown();
            }

        }

        private bool RegisterAttendance(Course course, Student student, DateTime now)
        {
            bool success = false;
            if (course == null)
            {
                MessageBox.Show("No course is available.");
            }
            else
            {
                //register attendance
                Attendance attendance = new Attendance(course, student, "127.0.0.1", now, false);
                success = !Attendance.HasAttended(attendance);
                if (success)
                {
                    Attendance.Add(attendance);
                }
                else
                {
                    MessageBox.Show("You have already been counted for today.");
                }

            }

            return success;
        } 

    }
}
