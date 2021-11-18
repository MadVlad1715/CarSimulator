using System;
using System.Globalization;
using System.Windows.Data;

namespace CarSimulator.ViewModels.Converters
{
    internal class GearToLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            sbyte gear = System.Convert.ToSByte(value);

            return gear switch
            {
                -1 => "R",
                0 => "N",
                1 or 2 or 3 or 4 or 5 or 6 => gear,
                _ => ""
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
