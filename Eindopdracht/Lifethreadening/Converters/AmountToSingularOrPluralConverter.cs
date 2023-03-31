using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Lifethreadening.Converters
{
    /// <summary>
    /// Converts a parameter into a plural version based on if the given value is not 1
    /// </summary>
    public class AmountToSingularOrPluralConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int amount = (int)value;
            string noun = (string)parameter;
            return noun + (amount != 1 ? "s" : "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
