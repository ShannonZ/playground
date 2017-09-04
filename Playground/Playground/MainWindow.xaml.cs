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
        public DispatcherTimer Timer;
        public Model m;
        public Model1 m1;
        public MainWindow()
        {
            InitializeComponent();
            m = new Model();
            m1 = new Model1();
            DataContext = m;
            sf.SelectedObject = m;
            sf1.SelectedObject = m1;

            Timer = new DispatcherTimer(DispatcherPriority.Render);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new System.TimeSpan(0, 0, 0, 0, 10);
            Timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Debug.WriteLine("m.SF="+m.SF.ToString()+"  m1.SF="+m1.SF.ToString());
        }
    }
}
