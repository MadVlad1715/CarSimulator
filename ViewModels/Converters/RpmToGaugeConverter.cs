namespace CarSimulator.ViewModels.Converters
{
    internal class RpmToGaugeConverter : MapFloatConverter
    {
        public RpmToGaugeConverter()
            : base(0, 6000, -120, 120)
        {

        }
    }
}
