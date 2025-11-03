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
            SetGrid();
            Plot();
            ClearFigure();
            ChangeMouseInteractivity();
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

        private void ClearFigure()
        {
            PlotControl.Plot.Clear();
        }

        void Plot()
        {
            double[] dataX = { 1, 2, 3, 4, 5 };
            double[] dataY = { 1, 4, 9, 16, 25 };
            PlotControl.Plot.Add.Scatter(dataX, dataY);
            PlotControl.Refresh();
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

        void SetGrid()
        {
            var myPlot = PlotControl.Plot;
            // shade regions between major grid lines
            myPlot.Grid.XAxisStyle.FillColor1 = Colors.Gray.WithOpacity(0.1);
            myPlot.Grid.XAxisStyle.FillColor2 = Colors.Gray.WithOpacity(0.2);
            myPlot.Grid.YAxisStyle.FillColor1 = Colors.Gray.WithOpacity(0.1);
            myPlot.Grid.YAxisStyle.FillColor2 = Colors.Gray.WithOpacity(0.2);

            // show minor grid lines too
            myPlot.Grid.XAxisStyle.MinorLineStyle.Width = 1;
            myPlot.Grid.YAxisStyle.MinorLineStyle.Width = 1;

        }
        void ChangeMouseInteractivity()
        {
            PlotControl.UserInputProcessor.IsEnabled = true;

            // remove all existing responses so we can create and add our own
            PlotControl.UserInputProcessor.UserActionResponses.Clear();

            // middle-click-drag pan
            var panButton = ScottPlot.Interactivity.StandardMouseButtons.Right;
            var panResponse = new ScottPlot.Interactivity.UserActionResponses.MouseDragPan(panButton);
            PlotControl.UserInputProcessor.UserActionResponses.Add(panResponse);

            // right-click-drag zoom rectangle
            var zoomRectangleButton = ScottPlot.Interactivity.StandardMouseButtons.Left;
            var zoomRectangleResponse = new ScottPlot.Interactivity.UserActionResponses.MouseDragZoomRectangle(zoomRectangleButton);
            PlotControl.UserInputProcessor.UserActionResponses.Add(zoomRectangleResponse);

            // right-click autoscale
            var autoscaleButton = ScottPlot.Interactivity.StandardMouseButtons.Middle;
            var autoscaleResponse = new ScottPlot.Interactivity.UserActionResponses.SingleClickAutoscale(autoscaleButton);
            PlotControl.UserInputProcessor.UserActionResponses.Add(autoscaleResponse);

            //// left-click menu
            //var menuButton = ScottPlot.Interactivity.StandardMouseButtons.Left;
            //var menuResponse = new ScottPlot.Interactivity.UserActionResponses.SingleClickContextMenu(menuButton);
            //PlotControl.UserInputProcessor.UserActionResponses.Add(menuResponse);

            // ESC key autoscale too
            var autoscaleKey = new ScottPlot.Interactivity.Key("escape");
            Action<ScottPlot.IPlotControl, ScottPlot.Pixel> autoscaleAction = (plotControl, pixel) => plotControl.Plot.Axes.AutoScale();
            var autoscaleKeyResponse = new ScottPlot.Interactivity.UserActionResponses.KeyPressResponse(autoscaleKey, autoscaleAction);
            PlotControl.UserInputProcessor.UserActionResponses.Add(autoscaleKeyResponse);

            // WASD keys pan
            var keyPanResponse = new ScottPlot.Interactivity.UserActionResponses.KeyboardPanAndZoom()
            {
                PanUpKey = new ScottPlot.Interactivity.Key("W"),
                PanLeftKey = new ScottPlot.Interactivity.Key("A"),
                PanDownKey = new ScottPlot.Interactivity.Key("S"),
                PanRightKey = new ScottPlot.Interactivity.Key("D"),
            };
            PlotControl.UserInputProcessor.UserActionResponses.Add(keyPanResponse);
        }
    }
}
