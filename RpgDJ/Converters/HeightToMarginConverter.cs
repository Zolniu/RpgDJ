using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace RpgDJ.Converters
{
    [ValueConversion(typeof(int), typeof(Thickness))]
    internal class HeightToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var height = (int)value;

            return new Thickness
            {
                Left = 0,
                Top = height <= 3 ? 15 : 50,
                Right = 5,
                Bottom = 50
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
