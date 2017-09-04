using SoftFluent.Windows;
using System;
using System.ComponentModel;

namespace Playground
{
    public class Model: AutoObject,INotifyPropertyChanged
    {

        [Category("Cumstumized Editor(NumericUpDown)")]
        [DisplayName("using SF AutoObject")]
        [PropertyGridOptions(SortOrder = 2, EditorDataTemplateResourceKey = "NUDEditor")]
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
    public class Model2 : AutoObject, INotifyPropertyChanged
    {

        [Category("Cumstumized Editor(TextBox)")]
        [DisplayName("using SF AutoObject")]
        [PropertyGridOptions(SortOrder = 2, EditorDataTemplateResourceKey = "TXTEditor")]
        public double SF
        {
            get { return GetProperty<double>(); }
            set
            {
                SetProperty(value);
            }
        }

        public Model2()
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
