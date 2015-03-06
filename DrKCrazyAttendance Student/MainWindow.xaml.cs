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
        private Course course;
        private Student student;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Properties
        public Course Course
        {
            set {
                course = value;
                RefreshInfo();
            }
            get {
                return course;
            }
        }

        public Student Student
        {
            set {
                student = value;
                RefreshInfo();
            }
            get {
                return student;  
            }
        }
        #endregion

        private void RefreshInfo()
        {
            if (Course != null) {
                lblCourse.Content = Course.CourseName + " " + Course.Section;
                lblInstructor.Content = Course.Instructor;
            }
            if (Student != null)
            {
                lblStudentId.Content = Student.Id;
                lblUsername.Content = Student.Username;
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (Student != null)
            {
                bool? success = new StudentIDForm(Student).ShowDialog();
                if (success != null && (bool)success)
                {
                    //fetch the new student
                    Student = Student.GetStudent(Student.Username);
                    RefreshInfo();
                    MessageBox.Show("Successfully changed your id");
                }
                else
                {
                    MessageBox.Show("Your id did not change.");
                }
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshInfo();
        }

    }
}
