using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Diagnostics;

namespace Jet64Heat
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public PlotModel model { get; set; }
        public PlotModel xpmodel { get; set; }
        public PlotModel ypmodel { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            model = new PlotModel();
            xpmodel = new PlotModel();
            ypmodel = new PlotModel();
            double x0 = 0.01;
            double x1 = 10000;
            double y0 = 0.01;
            double y1 = 10000;
            Func<double, double, double> peaks = (x, y) => x+y;
            var xx = ArrayBuilder.CreateVector(x0, x1, 100);
            var yy = ArrayBuilder.CreateVector(y0, y1, 100);
            var peaksData = ArrayBuilder.Evaluate(peaks, xx, yy);

            // Color Bar
            model.Axes.Add(new LinearColorAxis
            {
                Position = AxisPosition.Left,
                AxisDistance = 8,
                Palette = OxyPalettes.Jet(512),
                MinorTicklineColor = OxyColors.Transparent,
                AxisTickToLabelDistance = 20,
                MinimumPadding = 0,
                MaximumPadding = 0
            });

            // X Axis
            model.Axes.Add(new LogarithmicAxis
            {
                Position = AxisPosition.Top,
                MaximumPadding = 0,
                MinimumPadding = 0,
                UseSuperExponentialFormat = true,
                Title ="X Title"
            });

            xpmodel.Axes.Add(new LogarithmicAxis
            {
                Position = AxisPosition.Bottom,
                MaximumPadding = 0,
                MinimumPadding = 0,
                Minimum = 0.01,
                Maximum = 10000,
                UseSuperExponentialFormat = true,
                Title = "X Title"
            });

            ypmodel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Top,
                MaximumPadding = 0,
                MinimumPadding = 0,
                UseSuperExponentialFormat = true,
                Title = "X Title"
            });

            // Y Axis
            model.Axes.Add(new LogarithmicAxis
            {
                Position = AxisPosition.Right,
                MaximumPadding = 0,
                MinimumPadding = 0,
                UseSuperExponentialFormat = true,
                AxisTitleDistance = 5,
                Title = "Y Title"
            });

            xpmodel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                MaximumPadding = 0,
                MinimumPadding = 0,
                Minimum = 0.01,
                Maximum = 10000,
                UseSuperExponentialFormat = true,
                AxisTitleDistance = 5,
                Title = "Y Title"
            });

            ypmodel.Axes.Add(
                new LogarithmicAxis
                {
                    Position = AxisPosition.Left,
                    MaximumPadding = 0,
                    MinimumPadding = 0,
                    Minimum = 0.01,
                    Maximum = 10000,
                    UseSuperExponentialFormat = true,
                    AxisTitleDistance = 5,
                }
                );

            var hms = new HeatMapSeries { X0 = x0, X1 = x1, Y0 = y0, Y1 = y1, Data = peaksData, IsVisible = true };
            var diagLine = new LineSeries { Color= OxyColor.FromArgb(255,255,0,0),MarkerFill=OxyColor.FromArgb(255,255,0,0) };
            diagLine.Points.Add(new DataPoint(0.01, 0.01));
            diagLine.Points.Add(new DataPoint(10000, 10000));

            model.Series.Add(hms);
            model.Series.Add(diagLine);
            model.InvalidatePlot(true);

            DataContext = this;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Debug.WriteLine(model.Axes[0].MajorStep);
            Debug.WriteLine(model.Axes[1].MajorStep);
        }
    }
}
