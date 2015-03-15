using DrKCrazyAttendance;
using DrKCrazyAttendance.Properties;
using MySql.Data.MySqlClient;
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
            MainWindow = new MainWindow();
            MainWindow.Visibility = Visibility.Hidden;

            /*Settings.Default.SqlDatabase = "capstone_2";
            Settings.Default.SqlUsername = "capstone";
            Settings.Default.SqlServerAddr = "www.projectgxp.com";
            Settings.Default.SqlPassword = "SYM4GMmlzHmpoGenV4yb";
            Settings.Default.Classroom = "C2427";*/
            string userName = Environment.UserName;

            DateTime now = DateTime.Now;
            Course course = null;
            Student student = null;

            try
            {
                //may throw a sql exception
                course = Course.GetCoursesByTime(Settings.Default.Classroom, now);

                //may throw a sql exception
                student = Student.GetStudent(userName);

                if (student == null)
                {
                    //ask for student id
                    StudentIDForm stuIdForm = new StudentIDForm(userName);
                    bool? success = stuIdForm.ShowDialog();

                    if (success != null && (bool)success)
                    {

                        student = Student.GetStudent(userName);
                        if (student == null)
                        {
                            MessageBox.Show("Sorry, some error has occurered, and you will not be counted as attended. Please try agian.",
                                "Internal error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Server Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (course == null)
            {
                MessageBox.Show("No course is not found.", "Course Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
           
            if (student != null && course != null && RegisterAttendance(course, student, now))
            {
                ((MainWindow)MainWindow).Course = course;
                ((MainWindow)MainWindow).Student = student;
                MainWindow.Visibility = Visibility.Visible;
                MainWindow.Show();                    
            }
            else
            {
                //Shutdown();
                MainWindow.Close();
            }
       }

        private bool RegisterAttendance(Course course, Student student, DateTime now)
        {
            bool success = false;
            //register attendance
            Attendance attendance = new Attendance(course, student, "127.0.0.1", now, course.IsTardy(now));
            success = !Attendance.HasAttended(attendance);
            if (success)
            {
                Attendance.Add(attendance);
            }
            else
            {
                MessageBox.Show("You have already been counted for today.");
            }

            return success;
        } 

    }
}
