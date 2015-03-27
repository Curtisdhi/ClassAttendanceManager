using DrKCrazyAttendance;
using DrKCrazyAttendance_Instructor.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DrKCrazyAttendance_Instructor
{
    /// <summary>
    /// Interaction logic for AttendanceReport.xaml
    /// </summary>
    public partial class AttendanceReport : Window
    {
        private Course course;
        private List<AttendanceViewModel> attendanceVms = new List<AttendanceViewModel>();

        private Bitmap bitmap;

        public ICommand ToggleAttendanceCommand { get; private set; }

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
                    HeaderStringFormat = "MMM d",
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

        }

        //http://stackoverflow.com/questions/14304355/linq-separating-single-list-to-multiple-lists
        private List<List<Attendance>> AttendancesSplit(IEnumerable<Attendance> items)
        {
            //group by student id and bind attendances into a group
            //so we can build the attendance table
            //as each attendance row contains a different date.
            var groups = items
                        .GroupBy(i => i.Student.Id)
                        .Select(g => g.ToList())
                        .ToList();

            return groups;
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
            PrintRenderer render = new PrintRenderer(attendanceVms, course);

            bitmap = render.GenerateGrid();
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Seek(0, SeekOrigin.Begin);
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
            }

            PrintDialog dialog = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += this.Doc_PrintPage;

            doc.DefaultPageSettings.Landscape = true;

            bool? result = dialog.ShowDialog();
            if (result.HasValue == result == true)
            {
                doc.PrinterSettings.PrinterName = dialog.PrintQueue.FullName;
                doc.Print();
            }
        }

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
            bitmap.Dispose();
            bitmap = null;
        }

        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //datagrid delete button event
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            AttendanceViewModel avm = btn.DataContext as AttendanceViewModel;
            var  result = MessageBox.Show("Are you sure you want to delete this student's attendance record?", "Are you sure?", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Attendance.Remove(avm.Attendances);
                //remove this from the datagrid
                attendanceVms.Remove(avm);
                attendanceDataGrid.Items.Remove(avm);
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
                            bool[] attendeds = ((bool[])prop.Value);

                            //if student has attended
                            if (attendeds[0])
                            {
                                //If tardy put T otherwise X
                                sb.Append(attendeds[1] ? "X" : "T");
                                
                            }
                            //append the trailing comma
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

        private void attendanceDataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;
        }

    }
}
