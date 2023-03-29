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
    public class PositionToScaleConverter : IValueConverter
    {
        private static readonly double MIN_SCALE = 0.7;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var currentSize = double.Parse(parameter.ToString());
            var index = double.Parse(value.ToString());
            return index <= 3 ? ((1 - 0.1 * index) * currentSize) : MIN_SCALE * currentSize; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
