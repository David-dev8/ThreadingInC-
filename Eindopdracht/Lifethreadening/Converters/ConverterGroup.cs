using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Lifethreadening.Converters
{
    public class ConverterGroup : List<IValueConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,string language)
        {
            object curValue = value;
            foreach(IValueConverter converter in this)
            {
                curValue = converter.Convert(curValue, targetType, parameter, language);
            }
            return curValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            object curValue = value;
            for(int i = Count - 1; i >= 0; i--)
            {
                curValue = this[i].ConvertBack(curValue, targetType, parameter, language);
            }
            return curValue;
        }
    }
}
