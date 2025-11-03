using ScottPlot;
using ScottPlot.WPF;
using System.Diagnostics;

namespace WpfScottPlot5
{
    public class VM
    {
        public WpfPlot PlotControl { get; } = new WpfPlot();

        public VM()
        {
            FastPlot();
            Crosshair();
        }

        ScottPlot.Plottables.SignalXY MySignal;
        ScottPlot.Plottables.Crosshair MyCrosshair;
        private void Crosshair()
        {
            MyCrosshair = PlotControl.Plot.Add.Crosshair(0, 0);
            MyCrosshair.IsVisible = false;
            MyCrosshair.MarkerShape = MarkerShape.OpenCircle;
            MyCrosshair.MarkerSize = 15;

            PlotControl.MouseMove += (s, e) =>
            {
                var pnt = e.GetPosition(PlotControl);
                // determine where the mouse is and get the nearest point
                Pixel mousePixel = new(pnt.X, pnt.Y);
                Coordinates mouseLocation = PlotControl.Plot.GetCoordinates(mousePixel);
                DataPoint nearest = MySignal.Data.GetNearest(mouseLocation, PlotControl.Plot.LastRender);

                // place the crosshair over the highlighted point
                if (nearest.IsReal)
                {
                    MyCrosshair.IsVisible = true;
                    MyCrosshair.Position = nearest.Coordinates;
                    PlotControl.Refresh();
                    Debug.WriteLine($"Selected Index={nearest.Index}, X={nearest.X:0.##}, Y={nearest.Y:0.##}");
                }

                // hide the crosshair when no point is selected
                if (!nearest.IsReal && MyCrosshair.IsVisible)
                {
                    MyCrosshair.IsVisible = false;
                    PlotControl.Refresh();
                    Debug.WriteLine($"No Selected Data");
                }
            };
        }


        void FastPlot()
        {

            // generate sample data with gaps
            List<double> xList = new();
            List<double> yList = new();
            for (int i = 0; i < 5; i++)
            {
                xList.AddRange(Generate.Consecutive(1000, first: 2000 * i));
                yList.AddRange(Generate.RandomSample(1000));
            }
            double[] xs = xList.ToArray();
            double[] ys = yList.ToArray();

            // add a SignalXY plot
            MySignal = PlotControl.Plot.Add.SignalXY(xs, ys);
            //sig.LineWidth = 3;
            MySignal.LegendText = "Signal";
            PlotControl.Plot.ScaleFactor = 2;
            PlotControl.Plot.Axes.Bottom.Label.Text = "X Label";
            PlotControl.Plot.YLabel("Y Label");
            PlotControl.Plot.Axes.SetLimitsX(300, 1000);

            // create a static function containing the string formatting logic
            static string CustomFormatter(double position)
            {
                return $"{position:N3}";
            }

            // create a custom tick generator using your custom label formatter
            ScottPlot.TickGenerators.NumericAutomatic myTickGenerator = new()
            {
                LabelFormatter = CustomFormatter
            };
            PlotControl.Plot.Axes.Bottom.TickGenerator = myTickGenerator;
            PlotControl.Refresh();
        }
    }
}
