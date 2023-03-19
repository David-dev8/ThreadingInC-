using FontAwesome.UWP;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Lifethreadening.Views.CustomControls
{
    public sealed partial class LineChartWithMultipleLines : UserControl
    {
        public static readonly DependencyProperty ItemsProperty =
           DependencyProperty.Register("Items", typeof(IDictionary<NamedEntity, IDictionary<DateTime, int>>), typeof(LineChartWithMultipleLines), new PropertyMetadata(null, new PropertyChangedCallback(InitializeFill)));

        public IDictionary<NamedEntity, IDictionary<DateTime, int>> Items
        {
            get 
            { 
                var dictionary = (IDictionary<Species, IDictionary<DateTime, int>>)GetValue(ItemsProperty);
                return dictionary.ToDictionary(k => (NamedEntity)k.Key, v => v.Value);
            }
            set { 
                SetValue(ItemsProperty, value); 
            }
        }

        public LineChartWithMultipleLines()
        {
            this.InitializeComponent();
        }

        private static void InitializeFill(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineChartWithMultipleLines chart = (LineChartWithMultipleLines)d;
            chart.FillLineChart();
        }

        private void FillLineChart()
        {
            lineChart.Series.Clear();
            foreach (KeyValuePair<NamedEntity, IDictionary<DateTime, int>> item in Items)
            {
                LineSeries lineSeries = new LineSeries();
                lineSeries.DependentValuePath = "Value";
                lineSeries.IndependentValuePath = "Key";
                lineSeries.ItemsSource = item.Value;
                lineSeries.Title = item.Key.Name;


                lineSeries.BorderThickness = new Thickness();
                lineSeries.FocusVisualPrimaryThickness = new Thickness();
                lineSeries.DataPointStyle = Resources["DataPointStyle"] as Style;
                var s = Resources["LineSeriesTemplate"] as ControlTemplate;
                lineSeries.Template = s;
                lineChart.Series.Add(lineSeries);
            }
        }
    }
}
