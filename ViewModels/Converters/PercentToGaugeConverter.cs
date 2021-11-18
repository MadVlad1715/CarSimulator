namespace CarSimulator.ViewModels.Converters
{
    internal class PercentToGaugeConverter : MapFloatConverter
    {
        public PercentToGaugeConverter()
            : base(0, 1, -120, 120)
        {

        }
    }
}
