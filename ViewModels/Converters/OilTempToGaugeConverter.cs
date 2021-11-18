namespace CarSimulator.ViewModels.Converters
{
    internal class OilTempToGaugeConverter : MapFloatConverter
    {
        public OilTempToGaugeConverter()
            : base(0, 180, -120, 120)
        {

        }
    }
}

