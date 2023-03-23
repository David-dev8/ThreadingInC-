using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Lifethreadening.Converters
{
    public class IndexConverter : IValueConverter
    {
        private int _index;

        public IndexConverter()
        {
            _index = 0;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            _index++;
            return _index;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
