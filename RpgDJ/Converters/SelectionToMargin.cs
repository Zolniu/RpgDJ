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
    [ValueConversion(typeof(bool), typeof(Thickness))]
    internal class SelectionToMargin : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isSelected = (bool)value;

            return new Thickness
            {
                Left = 10,
                Top = isSelected ? 0 : 10,
                Right = 10,
                Bottom = 0
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
