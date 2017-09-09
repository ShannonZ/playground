using System;
using System.Collections.Generic;
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
        public Model m;
        public MainWindow()
        {
            InitializeComponent();
            m = new Model();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pg1.SelectedObject = new Model();
            pg2.SelectedObject = new Model();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //pg1.SelectedObject = m;
            //pg2.SelectedObject = m;
        }
    }
}
