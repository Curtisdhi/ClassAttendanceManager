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
        /// <summary>
        /// Setting up objects that will be used
        /// </summary>
        private Course course;

        /// <summary>
        /// Holds list of objects that contains a course attendance record for each student in the class
        /// </summary>
        private List<AttendanceViewModel> attendanceVms = new List<AttendanceViewModel>();

        /// <summary>
        /// Consists of the pixel data for a graphics image and its attributes
        /// </summary>
        private Bitmap bitmap;

        /// <summary>
        /// Sets up input from accessed fields 
        /// </summary>        
        public ICommand ToggleAttendanceCommand { get; private set; }

        /// <summary>
        /// Uses a course object provided in parameter to setup Attendance Record
        /// </summary>
        public AttendanceReport(Course course)
        {
            InitializeComponent();
            this.course = course;

            /// <summary>
            /// Uses course Id number to get attendance record of each student
            /// </summary>
            List<Attendance> attendances = Attendance.GetAttendancesByCourseId(course.Id);

            /// <summary>
            /// Keeps count
            /// </summary>
            int i = 0;

            /// <summary>
            /// Creates a Column header with the courses days that the student and instructor meet for class
            /// </summary>
            foreach (DateTime date in course.GetClassMeetings())
            {
                var binding = new Binding(string.Format("AttendsToCourse[{0}]", i));

                /// <summary>
                /// Creates columns for datagrid
                /// </summary>
                CustomBoundColumn col = new CustomBoundColumn()
                {
                    /// <summary>
                    /// Set's up the column headers as dates with 3 letters for month and includes the day in a numeric value
                    /// </summary>
                    Header = date,
                    HeaderStringFormat = "MMM d",
                    TemplateName = "attendanceCheckmark",
                    Binding = binding
                };
                attendanceDataGrid.Columns.Add(col);
                i++;
            }
            /// <summary>
            /// Adds Rows of Student Attendance Record for each student in that class
            /// </summary>
            foreach (List<Attendance> ia in AttendancesSplit(attendances)) {
                if (ia.Count > 0) {
                    Student student = ia[0].Student;
                    AttendanceViewModel avm = new AttendanceViewModel(course, student, ia);
                    attendanceVms.Add(avm);
                    attendanceDataGrid.Items.Add(avm);
                }
            }

        }

        /// <summary>
        /// Grabs instructors name from the current logged in username on the operating system 
        /// </summary>
        /// /// <remarks>
        /// http://stackoverflow.com/questions/14304355/linq-separating-single-list-to-multiple-lists
        /// </remarks>
        /// <param name="items"></param>
        /// <returns> groups </returns>
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

        /// <summary>
        ///  Saves Attendance record as a .csv record to make it easier to keep current format
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
       
        /// <summary>
        /// Prints Attendance Report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            /// <summary>
            /// Performs rendering on print/print preview page for AttendanceReport window
            /// </summary>
            PrintRenderer render = new PrintRenderer(attendanceVms, course);

            /// <summary>
            /// Holds the pixelValues and attributes of the datagrid that contains the attendance report
            /// </summary>
            bitmap = render.GenerateGrid();

            /// <summary>
            /// Pure in-memory stream of data
            /// </summary>
            using (MemoryStream memory = new MemoryStream())
            {
                /// <summary>
                /// Saves bitmap image as a png format
                /// </summary>
                bitmap.Save(memory, ImageFormat.Png);

                /// <summary>
                /// Finds the begining of this image in memory
                /// </summary>
                memory.Seek(0, SeekOrigin.Begin);

                /// <summary>
                /// A specialized BitmapSource that is optimized for loading images
                /// </summary>
                BitmapImage bitmapImage = new BitmapImage();

                /// <summary>
                /// Signals the start of the BitmapImage initialization
                /// </summary>
                bitmapImage.BeginInit();

                /// <summary>
                /// Gets the stream source of the BitmapImage in memory
                /// </summary>
                bitmapImage.StreamSource = memory;

                /// <summary>
                // Specifies how bitmap image should handle memory caching
                /// </summary>
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

                /// <summary>
                // Signals the End of the BitmapImage initialization
                /// </summary>
                bitmapImage.EndInit();
            }

            /// <summary>
            /// Lets users select a printer and choose which sections of the document to print
            /// </summary>
            PrintDialog dialog = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += this.Doc_PrintPage;

            /// <summary>
            /// Sets the default setting to print in landscape to fit whole attendance report width on one page
            /// </summary>
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

        /// <summary>
        /// Datagrid delete button event
        /// Allows instructor to delete attendance report for the selected class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            AttendanceViewModel avm = btn.DataContext as AttendanceViewModel;
            var  result = MessageBox.Show("Are you sure you want to delete this student's attendance record?", "Are you sure?", 
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Attendance.Remove(avm.Attendances);
                /// <summary>
                /// remove this from the datagrid
                /// </summary>
                attendanceVms.Remove(avm);
                attendanceDataGrid.Items.Remove(avm);
            }
        }
 
        /// <summary>
        /// Save this object into a CSV file
        /// </summary>
        /// <param name="stream"></param>
        public void SaveToFile(Stream stream)
        {
            try
            {
                StreamWriter sw = null;
                using (sw = new StreamWriter(stream))
                {
                    StringBuilder sb = new StringBuilder();

                    /// <summary>
                    /// write header info
                    /// </summary>
                    sb.Append("Username,");
                    sb.Append("Id,");
                    foreach (DateTime date in course.GetClassMeetings())
                    {
                        sb.Append(date.ToString("MMM dd"));
                        sb.Append(",");
                    }
                    /// <summary>
                    /// remove the last comma
                    /// </summary>
                    sb.Remove(sb.Length - 1, 1);

                    sw.WriteLine(sb.ToString());

                    foreach (AttendanceViewModel avm in attendanceVms) {
                        /// <summary>
                        /// clear the builder to make sure we don't append to an existing string lol.
                        /// </summary>
                        sb.Clear();
                        sb.Append(avm.Student.Username);
                        sb.Append(",");
                        sb.Append(avm.Student.Id);
                        sb.Append(",");
                        foreach (object a in avm.AttendsToCourse)
                        {
                            Property prop = (Property)a;
                            bool[] attendeds = ((bool[])prop.Value);

                            /// <summary>
                            //if student has attended
                            /// </summary>
                            if (attendeds[0])
                            {
                                /// <summary>
                                /// If tardy put T otherwise X
                                /// </summary>
                                sb.Append(attendeds[1] ? "X" : "T");
                                
                            }
                            /// <summary>
                            /// append the trailing comma
                            /// </summary>
                            sb.Append(",");
                   
                        }
                        /// <summary>
                        /// remove the last comma
                        /// </summary>
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
