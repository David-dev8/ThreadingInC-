using System;
using System.Globalization;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Lifethreadening.Converters
{

    /// <summary>
    /// Converts a filename as string into a bitmapImage
    /// </summary>
    public class StringToImageConverter : IValueConverter
    {
        private static readonly string _basePath = "ms-appdata:///local/UserUploads/";
        
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new BitmapImage(new Uri(new Uri(_basePath), (string)value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
