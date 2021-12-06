using CarSimulator.Infrastructure.Commands;
using CarSimulator.Models;
using CarSimulator.ViewModels.Base;
using CarSimulator.ViewModels.Converters;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CarSimulator.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private Car car { get; } = new();

        #region Window

        private double gaugeSize = 100d;
        public double GaugeSize
        {
            get => gaugeSize;
            set => Set(ref gaugeSize, value);
        }

        private double sliderHeight = 100d;
        public double SliderHeight
        {
            get => sliderHeight;
            set => Set(ref sliderHeight, value);
        }

        private double windowWidth = 800d;
        public double WindowWidth
        {
            get => windowWidth;
            set => Set(ref windowWidth, value);
        }

        private double windowHeight = 570d;
        public double WindowHeight
        {
            get => windowHeight;
            set => Set(ref windowHeight, value);
        }

        private void updateGaugeSize(double winWidth, double winHeight)
        {
            double newSizeX = 100 + ((winWidth - 600) / 4);
            double newSizeY = 100 + ((winHeight - 480) / 2);

            double newSize = Math.Min(newSizeX, newSizeY);

            if (newSize < 100) newSize = 100;
            else if (newSize > 350) newSize = 350;

            GaugeSize = newSize;
        }
        #endregion

        #region Car

        public float CarBrake
        {
            get => car.Brake;
            set => car.Brake = value;
        }

        public float CarSpeed
        {
            get => car.Speed;
        }

        public float CarFuelLevel
        {
            get => car.FuelLevel;
        }

        #endregion

        #region Engine

        public bool EngineIsRunning
        {
            get => car.Engine.IsRunning;
        }

        public float EngineRpm
        {
            get => car.Engine.Rpm;
        }

        public float EngineGas
        {
            get => car.Engine.Gas;
            set => car.Engine.Gas = value;
        }

        public float EngineCoolantLevel
        {
            get => car.Engine.CoolantLevel;
        }

        public float EngineCoolantTemp
        {
            get => car.Engine.CoolantTemp;
        }

        public float EngineOilLevel
        {
            get => car.Engine.OilLevel;
        }

        public float EngineOilTemp
        {
            get => car.Engine.OilTemp;
        }

        #endregion

        #region Transmission

        public byte TransmissionMode
        {
            get => car.Transmission.Mode;
            set => car.Transmission.Mode = value;
        }

        public sbyte TransmissionGear
        {
            get => car.Transmission.Gear;
        }

        public float TransmissionOilLevel
        {
            get => car.Transmission.OilLevel;
        }

        public float TransmissionOilTemp
        {
            get => car.Transmission.OilTemp;
        }

        #endregion

        #region Commands

        public ICommand StartStopEngineCommand { get; }
        private void startStopEngineCommandExecute(object p) => car.StartStopEngine();

        public ICommand UpdateGaugeSizeCommand { get; }
        private void updateGaugeSizeCommandExecute(object p)
        {
            SizeChangedEventArgs e = (SizeChangedEventArgs)p;
            updateGaugeSize(e.NewSize.Width, e.NewSize.Height);
        }
        public ICommand ChangeGearModeByLabelClickCommand { get; }
        private void changeGearModeByLabelClickCommandExecute(object p)
        {
            MouseEventArgs e = (MouseEventArgs)p;
            TextBlock source = e.Source as TextBlock;
            TransmissionModeToLetterConverter converter = new();
            TransmissionMode = Convert.ToByte(converter.ConvertBack(source.Text, null, null, System.Globalization.CultureInfo.CurrentCulture));
        }

        #endregion

        public MainWindowViewModel()
        {
            StartStopEngineCommand = new RelayCommand(startStopEngineCommandExecute);
            UpdateGaugeSizeCommand = new RelayCommand(updateGaugeSizeCommandExecute);
            ChangeGearModeByLabelClickCommand = new RelayCommand(changeGearModeByLabelClickCommandExecute);

            car.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(nameof(Car) + e.PropertyName);
            car.Engine.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(nameof(Engine) + e.PropertyName);
            car.Transmission.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(nameof(Transmission) + e.PropertyName);

            updateGaugeSize(WindowWidth, WindowHeight);
        }
    }
}
