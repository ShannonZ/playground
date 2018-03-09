namespace plainDataValidation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel model = new ViewModel();
            model.X = 6;
            model.Y = 7;
            this.DataContext = model;
        }
    }
}
