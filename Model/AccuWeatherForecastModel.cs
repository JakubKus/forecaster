using GalaSoft.MvvmLight;
using System;

namespace Forecaster.Model
{
    public class AccuWeatherForecastModel : ObservableObject
    {
        private int iconCode, rainChance, stormChance;
        private double temperature, minTemperature, maxTemperature;
        private DateTime timeStamp;
        private string description, airQuality;
        public int IconCode
        {
            get => iconCode;
            set => Set(() => IconCode, ref iconCode, value);
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
        public DateTime TimeStamp
        {
            get => timeStamp;
            set => Set(() => TimeStamp, ref timeStamp, value);
        }
        public string Description
        {
            get => description;
            set => Set(() => Description, ref description, value);
        }
        public int RainChance
        {
            get => rainChance;
            set => Set(() => RainChance, ref rainChance, value);
        }
        public string AirQuality
        {
            get => airQuality;
            set => Set(() => AirQuality, ref airQuality, value);
        }
        public int StormChance
        {
            get => stormChance;
            set => Set(() => StormChance, ref stormChance, value);
        }
    }
}
