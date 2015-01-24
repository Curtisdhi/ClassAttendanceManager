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

namespace DrKCrazyAttendance_Instructor
{
    /// <summary>
    /// Interaction logic for Course.xaml
    /// </summary>
    public partial class CourseEditor : Window
    {
        public CourseEditor()
        {
            InitializeComponent();
        }


        private void resetForm()
        {
            chkEnableTardy.IsChecked = false;
            chkMonday.IsChecked = false;
            chkTuesday.IsChecked = false;
            chkWednesday.IsChecked = false;
            chkThursday.IsChecked = false;
            chkFriday.IsChecked = false;
            chkSaturday.IsChecked = false;
            semesterChoice.SelectedIndex = 0;
            txtCourse.Text = "";
            txtSection.Text = "";
            startDateTimePicker.Value = null;
            endDateTimePicker.Value = null;
        }

        private void btnResetCourse_Click(object sender, RoutedEventArgs e)
        {
            resetForm();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
