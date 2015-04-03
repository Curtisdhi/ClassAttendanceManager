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
            //Stop the program if classroom isn't defined
            if (string.IsNullOrWhiteSpace(Settings.Default.Classroom))
            {
                MessageBox.Show("Classroom not found. Please check the configuration files.",
                               "404 error", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown(0);
                return;
            }

            MainWindow = new MainWindow();
            ((MainWindow)MainWindow).HideWin();

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

            if (student != null && RegisterAttendance(course, student, now))
            {
                ((MainWindow)MainWindow).Course = course;
                ((MainWindow)MainWindow).Student = student;
                ((MainWindow)MainWindow).ShowWin();                    
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
            if (course == null)
            {
                MessageBox.Show("Course not found.", "404 Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                //register attendance
                Attendance attendance = new Attendance(course, student, "127.0.0.1", now, course.IsTardy(now));
                success = !Attendance.HasAttended(attendance);
                if (success)
                {
                    Attendance.Add(attendance);
                }
                else
                {
                    MessageBox.Show("You have already been counted for today.", "Info",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            return success;
        } 

    }
}
