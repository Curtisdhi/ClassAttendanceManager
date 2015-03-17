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
        private About about;
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

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        public void HideWin()
        {
            Hide();
            Left = System.Windows.SystemParameters.PrimaryScreenWidth;
            Top = System.Windows.SystemParameters.PrimaryScreenHeight;
        }
        public void ShowWin()
        {
            Show();
            CenterWindowOnScreen();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if (Student != null)
            {
                long id = Student.Id;
                bool? success = new StudentIDForm(Student).ShowDialog();
                if (success != null && (bool)success)
                {
                    //fetch the new student
                    Student = Student.GetStudent(Student.Username);
                    RefreshInfo();
                    
                    if (Student.Id == id) {
                        MessageBox.Show("User ID matched the previously entered ID no edits will take place ", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                    MessageBox.Show("Successfully changed your id");
                }
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

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void mnuAbout_Click(object sender, RoutedEventArgs e)
        {
            if (about != null)
            {
                if (about.IsLoaded)
                {
                    about.Focus();
                    about.WindowState = WindowState.Normal;
                }
            }
            else
            {
                about = new About();
                about.Show();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (about != null && about.IsLoaded)
            {
                about.Close();
            }
        }

    }
}
