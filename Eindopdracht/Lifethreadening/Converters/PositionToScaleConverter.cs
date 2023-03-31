using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Lifethreadening.Converters
{
    /// <summary>
    /// Converts a leaderboard possition into a scale, making items higher in a leaderboard appear bigger
    /// </summary>
    public class PositionToScaleConverter : IValueConverter
    {
        private static readonly double minScale = 0.7;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var currentSize = double.Parse(parameter.ToString());
            var index = double.Parse(value.ToString());
            return index <= 3 ? ((1 - 0.1 * index) * currentSize) : minScale * currentSize; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
