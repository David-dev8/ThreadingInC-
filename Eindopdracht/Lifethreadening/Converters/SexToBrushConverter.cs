using Lifethreadening.Models;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Lifethreadening.Converters
{
    public class SexToBrushConverter: IValueConverter
    {
        private static readonly IDictionary<Sex, SolidColorBrush> _colorMapping = new Dictionary<Sex, SolidColorBrush>() 
        {
            { Sex.MALE, new SolidColorBrush(ColorHelper.ToColor("#0071B0")) },
            { Sex.FEMALE, new SolidColorBrush(ColorHelper.ToColor("#FF3399")) },
        };

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return _colorMapping[(Sex)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
