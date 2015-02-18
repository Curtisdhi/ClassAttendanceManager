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
            if (student != null)
            {
                string id = student.Id.ToString();
                string section1 = id.Substring(0,3);
                string section2 = id.Substring(3,6);
                string section3 = id.Substring(6,9);

                txtstuID1.Text = section1;
                txtstuID2.Text = section2;
                txtstuID3.Text = section3;

                txtConID1.Text = section1;
                txtConID2.Text = section2;
                txtConID3.Text = section3;
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
                            Close();
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
                MessageBox.Show("Student id must contain "+ STUDENT_ID_MAX_LENGTH +" digits.");
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

    }
}
