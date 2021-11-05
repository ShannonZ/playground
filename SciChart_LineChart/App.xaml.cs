using System.Windows;
using SciChart.Charting.Visuals;
using SciChart.Examples.ExternalDependencies.Controls.ExceptionView;

namespace SciChartExport
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
		     DispatcherUnhandledException += App_DispatcherUnhandledException;

			 InitializeComponent();

            // TODO: Put your SciChart License Key here if needed
            // Set this code once in App.xaml.cs or application startup
            SciChartSurface.SetRuntimeLicenseKey("jvVMZ7AkiMy+2yDsEMc7iGUeI+dc6utHwMsRWNc8uK61wLlNeQsBj4GiZVvAymvm2mz5VvxvrDejcQDSRKHI61QQ8lfDAL+672Zz5p0YmRU3QeBvqXKF85L5Uc6vhcjUtBCXspIl5TuqmJqMxdiXOAYEwsrIOLV4vxh94VStt4igGa8ZnTI41eStsjt33ZNV9ZwPX3uvbLNUKktgWE8MzleHgZbod9E7YuyWMhtf2wi8AiX+M391AJbEdeJ5glaq4ZJacPjaDgcdjrc9ChwuVSddksuc3aagNtD0a/3WiEHnkZOywEVmtoYgZCLvHFU0J1OYBBhcpF+maDqBL7KLn05XiwxDbDj0f5aFTmEtRsJ5T+VkTP+Os5Y+J7UIVKQW3lhu6TjSpSRXegzDnG7LyEOmz78Apxh/CdZbnBXtwIPslK38Nw5hyGdwQiL4gZqXKK2IsCN4ovsypItsq8eIhMQfPCFDAGrkRjFYHDd+EAbeySGNf3W4Bl+Jr8CPTPvEUz30ILhNqGphucZbejlIFTmrTFLUQ1T5DBINBQejJB9mJP260i+ZUcZD1A==");
        }

		private void App_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {            
            var exceptionView = new ExceptionView(e.Exception)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            exceptionView.ShowDialog();

            e.Handled = true;
        }
    }
}
