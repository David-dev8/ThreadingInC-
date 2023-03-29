using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Lifethreadening.Converters
{
    public class IntPositivityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int val = (int)value;

            if (val > 0)
            {
                return ColorHelper.ToColor("#248534");
            }
            else if (val == 0)
            {
                return ColorHelper.ToColor("#000000");
            }
            else
            {
                return ColorHelper.ToColor("#F02814");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
