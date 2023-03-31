using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Lifethreadening.Converters
{
    /// <summary>
    /// Converts A timespan into the ammount of days within the timespan
    /// </summary>
    public class TimeSpanToAmountOfDaysConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TimeSpan timeSpan = (TimeSpan)value;
            return timeSpan.Days;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            double days = (double)value;
            return new TimeSpan((int)days, 0, 0, 0);
        }
    }
}
