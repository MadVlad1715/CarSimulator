namespace CarSimulator.ViewModels.Converters
{
    internal class SpeedToGaugeConverter : MapFloatConverter
    {
        public SpeedToGaugeConverter()
            : base(0, 240, -120, 120)
        {

        }
    }
}