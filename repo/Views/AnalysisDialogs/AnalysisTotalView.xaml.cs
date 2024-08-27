
using System;
using System.Collections.ObjectModel;

using System.Windows.Controls;

namespace Client.Views
{
    /// <summary>
    /// AnalysisTotalView.xaml 的交互逻辑
    /// </summary>
    public partial class AnalysisTotalView : UserControl
    {
        public ObservableCollection<Type> AnalysisViews { get; set; }

        public int SamplingRecordID { get; set; }

        public AnalysisTotalView()
        {
            InitializeComponent();
            AnalysisViews = new ObservableCollection<Type>
            {
                typeof(SamplingView),
            };
            AnalysisViews.CollectionChanged += AnalysisViews_CollectionChanged;
            AnalysisViews.Add(typeof(SamplingView));

        }

        private void AnalysisViews_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            try
            {
               content.Content = Activator.CreateInstance(AnalysisViews[^1]);
            }
            catch
            {

            }
        }
    }
}
