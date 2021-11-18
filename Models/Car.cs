using CarSimulator.Models.Base;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CarSimulator.Models
{
    internal class Car : BindableModel
    {
        public Engine Engine { get; }
        public Transmission Transmission { get; }

        private float brake;
        public float Brake
        {
            get => brake;
            set
            {
                if (value is >= 0f and <= 1f)
                {
                    Set(ref brake, value);
                }
            }
        }

        private float fuelLevel = 0.95f;
        public float FuelLevel
        {
            get => fuelLevel;
            set
            {
                if (value is >= 0f and <= 1f)
                {
                    Set(ref fuelLevel, value);
                }
            }
        }

        private float speed;
        public float Speed
        {
            get => speed;
            set
            {
                if (value < 0) value = 0;
                else if (value > 240) value = 240;
                Set(ref speed, value);
            }
        }

        public Car()
        {
            Engine = new(this);
            Transmission = new(this);
            Task.Run(lifeCycle);
        }

        public void StartStopEngine()
        {
            if (!Engine.IsRunning && (FuelLevel == 0f || Transmission.Mode != 0 || Engine.Load > 0)) return;

            if (Engine.IsRunning) Engine.Stop();
            else
            {
                Engine.Start();
                Transmission.Mode = Transmission.Mode;
            }
        }

        private void lifeCycle()
        {
            while (true)
            {
                if (Engine.IsRunning && FuelLevel > 0)
                {
                    float delta = Engine.Load * 0.0001f;

                    if (FuelLevel > delta) FuelLevel -= delta;
                    else FuelLevel = 0f;
                }

                if (FuelLevel == 0f && Engine.IsRunning) Engine.Stop();

                if (Transmission.Mode is 1)
                {
                    float acceleration = Engine.Load * 0.8f - (float)Math.Pow(Brake * 1.2, 4) - (float)Math.Pow(Speed, 1.15) * 0.01f;

                    Speed += acceleration;
                }
                else if (Transmission.Mode is 3 or 4)
                {
                    float transAcceleration = (float)Math.Pow(Transmission.Gear, 1.7);
                    if (transAcceleration < 3 && Engine.Rpm != Engine.minRpm) transAcceleration = 3;
                    if (transAcceleration > 7.5) transAcceleration = 7.5f;

                    float acceleration = Engine.Load * 1.5f * transAcceleration  - (float)Math.Pow(Brake * 1.6, 3) - (float)Math.Pow(Speed, 1.25) * 0.012f;

                    Speed += acceleration;
                }

                Thread.Sleep(50);
            }
        }
    }
}
