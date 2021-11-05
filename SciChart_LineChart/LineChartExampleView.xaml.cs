// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// LineChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting;
using SciChart.Charting.Model.DataSeries;
using SciChart_LineChart;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    public partial class LineChartExampleView : UserControl
    {
        public LineChartExampleView()
        {
            InitializeComponent();
        }

        private void LineChartExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {            
            // Create a DataSeries of type X=double, Y=double
            var dataSeries = new XyDataSeries<double, double>();

            lineRenderSeries.DataSeries = dataSeries;

            for (int i = 0; i < 10000; i++)
            {
                dataSeries.Append(i, System.Math.Exp(-i*1.0/100));
            }
            
            sciChart.ZoomExtents();
        }

        private void ChangeThemeClick(object sender, RoutedEventArgs e)
        {
            var customTheme = new CustomTheme();

            ThemeManager.AddTheme("CT1", customTheme); 
            /*
             * System.NullReferenceException HResult = 0x80004003
                Message = Object reference not set to an instance of an object.
                Source = SciChart.Charting
                StackTrace:
                at SciChart.Charting.Themes.ThemeColorProvider.wqx[c](String bzw, IDictionary bzx)
            */

            ThemeManager.SetTheme(sciChart, "CT1");
        }
    }
}
