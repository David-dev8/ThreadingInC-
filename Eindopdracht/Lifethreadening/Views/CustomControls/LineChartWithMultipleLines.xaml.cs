using FontAwesome.UWP;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using WinRTXamlToolkit.Controls.DataVisualization;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Lifethreadening.Views.CustomControls
{
    public sealed partial class LineChartWithMultipleLines : UserControl
    {
        public static readonly DependencyProperty ItemsProperty =
           DependencyProperty.Register("Items", typeof(IDictionary<ChartNamedEntity, IDictionary<DateTime, int>>), typeof(LineChartWithMultipleLines), 
               new PropertyMetadata(null, new PropertyChangedCallback(InitializeFill)));

        private static readonly IList<string> _colorHexCode = new List<string>()
        {
            "#f69284", "#95b8e3", "#c095e3", "#fff384", "#e3adb5", "#231f20", "#929195", "#95dfe3", "#3e2a1f", "#f0b892", 
            "#987d84", "#008080", "#665a72", "#a99887", "#d5ede6", "#3f2b44", "#587a8f", "#53af8b", "#454727", "#8b7355"
        };

        private Queue<Color> _colors;

        public IDictionary<ChartNamedEntity, IDictionary<DateTime, int>> Items
        {
            get 
            { 
                var dictionary = (IDictionary<Species, IDictionary<DateTime, int>>)GetValue(ItemsProperty);
                return dictionary.ToDictionary(k => (ChartNamedEntity)k.Key, v => v.Value);
            }
            set 
            { 
                SetValue(ItemsProperty, value); 
            }
        }

        public LineChartWithMultipleLines()
        {
            this.InitializeComponent();
            _colors = new Queue<Color>();
        }

        private static void InitializeFill(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LineChartWithMultipleLines chart = (LineChartWithMultipleLines)d;
            chart.FillLineChart();
        }

        private void FillLineChart()
        {
            lineChart.Series.Clear();
            foreach (KeyValuePair<ChartNamedEntity, IDictionary<DateTime, int>> item in Items)
            {
                LineSeries lineSeries = new LineSeries();
                lineSeries.DependentValuePath = "Value";
                lineSeries.IndependentValuePath = "Key";
                lineSeries.ItemsSource = item.Value;
                lineSeries.Title = item.Key.Name;

                Color currentColor = GetNextColor();
                lineSeries.DataPointStyle = Resources["DataPointStyle"] as Style;
                lineSeries.Template = Resources["LineSeriesTemplate"] as ControlTemplate;
                lineSeries.FocusVisualSecondaryBrush = new SolidColorBrush(currentColor);

                var legendItemStyle = new Style(typeof(LegendItem));
                legendItemStyle.BasedOn = Resources["legend"] as Style;
                legendItemStyle.Setters.Add(new Setter(LegendItem.BackgroundProperty, currentColor));
                lineSeries.LegendItemStyle = legendItemStyle;
                lineChart.Series.Add(lineSeries);
            }
        }

        private Color GetNextColor()
        {
            if(_colors.Count == 0)
            {
                FillColorCollection();
            }
            
            Color currentColor = _colors.Dequeue();
            _colors.Enqueue(currentColor);
            return currentColor;
        }

        private void FillColorCollection()
        {
            if(_colors.Count == 0)
            {
                foreach (string hexCode in colorHexCode)
                {
                    _colors.Enqueue(Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor(hexCode));
                }
            }
        }
    }
}
