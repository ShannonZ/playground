using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ListBoxDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private byte[] _imageBuffer;
        public int n;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlotPixel(int x, int y, int stride, byte redValue,
         byte greenValue, byte blueValue)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double h = lstCanvas.ActualHeight;
            double w = lstCanvas.ActualWidth;

            System.Windows.Controls.Image im = new System.Windows.Controls.Image();

            BitmapImage btm = new BitmapImage();
            btm.BeginInit();
            btm.UriSource = new Uri(@"50x50.png", UriKind.RelativeOrAbsolute);
            btm.CacheOption = BitmapCacheOption.OnLoad;
            btm.EndInit();

            im.Source = btm;
            im.Width = 100; im.Height = 50;
            im.Margin =new Thickness(2,2,2,2);

            lstCanvas.Items.Add(im);

            //BitmapData data = btm.LockBits(new Rectangle(0, 0, btm.Width, btm.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            n = 3;
        }
    }
}
