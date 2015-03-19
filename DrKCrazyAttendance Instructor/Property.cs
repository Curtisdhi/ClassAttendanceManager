using System.ComponentModel;

namespace DrKCrazyAttendance_Instructor
{
    public class Property : INotifyPropertyChanged
    {
        public Property(string name, object value)
        {
            Name = name;
            Value = value;
        }


        public string Name { get; private set; }
        public object Value { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
