using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plainDataValidation
{
    class ViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private int x;
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                NotifyPropertyChanged("X");
                NotifyPropertyChanged("Y");
            }
        }

        private int y;
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                NotifyPropertyChanged("Y");
                NotifyPropertyChanged("X");
            }
        }
        public string Error { get; private set; }
        public string this[string property]
        {
            get
            {
                switch (property)
                {
                    case "X":
                        if (X < 0)
                        {
                            return "X > 0";
                        }
                        if (X + Y > 10)
                        {
                            return "X + Y must <= 10";
                        }
                        break;
                    case "Y":
                        if (Y < 0)
                        {
                            return "X > 0";
                        }
                        if (X + Y > 10)
                        {
                            return "X + Y must <= 10";
                        }
                        break;
                }
                return null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
