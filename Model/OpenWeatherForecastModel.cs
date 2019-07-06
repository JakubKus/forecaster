using GalaSoft.MvvmLight;
using System;

namespace Forecaster.Model
{
    public class OpenWeatherForecastModel : ObservableObject
    {
        private DateTime timeStamp;
        private double temperature, minTemperature, maxTemperature;
        private string description;
        private int pressure;
        public DateTime TimeStamp
        {
            get => timeStamp;
            set => Set(() => TimeStamp, ref timeStamp, value);
        }
        public double Temperature
        {
            get => temperature;
            set => Set(() => Temperature, ref temperature, value);
        }
        public double MinTemperature
        {
            get => minTemperature;
            set => Set(() => MinTemperature, ref minTemperature, value);
        }
        public double MaxTemperature
        {
            get => maxTemperature;
            set => Set(() => MaxTemperature, ref maxTemperature, value);
        }
        public string Description
        {
            get => description;
            set => Set(() => Description, ref description, value);
        }
        public int Pressure
        {
            get => pressure;
            set => Set(() => Pressure, ref pressure, value);
        }
    }
}
