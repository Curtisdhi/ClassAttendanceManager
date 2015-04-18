using DrKCrazyAttendance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrKCrazyAttendance_Instructor.ViewModels
{
    public class CourseViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        private Course course;
        private Dictionary<string, bool> propertiesValid = new Dictionary<string, bool>();
        private Dictionary<string, bool> eventsInAction = new Dictionary<string, bool>();

        public CourseViewModel()
        {
            this.Course = new Course();
        }

        public CourseViewModel(Course course)
        {
            this.Course = course;
        }

        #region Properties
        public Course Course
        {
            get { return course; }
            private set
            {
                course = value;
                //clone the course so we have the original values to
                //revert back to in the even the user doesn't save.
                //Note in the event something bad happening, this won't be
                //explictly persisted to the DB until the user "saves"
                OriginalCourse = new Course(value);
            }
        }

        public Course OriginalCourse
        {
            get;
            private set;
        }

        public bool IsValid
        {
            get
            {
                //if if this doesn't contain true, the object isn't valid
                return !propertiesValid.ContainsValue(false);
            }
        }


        public long Id
        {
            get { return Course.Id; }
        }

        public List<DayOfWeek> Days
        {
            get { return Course.Days; }
            set { Course.Days = value; }
        }

        public string Classroom
        {
            get { return course.Classroom; }
            set { Course.Classroom = value; }
        }

        public string CourseName
        {
            get { return Course.CourseName; }
            set { Course.CourseName = value; }
        }

        public string Instructor
        {
            get { return Course.Instructor; }
            set { Course.Instructor = value; }
        }

        public string Section
        {
            get { return Course.Section; }
            set { Course.Section = value; }
        }

        public DateTime StartDate
        {
            get { return Course.StartDate; }
            set { Course.StartDate = value; }
        }

        public DateTime EndDate
        {
            get { return Course.EndDate; }
            set { Course.EndDate = value; }
        }

        public DateTime StartTime
        {
            get { return Course.StartTime; }
            set { Course.StartTime = value; }
        }

        public DateTime EndTime
        {
            get { return Course.EndTime; }
            set { Course.EndTime = value; }
        }

        public int GracePeriodMinutes
        {
            get { return GracePeriod.Minutes; }
            set {
                GracePeriod = TimeSpan.FromMinutes(value);
            }
        }

        public TimeSpan GracePeriod
        {
            get { return Course.GracePeriod; }
            set { Course.GracePeriod = value; }
        }

        public bool LogTardy
        {
            get { return Course.LogTardy; }
            set { Course.LogTardy = value; }
        }
        #endregion

        #region IDataError
        string IDataErrorInfo.Error
        {
            get { throw new NotImplementedException(); }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                TimeSpan timeValidation = new TimeSpan(0, 30, 0);
                string result = "";
                switch (propertyName)
                {
                    case "Classroom":
                        if (Classroom.Length != 5)
                        {
                            result = "Requires 5 characters.";
                        }
                        break;
                    case "CourseName":
                        if (CourseName.Length != 8)
                        {
                            result = "Requires 8 characters.";
                        }
                        break;
                    case "Section":
                        if (Section.Length != 3)
                        {
                            result = "Requires 3 characters.";
                        }
                        break;
                    case "StartDate":
                        if (StartDate == DateTime.MinValue)
                        {
                            result = "Required";
                        }
                        else if (StartDate > EndDate)
                        {
                            result = "Start date can't be after end date.";
                        }
                        RaisePropertyChanged("EndDate");
                        eventsInAction[propertyName] = false;
                        break;
                    case "EndDate":
                        if (EndDate == DateTime.MinValue)
                        {
                            result = "Required";
                        }
                        else if (StartDate > EndDate)
                        {
                            result = "\t";//"End date can't be before start date.";
                        }
                        //validate start date
                        RaisePropertyChanged("StartDate");
                        eventsInAction[propertyName] = false;
                        break;
                    case "StartTime":
                        if (StartTime.TimeOfDay > EndTime.TimeOfDay)
                        {
                            result = "Start time can not be after end time.";
                        }
                        else if ((EndTime.TimeOfDay - StartTime.TimeOfDay) < timeValidation)
                        {
                            result = "Class must be atleast 30 minutes long.";
                        }

                        RaisePropertyChanged("EndTime");
                        eventsInAction[propertyName] = false;

                        break;
                    case "EndTime":
                        if (StartTime.TimeOfDay > EndTime.TimeOfDay)
                        {
                            result = "\t";//"End time can't come before start time.";
                        }
                        else if ((EndTime.TimeOfDay - StartTime.TimeOfDay) < timeValidation)
                        {
                            result = "\t";//"Class must be atleast 30 minutes long.";
                        }
                        //validate start time
                        RaisePropertyChanged("StartTime");
                        eventsInAction[propertyName] = false;
                        break;
                    case "GracePeriodMinutes":
                        if (LogTardy && GracePeriodMinutes == 0)
                        {
                            result = "Required";
                        }
                        break;
                    case "LogTardy":
                        //validate grace period
                        RaisePropertyChanged("GracePeriod");
                        break;
                }

                propertiesValid[propertyName] = string.IsNullOrEmpty(result);

                return result;
            }
        }
        #endregion

        #region INotifyPropertyChanged Members

        private void RaisePropertyChanged(string propName)
        {
            if (propertyChangedDelegate != null)
            {
                if (!eventsInAction.ContainsKey(propName) || !eventsInAction[propName])
                {
                    eventsInAction[propName] = true;
                    propertyChangedDelegate(this, new PropertyChangedEventArgs(propName));
                }
                else
                {
                    eventsInAction[propName] = false;
                }
            }
        }

        private PropertyChangedEventHandler propertyChangedDelegate;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                propertyChangedDelegate = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedDelegate, value);
            }
            remove
            {
                propertyChangedDelegate = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedDelegate, value);
            }
        }

        #endregion

    }
}
