using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Playground
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
      
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            double[] TIs = { 1, 2, 3, 4, 5, 6 };
            ObservableCollection<TI> itemsource = new ObservableCollection<TI>();
            for (int i = 0; i < 6; i++)
            {
                itemsource.Add(new TI
                {
                    ID = i + 1,
                    Value = (float)TIs[i],
                    sign = "*"
                });
            }
            dgTI.DataContext = itemsource;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double[] TIs = { 1, 2, 3, 4, 5, 6 };
            ObservableCollection<TI> itemsource = new ObservableCollection<TI>();
            for (int i = 0; i < 6; i++)
            {
                itemsource.Add(new TI
                {
                    ID = i + 1,
                    Value = (float)TIs[i],
                    sign = "*"
                });
            }
            dgTI.ItemsSource = itemsource;
        }
    }


    public class TI
    {
        public int ID;
        public float Value;
        public string sign;
    }
}
