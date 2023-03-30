using FontAwesome.UWP;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Lifethreadening.Converters
{
    /// <summary>
    /// Converts an animals sex into an Icon depicting the sex
    /// </summary>
    public class SexToIconConverter : IValueConverter
    {
        private static readonly ColorConverter _colorConverter = new ColorConverter();
        private static readonly IDictionary<Sex, FontAwesomeIcon> _colorMapping = new Dictionary<Sex, FontAwesomeIcon>()
        {
            { Sex.MALE, FontAwesomeIcon.Mars },
            { Sex.FEMALE, FontAwesomeIcon.Venus }
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
