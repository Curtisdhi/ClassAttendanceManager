using DrKCrazyAttendance;
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
using System.IO;


namespace DrKCrazyAttendance_Student
{
    /// <summary>
    /// Interaction logic for StudentIDForm.xaml
    /// </summary>
    public partial class StudentIDForm : Window
    {
        const int STUDENT_ID_MAX_LENGTH = 9;
        Student student = null;
        string username;
        private Student_ID_Help studentHelpForm;

        public StudentIDForm()
        {
            InitializeComponent();
        }

        public StudentIDForm(string username)
        {
            InitializeComponent();
            this.username = username;
        }

        public StudentIDForm(Student student)
        {
            InitializeComponent();
            this.student = student;
            this.username = student.Username;
            if (student != null)
            {
                string id = student.Id.ToString();

                //ignore the first part as we have it preset
                //txtstuID1.Text = id.Substring(0, 3);
                txtstuID2.Text = id.Substring(3, 3);
                txtstuID3.Text = id.Substring(6, 3);
            }
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string studentId = txtstuID1.Text + txtstuID2.Text + txtstuID3.Text;
            string conStuId = txtConID1.Text + txtConID2.Text + txtConID3.Text;

            //id must be 9 characters long
            if (studentId.Length == STUDENT_ID_MAX_LENGTH)
            {
                if (studentId.Equals(conStuId))
                {
                    long id = 0;
                    if (long.TryParse(studentId, out id))
                    {
                        if (Student.GetStudent(id) == null)
                        {
                            Student newStudent = new Student(id, username);
                            if (student != null)
                            {
                                Student.Update(student, newStudent);
                                student = newStudent;
                            }
                            else
                            {
                                student = newStudent;
                                Student.Add(student);
                            }
                            DialogResult = true;

                        }
                        else
                        {
                            MessageBox.Show("Student id already exists. Please ask you Instructor if this is an issue.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Student id must be a number.");
                    }
                }
                else
                {
                    MessageBox.Show("Student id must match the confirm Id.");
                }
            }
            else
            {
                MessageBox.Show("Student id must contain " + STUDENT_ID_MAX_LENGTH + " digits.");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtConID2.Clear();
            txtConID3.Clear();
            txtstuID2.Clear();
            txtstuID3.Clear();
        }

        private void txtId_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = (TextBox)sender;
            int length = box.Text.Length;
            //only allow 3 characters per box
            if (length > 3)
            {
                box.Text = box.Text.Substring(0, length - 1);
                box.CaretIndex = box.Text.Length;
                e.Handled = true;
            }
            //prevent the student from entering non-digits
            else if (length > 0 && !char.IsDigit(box.Text[length - 1]))
            {
                box.Text = box.Text.Substring(0, length - 1);
                box.CaretIndex = box.Text.Length;
                e.Handled = true;
            }
        }

        private void menuHelp_Click(object sender, RoutedEventArgs e)
        {
            
            if (studentHelpForm != null)
            {
                //window is already open, let's just refocus it
                if (studentHelpForm.IsLoaded)
                {
                    studentHelpForm.Focus();
                    studentHelpForm.WindowState = WindowState.Normal;
                }
                //window has been closed so let's open a new one
                else
                {
                    studentHelpForm = new Student_ID_Help();
                    studentHelpForm.Show();
                }
            }
            //window has never been open
            else {
                studentHelpForm = new Student_ID_Help();
                studentHelpForm.Show();
            }
            
        }

    
    }
}
