using DrKCrazyAttendance;
using DrKCrazyAttendance_Instructor.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrKCrazyAttendance_Instructor
{
    public class PrintRenderer
    {
        const int PAGE_WIDTH = 1050;
        const int PAGE_HEIGHT = 1000;
        const int MIN_COL_WIDTH = 24;
        const int SPACING = 2;
        
        private Course course;
        private List<AttendanceViewModel> avms;

        private Font font;
        private Brush brush;
        private Pen pen;

        private float[] rowHeights;
        private float[] colWidths;

        public PrintRenderer(List<AttendanceViewModel> avms, Course course)
        {
            this.avms = avms;
            this.course = course;
            this.font = new Font(FontFamily.GenericSansSerif, 9);
            this.brush = Brushes.Black;
            this.pen = Pens.Black;

            colWidths = new float[2 + course.GetClassMeetings().Length];
            rowHeights = new float[1 + avms.Count];
        }

        public Bitmap GenerateGrid()
        {
            Bitmap bm = new Bitmap(PAGE_WIDTH, PAGE_HEIGHT);
            using (Graphics g = Graphics.FromImage(bm))
            {
                g.Clear(Color.White);
                GenerateHeaders(g);
                GenerateRows(g);
                GenerateLines(g);
            }
            return bm;
        }

        public void GenerateHeaders(Graphics g)
        {
            string usernameHeader = "Username";
            string stuidHeader = "Student Id";
            SizeF userSF = g.MeasureString(usernameHeader, font);
            SizeF stuidSF = g.MeasureString(stuidHeader, font);

            rowHeights[0] = userSF.Height * 2;

            //draw header text and col lines
            g.DrawString(usernameHeader, font, brush, 0, rowHeights[0]/2);
            colWidths[0] = userSF.Width;

            g.DrawString(stuidHeader, font, brush, colWidths[0] + SPACING, rowHeights[0] / 2);
            colWidths[1] = colWidths[0] + SPACING + 4 + stuidSF.Width;

            //draw class meetings headers
            DateTime[] meetings = course.GetClassMeetings();
            for (int i = 0, c = 1; i < meetings.Length; i++, c++)
            {
                g.DrawString(meetings[i].ToString("MMM"), font, brush, colWidths[c] + SPACING, 0);
                g.DrawString(meetings[i].ToString("dd"), font, brush, colWidths[c] + SPACING, rowHeights[0] / 2);
                colWidths[c+1] = colWidths[c] + MIN_COL_WIDTH + SPACING;
            }

        }

        public void GenerateRows(Graphics g)
        {
            float height = g.MeasureString("height", font).Height;

            for (int i = 0, r = 1; i < avms.Count; i++, r++)
            {
                rowHeights[r] = rowHeights[r-1] + height;

                g.DrawString(avms[i].Student.Username, font, brush, 0, rowHeights[r]-height);
                g.DrawString(avms[i].Student.Id.ToString(), font, brush, colWidths[0], rowHeights[r]-height);

                int c = 1;
                foreach (Property prop in avms[i].AttendsToCourse)
                {
                    bool[] attendance = (bool[])prop.Value;
                    if (attendance[0]) {
                        string mark = attendance[1] ? "T" : "X";
                        g.DrawString(mark, font, brush, colWidths[c] + SPACING, rowHeights[r]-height);
                    }
                    c++;
                }
                   
            }
        }

        public void GenerateLines(Graphics g)
        {
            int height = (int)rowHeights[rowHeights.Length-1];

            for (int i = 0; i < rowHeights.Length; i++)
            {
                g.DrawLine(pen, 0, rowHeights[i], PAGE_WIDTH, rowHeights[i]);
            }

            g.DrawLine(pen, 0, 0, 0, height);

            for (int i = 0; i < colWidths.Length; i++)
            {
                g.DrawLine(pen, colWidths[i], 0, colWidths[i], height);
            }
        }

    }
}
