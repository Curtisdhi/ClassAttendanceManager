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

        public Property(string name, object value, object owner)
            : this(name, value)
        {
            Owner = owner;
        }

        public string Name { get; private set; }
        public object Value { get; set; }
        public object Owner { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
