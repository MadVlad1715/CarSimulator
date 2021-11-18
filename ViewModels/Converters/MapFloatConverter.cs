using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CarSimulator.ViewModels.Converters
{
    internal class MapFloatConverter : IValueConverter
    {
        protected float minIn, maxIn, minOut, maxOut;

        public MapFloatConverter(float minIn, float maxIn, float minOut, float maxOut)
        {
            this.minIn = minIn;
            this.maxIn = maxIn;
            this.minOut = minOut;
            this.maxOut = maxOut;
        }

        virtual public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float res = ((float)value - minIn) / (maxIn - minIn) * (maxOut - minOut) + minOut;

            if (res < minOut) res = minOut;
            else if (res > maxOut) res = maxOut;

            return res;
        }

        virtual public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
