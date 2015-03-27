using System.ComponentModel;

namespace DrKCrazyAttendance_Instructor
{
    public class Property : INotifyPropertyChanged
    {
        private object value;

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
        public object Value { 
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }
        public object Owner { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }

        }
    }
}
