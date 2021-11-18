using System;
using System.Windows.Data;

namespace CarSimulator.ViewModels.Converters
{
    internal class IsGearSelectorEnabledConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            float brake =  System.Convert.ToSingle(values[0]);
            byte gearMode = System.Convert.ToByte(values[1]);

            return brake >= 0.9 || gearMode is 3 or 4;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
