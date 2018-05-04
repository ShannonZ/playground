using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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

namespace Legend
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Series = new SeriesCollection
            {
                new StackedAreaSeries
                {
                    Values = new ChartValues<double> {20, 30, 35, 45, 65, 85},
                    Title = "Electricity"
                },
                new StackedAreaSeries
                {
                    Values = new ChartValues<double> {10, 12, 18, 20, 38, 40},
                    Title = "Water"
                },
                new StackedAreaSeries
                {
                    Values = new ChartValues<double> {5, 8, 12, 15, 22, 25},
                    Title = "Solar"
                },
                new StackedAreaSeries
                {
                    Values = new ChartValues<double> {10, 12, 18, 20, 38, 40},
                    Title = "Gas"
                }
            };

            DataContext = this;
        }

       
        public SeriesCollection Series
        { get;set; }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Series.Clear();
            Series.Add(
                new LineSeries
                {
                    Values = new ChartValues<double> {120, 30, 35, 45, 65, 85},
                    Title = "XElectricity",
                    PointGeometry = null,
                    StrokeThickness = 2,
                });

            Console.WriteLine(ListBox.Items.Count);
           //lvChart.Series = Series;
            //listBox.ItemsSource = Series;
            //(Series[0] as LineSeries).Visibility = Visibility.Hidden;

        }
    }
}
