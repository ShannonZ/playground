using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows;

namespace ListViewStyling
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ScanBase> PrescanCollection { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            // PreScan
            PrescanCollection = new ObservableCollection<ScanBase>();
            PrescanCollection.Add(new ScanBase());
            PrescanCollection.Add(new ScanBase());

            DataContext = this;
        }
    }

    public class ScanBase: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public string ScanName { get; set; }
        public bool IsEditable { get; set; }
        public bool DataArchived { get; set; }
        public TimeSpan TimeCount { get; set; }

    }
}
