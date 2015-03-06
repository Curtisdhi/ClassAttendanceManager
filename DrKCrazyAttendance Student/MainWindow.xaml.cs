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
        Student student = null;
        Course course = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(Course course, Student student)
        {
            InitializeComponent();
            this.course = course;
            this.student = student;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (student != null)
            {
                bool? success = new StudentIDForm(student).ShowDialog();
                if (success != null && (bool)success)
                {
                    MessageBox.Show("Successfully changed your id");
                }
            }
            if (student == student)
            {
                MessageBox.Show("User ID matched the previously entered ID no edits will take place ", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (course != null && student != null)
            {
                lblCourse.Content = course.CourseName + " " + course.Section;
                lblInstructor.Content = course.Instructor;
                lblStudentId.Content = student.Id;
                lblUsername.Content = student.Username;
            }
        }

    }
}
