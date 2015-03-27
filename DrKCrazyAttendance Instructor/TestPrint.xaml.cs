using DrKCrazyAttendance;
using DrKCrazyAttendance_Instructor.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
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

namespace DrKCrazyAttendance_Instructor
{
    /// <summary>
    /// Interaction logic for TestPrint.xaml
    /// </summary>
    public partial class TestPrint : Window
    {
        private Course course;
        private List<AttendanceViewModel> attendanceVms = new List<AttendanceViewModel>();
        private Bitmap bitmap;

        public TestPrint()
        {
            InitializeComponent();

           
        }

        private List<List<Attendance>> AttendancesSplit(IEnumerable<Attendance> items)
        {
            //group by student id and bind attendances into a group
            //so we can build the attendance table
            //as each attendance row contains a different date.
            var groups = items
                        .GroupBy(i => i.Student.Id)
                        .Select(g => g.ToList()).ToList();
            return groups;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            course = Course.GetCoursesByClassroom("C2427")[0];
            List<Attendance> attendances = Attendance.GetAttendancesByCourseId(course.Id);

            foreach (List<Attendance> ia in AttendancesSplit(attendances))
            {
                if (ia.Count > 0)
                {
                    Student student = ia[0].Student;
                    AttendanceViewModel avm = new AttendanceViewModel(course, student, ia);
                    attendanceVms.Add(avm);
                }
            }

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
                image.Source = bitmapImage;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += this.Doc_PrintPage;

            doc.DefaultPageSettings.Landscape = true;
            
            bool? result = dialog.ShowDialog();
            if (result.HasValue == result == true)
            {
                dialog.PrintQueue = dialog.PrintQueue;
                doc.Print();
            }
           
        }

        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }

    }
}
