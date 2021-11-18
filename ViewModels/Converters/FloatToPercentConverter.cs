namespace CarSimulator.ViewModels.Converters
{
    internal class FloatToPercentConverter : MapFloatConverter
    {
        public FloatToPercentConverter()
            : base(0, 1, 0, 100)
        {

        }
    }
}
