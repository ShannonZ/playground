using SoftFluent.Windows;
using System;
using System.ComponentModel;

namespace Playground
{
    public class Model: AutoObject,INotifyPropertyChanged
    {

        [Category("Cumstumized Editor")]
        [DisplayName("using SF AutoObject")]
        [PropertyGridOptions(SortOrder = 2, EditorDataTemplateResourceKey = "MyEditor")]
        public double SF
        {
            get { return GetProperty<double>(); }
            set {
                SetProperty(value);
            }
        }

       public Model()
        {
            SF = 12.345;
        }
    }

    public class Model1 : AutoObject, INotifyPropertyChanged
    {

        [Category("Default Editor")]
        [DisplayName("using SF AutoObject")]
        [PropertyGridOptions(SortOrder = 2)]
        public double SF
        {
            get { return GetProperty<double>(); }
            set
            {
                SetProperty(value);
            }
        }

        public Model1()
        {
            SF = 12.345;
        }
    }
}
