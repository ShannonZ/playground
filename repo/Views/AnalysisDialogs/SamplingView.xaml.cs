
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using UserControl = System.Windows.Controls.UserControl;

namespace Client.Views
{
    /// <summary>
    /// SamplingView.xaml 的交互逻辑
    /// </summary>
    public partial class SamplingView : UserControl
    {
        public SamplingView()
        {
            InitializeComponent();

            Debug.WriteLine("SamplingView ctor");
        }




        private async void bt_AccumulativeSampling_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await Task.Run(() => Thread.Sleep(100000));
        }

    }
}
