using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Lifethreadening.Converters
{
    public class PositionToDescriptionConverter: IValueConverter
    { 
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int index = (int)value;
            return $"WE WON WITH {index} AMOUNTJES";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
