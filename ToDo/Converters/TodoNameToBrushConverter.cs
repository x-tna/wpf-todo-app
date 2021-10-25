using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ToDo.Converters
{
    class TodoNameToBrushConverter : IValueConverter
    {
        public SolidColorBrush ForegroundLixtBox { get; set; }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var todoItemName = value.ToString();

            if (todoItemName.EndsWith("!"))
            {
                return Brushes.Red;
            } else
            {
                return ForegroundLixtBox;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
