using DrKCrazyAttendance;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using System.Windows.Threading;

namespace DrKCrazyAttendance_Instructor
{
    /// <summary>
    /// Interaction logic for AttendanceReport.xaml
    /// </summary>
    public partial class AttendanceReport : Window
    {
        private Course course;
        private List<AttendanceViewModel> attendanceVms = new List<AttendanceViewModel>();

        public AttendanceReport(Course course)
        {
            InitializeComponent();
            this.course = course;

            List<Attendance> attendances = Attendance.GetAttendancesByCourseId(course.Id);

            int i = 0;
            foreach (DateTime date in course.GetClassMeetings())
            {
                var binding = new Binding(string.Format("AttendsToCourse[{0}]", i));
                CustomBoundColumn col = new CustomBoundColumn()
                {
                    Header = date,
                    HeaderStringFormat = "M/ d",
                    TemplateName = "attendanceCheckmark",
                    Binding = binding
                };
                attendanceDataGrid.Columns.Add(col);
                i++;
            }

            foreach (List<Attendance> ia in AttendancesSplit(attendances)) {
                if (ia.Count > 0) {
                    Student student = ia[0].Student;
                    AttendanceViewModel avm = new AttendanceViewModel(course, student, ia);
                    attendanceVms.Add(avm);
                    attendanceDataGrid.Items.Add(avm);
                }
            }

            PrintRenderer render = new PrintRenderer(attendanceVms);

            Bitmap bitmap = render.GenerateGrid();
            MemoryStream ms;
            using (ms = new MemoryStream()) {
                bitmap.Save(ms, ImageFormat.Bmp);
                ms.Position = 0;
                BitmapImage bmpi = new BitmapImage(); 
                bmpi.BeginInit();
                bmpi.StreamSource = ms;
                bmpi.EndInit();
                testImage.Source = bmpi;
            }

        }

        //http://stackoverflow.com/questions/14304355/linq-separating-single-list-to-multiple-lists
        private List<List<Attendance>> AttendancesSplit(IEnumerable<Attendance> items)
        {
            //group by student id and bind attendances into a group
            //so we can build the attendance table
            //as each attendance row contains a different date.
            var groups = items
                        .GroupBy(i => i.Student.Id)
                        .Select(g => g.ToList()).ToList();
            return groups;
            /*while (groups.Count > 0)
            {
                yield return groups.Select(g =>
                { var i = g[0]; g.RemoveAt(g.Count - 1); return i; });
                groups.RemoveAll(g => g.Count == 0);
            }*/
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.AddExtension = true;
            fileDialog.DefaultExt = "csv";
            fileDialog.FileName = string.Format("{0}_{1}_{2}",course.CourseName, course.Section, DateTime.Now.ToString("MMM_dd_yyyy"));
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "csv files (*.csv)|*.csv";
            bool? result = fileDialog.ShowDialog();
            if (result.HasValue && (bool)result)
            {
                SaveToFile(fileDialog.OpenFile());
            }

        }
       
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            StackPanel page = new StackPanel();
            PrintDialog dialog = new PrintDialog();

bool? result = dialog.ShowDialog();
if (result.HasValue && result.Value)
{
    System.Windows.Size pageSize = new System.Windows.Size { Height = dialog.PrintableAreaHeight, Width = dialog.PrintableAreaWidth };
    page.Measure(pageSize);
    page.UpdateLayout();
    dialog.PrintVisual(this.attendanceDataGrid, "");
}
            
        }

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //datagrid delete button event
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
            
            //Attendance obj = ((FrameworkElement)sender).DataContext as Attendance;
             {
            var  result = MessageBox.Show("Are you sure you want to delete this student?", "Validation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if ( result == MessageBoxResult.Yes && attendanceDataGrid.SelectedItem != null)
        {
                attendanceDataGrid.Items.Remove(attendanceDataGrid.SelectedItem);
            }
        }

        //Save this object into a CSV file
        public void SaveToFile(Stream stream)
        {
            try
            {
                StreamWriter sw = null;
                using (sw = new StreamWriter(stream))
                {
                    StringBuilder sb = new StringBuilder();
                    //write header info
                    sb.Append("Username,");
                    sb.Append("Id,");
                    foreach (DateTime date in course.GetClassMeetings())
                    {
                        sb.Append(date.ToString("MMM dd"));
                        sb.Append(",");
                    }
                    //remove the last comma
                    sb.Remove(sb.Length - 1, 1);

                    sw.WriteLine(sb.ToString());

                    foreach (AttendanceViewModel avm in attendanceVms) {
                        //clear the builder to make sure we don't append to an existing string lol.
                        sb.Clear();
                        sb.Append(avm.Student.Username);
                        sb.Append(",");
                        sb.Append(avm.Student.Id);
                        sb.Append(",");
                        foreach (object a in avm.AttendsToCourse)
                        {
                            Property prop = (Property)a;
                            bool b = Convert.ToBoolean(prop.Value);
                            Console.Write(a.GetType());
                       
                            //place an X where the student attended on that date
                            sb.Append(b ? "X" : "");
                            sb.Append(",");
                        }
                        //remove the last comma
                        sb.Remove(sb.Length - 1, 1);
                        sw.WriteLine(sb.ToString());
                    }



                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

    }
}
