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
        public Car Car { get; } = new();

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
            get => Car.Brake;
            set => Car.Brake = value;
        }

        public float CarSpeed
        {
            get => Car.Speed;
        }

        public float CarFuelLevel
        {
            get => Car.FuelLevel;
        }

        #endregion

        #region Engine

        public bool EngineIsRunning
        {
            get => Car.Engine.IsRunning;
        }

        public float EngineRpm
        {
            get => Car.Engine.Rpm;
        }

        public float EngineGas
        {
            get => Car.Engine.Gas;
            set => Car.Engine.Gas = value;
        }

        public float EngineCoolantLevel
        {
            get => Car.Engine.CoolantLevel;
        }

        public float EngineCoolantTemp
        {
            get => Car.Engine.CoolantTemp;
        }

        public float EngineOilLevel
        {
            get => Car.Engine.OilLevel;
        }

        public float EngineOilTemp
        {
            get => Car.Engine.OilTemp;
        }

        #endregion

        #region Transmission

        public byte TransmissionMode
        {
            get => Car.Transmission.Mode;
            set => Car.Transmission.Mode = value;
        }

        public sbyte TransmissionGear
        {
            get => Car.Transmission.Gear;
        }

        public float TransmissionOilLevel
        {
            get => Car.Transmission.OilLevel;
        }

        public float TransmissionOilTemp
        {
            get => Car.Transmission.OilTemp;
        }

        #endregion

        #region Commands

        public ICommand StartStopEngineCommand { get; }
        private void StartStopEngineCommandExecute(object p) => Car.StartStopEngine();

        public ICommand UpdateGaugeSizeCommand { get; }
        private void UpdateGaugeSizeCommandExecute(object p)
        {
            SizeChangedEventArgs e = (SizeChangedEventArgs)p;
            updateGaugeSize(e.NewSize.Width, e.NewSize.Height);
        }
        public ICommand ChangeGearModeByLabelClickCommand { get; }
        private void ChangeGearModeByLabelClickCommandExecute(object p)
        {
            MouseEventArgs e = (MouseEventArgs)p;
            TextBlock source = e.Source as TextBlock;
            TransmissionModeToLetterConverter converter = new();
            TransmissionMode = Convert.ToByte(converter.ConvertBack(source.Text, null, null, System.Globalization.CultureInfo.CurrentCulture));
        }

        #endregion

        public MainWindowViewModel()
        {
            StartStopEngineCommand = new RelayCommand(StartStopEngineCommandExecute);
            UpdateGaugeSizeCommand = new RelayCommand(UpdateGaugeSizeCommandExecute);
            ChangeGearModeByLabelClickCommand = new RelayCommand(ChangeGearModeByLabelClickCommandExecute);

            Car.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(nameof(Car) + e.PropertyName);
            Car.Engine.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(nameof(Engine) + e.PropertyName);
            Car.Transmission.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(nameof(Transmission) + e.PropertyName);

            updateGaugeSize(WindowWidth, WindowHeight);
        }
    }
}
