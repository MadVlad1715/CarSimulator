using CarSimulator.Models.Base;
using System.Threading;
using System.Threading.Tasks;

namespace CarSimulator.Models
{
    internal class Transmission : BindableModel
    {
        private byte mode;
        public byte Mode
        {
            get => mode;
            set
            {
                if (value is >= 0 and <= 4)
                {
                    if (car.Brake < 0.9 && (mode != 3 || value != 4) && (mode != 4 || value != 3)) return;

                    Set(ref mode, value);

                    if (car.Engine.IsRunning)
                    {
                        if (mode is 0 or 2) Gear = 0;
                        else if (mode is 1) Gear = -1;
                        else if (mode is 3 or 4 && Gear <= 0) Gear = 1;
                    }
                }
            }
        }

        private sbyte gear;
        public sbyte Gear
        {
            get => gear;
            set
            {
                if (value is >= -1 and <= 6)
                {
                    Set(ref gear, value);
                }
            }
        }

        private float oilLevel = 0.9f;
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

        public Transmission(Car car)
        {
            this.car = car;
            Task.Run(lifeCycle);
        }

        private void lifeCycle()
        {
            while (true)
            {
                if (Mode is 3 or 4 && car.Engine.IsRunning)
                {
                    if (car.Engine.Rpm >= (Mode == 3 ? Engine.maxGearRpm : Engine.maxGearRpmSport) && Gear < 6)
                    {
                        if (gear == 1 && car.Speed >= 15 ||
                            gear == 2 && car.Speed >= 30 ||
                            gear == 3 && car.Speed >= 50 ||
                            gear == 4 && car.Speed >= 75 ||
                            gear == 5 && car.Speed >= 105) Gear++;
                    }
                    else if (car.Engine.Rpm <= (Mode == 3 ? Engine.minGearRpm : Engine.minGearRpmSport) && Gear > 1)
                    {
                        if (gear == 2 && car.Speed < 15 ||
                            gear == 3 && car.Speed < 30 ||
                            gear == 4 && car.Speed < 50 ||
                            gear == 5 && car.Speed < 75 ||
                            gear == 6 && car.Speed < 105) Gear--;
                    }
                }

                if (car.Speed > 0)
                {
                    if (OilTemp < 80) OilTemp += car.Speed / 80 * 0.05f;
                }
                else
                {
                    if (OilTemp > 20) OilTemp -= 0.005f;
                }

                Thread.Sleep(50);
            }
        }
    }
}
