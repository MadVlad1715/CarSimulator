using System;
using System.Globalization;
using System.Windows.Data;

namespace CarSimulator.ViewModels.Converters
{
    internal class TransmissionModeToLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte mode =  System.Convert.ToByte(value);

            return mode switch
            {
                0 => "P",
                1 => "R",
                2 => "N",
                3 => "D",
                4 => "S",
                _ => ""
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string mode = System.Convert.ToString(value);

            return mode switch
            {
                "P" => 0,
                "R" => 1,
                "N" => 2,
                "D" => 3,
                "S" => 4,
                _ => 0
            };
        }
    }
}
