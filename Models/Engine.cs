using System;
using System.Threading;
using System.Threading.Tasks;
using CarSimulator.Models.Base;

namespace CarSimulator.Models
{
    internal class Engine : BindableModel
    {
        public const float minRpm = 700;
        public const float maxRpm = 6000;
        public const float minGearRpm = 1400;
        public const float maxGearRpm = 2600;
        public const float minGearRpmSport = 2200;
        public const float maxGearRpmSport = 3400;
        private const float neutralRpmAcceleration = 163.73f;
        private const float reverseRpmAcceleration = 93.73f;

        private int state;
        private int State
        {
            get => state;
            set
            {
                if (Set(ref state, value))
                {
                    OnPropertyChanged(nameof(IsRunning));
                }
            }
        }

        public bool IsRunning => State == 1;

        private float gas;
        public float Gas
        {
            get => gas;
            set
            {
                if (value is >= 0f and <= 1f)
                {
                    Set(ref gas, value);
                }
            }
        }

        private float rpm;
        public float Rpm
        {
            get => rpm;
            private set
            {
                if (value < 0) value = 0;
                else if (value > maxRpm) value = maxRpm;

                Set(ref rpm, value);
            }
        }

        public float Load
        {
            get => Rpm / maxRpm;
        }

        private float coolantLevel = 0.95f;
        public float CoolantLevel
        {
            get => coolantLevel;
            set
            {
                if (value is >= 0f and <= 1f)
                {
                    Set(ref coolantLevel, value);
                }
            }
        }

        private float coolantTemp = 20f;
        public float CoolantTemp
        {
            get => coolantTemp;
            set => Set(ref coolantTemp, value);
        }

        private float oilLevel = 0.98f;
        public float OilLevel
        {
            get => oilLevel;
            set
            {
                if (value is >= 0f and <= 1f)
                {
                    Set(ref oilLevel, value);
                }
            }
        }

        private float oilTemp = 20f;
        public float OilTemp
        {
            get => oilTemp;
            set => Set(ref oilTemp, value);
        }

        private Car car;
        
        public Engine(Car car)
        {
            this.car = car;
            Task.Run(lifeCycle);
        }

        private Random rand = new();

        private void lifeCycle()
        {
            sbyte oldGear = car.Transmission.Gear;
            while (true)
            {
                float rpmAcceleration = car.Transmission.Gear switch
                {
                    -1 => reverseRpmAcceleration,
                    0 => neutralRpmAcceleration,
                    1 => 65.73f,
                    2 => 57.39f,
                    3 => 55.54f,
                    4 => 52.17f,
                    5 => 50.92f,
                    6 => 24.71f,
                    _ => neutralRpmAcceleration
                };

                if (car.Transmission.Gear == 1 && oldGear <= 0) oldGear = 1;

                if (car.Transmission.Mode is 0 or 1 or 2)
                {
                    float rpmThreshold = IsRunning ? minRpm + gas * (maxRpm - minRpm) : 0;

                    float rpmIncreaseValue = 0;
                    if (Rpm < rpmThreshold) rpmIncreaseValue = IsRunning ? gas * rpmAcceleration + (Rpm < minRpm ? rpmAcceleration / 2 : 0) : -rpmAcceleration;
                    else if (Rpm > rpmThreshold) rpmIncreaseValue = -rpmAcceleration;

                    if (Math.Abs(Rpm - rpmThreshold) > rpmAcceleration) Rpm += rpmIncreaseValue;
                    else Rpm = rpmThreshold;
                }
                else if (car.Transmission.Mode is 3 or 4)
                {
                    float rpmThreshold = 0;
                    float rpmIncreaseValue = 0;

                    if (!IsRunning)
                    {
                        rpmIncreaseValue = -rpmAcceleration;
                    }
                    else
                    {
                        float minGearRpm = car.Transmission.Gear > 1 ? car.Transmission.Mode == 3 ? Engine.minGearRpm : Engine.minGearRpmSport : minRpm;
                        float maxGearRpm = car.Transmission.Gear < 6 ? car.Transmission.Mode == 3 ? Engine.maxGearRpm : Engine.maxGearRpmSport : maxRpm;

                        if (oldGear != car.Transmission.Gear)
                        {
                            rpmThreshold = oldGear < car.Transmission.Gear ? car.Transmission.Mode == 3 ? Engine.minGearRpm : Engine.minGearRpmSport : car.Transmission.Mode == 3 ? Engine.maxGearRpm : Engine.maxGearRpmSport;

                            rpmIncreaseValue = (oldGear < car.Transmission.Gear ? -neutralRpmAcceleration : neutralRpmAcceleration);

                            if (Math.Abs(Rpm - rpmThreshold) < Math.Abs(rpmIncreaseValue)) oldGear = car.Transmission.Gear;
                        }

                        if (oldGear == car.Transmission.Gear)
                        {
                            float normalizedGas = gas % ((float)1.000001 / 6) * 6;
                            byte gasSector = (byte)((byte)1 + (byte)(gas / ((float)1.000001 / 6)));

                            rpmThreshold = minGearRpm + (gasSector == car.Transmission.Gear ? normalizedGas : gasSector > car.Transmission.Gear ? 1 : 0) * (maxGearRpm - minGearRpm);

                            rpmIncreaseValue = Rpm < rpmThreshold ? rpmAcceleration : -rpmAcceleration;
                        }
                    }

                    if (Math.Abs(Rpm - rpmThreshold) > rpmAcceleration) Rpm += rpmIncreaseValue;
                    else Rpm = rpmThreshold;
                }


                if (Load > 0)
                {
                    if (CoolantTemp < 85) CoolantTemp += Load * 0.05f;
                    if (OilTemp < 95) OilTemp += Load * 0.05f + (float)rand.NextDouble() * 0.005f;
                }
                else
                {
                    if (CoolantTemp > 20) CoolantTemp -= 0.005f;
                    if (OilTemp > 20) OilTemp -= 0.005f;
                }

                Thread.Sleep(50);
            }
        }

        public void Start()
        {
            State = 1;
        }

        public void Stop()
        {
            State = 0;
        }
    }
}
