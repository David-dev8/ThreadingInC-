using Lifethreadening.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Lifethreadening.Converters
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(!(value is Enum))
            {
                throw new ArgumentException(nameof(value));
            }
            return value.ToString().ToTitleCase();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
