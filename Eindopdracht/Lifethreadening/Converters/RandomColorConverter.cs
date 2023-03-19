using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Lifethreadening.Converters
{
    public class RandomColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Microsoft.Toolkit.Uwp.Helpers.ColorHelper.ToColor("#59ff00");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
