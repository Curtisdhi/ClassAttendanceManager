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
        public StudentIDForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string studentId = txtstuID1.Text + txtstuID2.Text + txtstuID3.Text;
            string conStuId = txtConID1.Text + txtConID2.Text + txtConID3.Text;
            if (studentId.Equals(conStuId))
            {
                long id = 0;
                if (long.TryParse(studentId, out id))
                {
                    if (Student.GetStudent(id) == null)
                    {
                        Student student = new Student(id, Environment.UserName);
                        Student.Add(student);
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
            else if (!char.IsDigit(box.Text[length - 1]))
            {
                box.Text = box.Text.Substring(0, length - 1);
                box.CaretIndex = box.Text.Length;
                e.Handled = true;
            }
        }

    }
}
