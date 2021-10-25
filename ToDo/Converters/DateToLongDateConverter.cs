﻿using System;
using System.Globalization;
using System.Windows.Data;


namespace ToDo.Converters
{
    class DateToLongDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.ToString("dddd, dd.MM.yyyy, HH:mm:ss", CultureInfo.CurrentCulture);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
