using System;
using System.Globalization;
using System.Windows.Data;

namespace CarSimulator.ViewModels.Converters
{
    internal class TransmissionModeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte mode = Byte.Parse(value.ToString());

            return mode switch
            {
                0 => "Parking",
                1 => "Reverse",
                2 => "Neutral",
                3 => "Drive",
                4 => "Sport",
                _ => ""
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
