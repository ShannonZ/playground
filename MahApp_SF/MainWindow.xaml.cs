using System.Windows;
using System.Windows.Controls;
using SoftFluent.Windows;

namespace MahApp_SF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private MainViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new MainViewModel();
            pSPSE.SelectedObject = vm;
            DataContext = vm;
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
                vm.NumErrors++;
            else
                vm.NumErrors--;
            System.Diagnostics.Debug.WriteLine(vm.NumErrors);
        }
    }
}
