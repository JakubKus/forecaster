using Forecaster.Model;
using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace Forecaster.ViewModel
{
    public class HourlyForecast : ObservableObject
    {
        static HttpClient client = new HttpClient();
        private string iconUrl, temperature;
        private int rainChance;
        private DateTime timeStamp;
        public string IconUrl
        {
            get => iconUrl;
            set => Set(() => IconUrl, ref iconUrl, value);
        }
        public DateTime TimeStamp
        {
            get => timeStamp;
            set => Set(() => TimeStamp, ref timeStamp, value);
        }
        public string Temperature
        {
            get => temperature;
            set => Set(() => Temperature, ref temperature, value);
        }
        public int RainChance
        {
            get => rainChance;
            set => Set(() => RainChance, ref rainChance, value);
        }
        public static ObservableCollection<HourlyForecast> GetForecast(string cityCode)
        {
            ObservableCollection<HourlyForecast> hourlyForecastCollection = new ObservableCollection<HourlyForecast>();
            ObservableCollection<AccuWeatherForecastModel> accuWeatherForecastCollection = new ObservableCollection<AccuWeatherForecastModel>();
            ObservableCollection<OpenWeatherForecastModel> openWeatherForecastCollection = new ObservableCollection<OpenWeatherForecastModel>();

            accuWeatherForecastCollection = GetAccuWeatherForecast(cityCode);
            openWeatherForecastCollection = GetOpenWeatherForecast(cityCode);

            int openWId = 0;
            bool openWHourExists = false;

            for (int i = 0; i < 8; i++)
            {
                openWHourExists = openWeatherForecastCollection[openWId].TimeStamp.Hour
                       == accuWeatherForecastCollection[i].TimeStamp.Hour;

                hourlyForecastCollection.Add(new HourlyForecast
                {
                    iconUrl = $"https://developer.accuweather.com/sites/default/files/{accuWeatherForecastCollection[i].IconCode.ToString().PadLeft(2, '0')}-s.png",
                    TimeStamp = accuWeatherForecastCollection[i].TimeStamp,
                    Temperature = Math.Round(openWHourExists
                        ? 0.5 * (accuWeatherForecastCollection[i].Temperature + openWeatherForecastCollection[openWId].Temperature)
                        : accuWeatherForecastCollection[i].Temperature, 1).ToString(),
                    RainChance = accuWeatherForecastCollection[i].RainChance,
                });
                openWId++;
            }
            return hourlyForecastCollection;
        }
        public static ObservableCollection<AccuWeatherForecastModel> GetAccuWeatherForecast(string cityCode)
        {
            Dictionary<string, int> cityCodes = new Dictionary<string, int>
            {
                { "sb", 275782 },
                { "ww", 274663 },
                { "kr", 274455 },
                { "el", 274340 },
                { "dw", 273125 },
                { "po", 276594 },
                { "gd", 275174 },
                { "zs", 276655 },
                { "cb", 273875 },
                { "lu", 274231 },
                { "bi", 275110 },
                { "sk", 275781 },
            };

            int cityCodeNo;
            if (cityCodes.ContainsKey(cityCode))
                cityCodes.TryGetValue(cityCode, out cityCodeNo);
            else cityCodeNo = 275782;

            ObservableCollection<AccuWeatherForecastModel> hourlyAccuWeatherCollection = new ObservableCollection<AccuWeatherForecastModel>();

            var client = new RestClient("http://dataservice.accuweather.com/forecasts/v1");
            var request = new RestRequest($"hourly/12hour/{cityCodeNo}?apikey=7ZG96kedelNLDaqTybwT5RHScsAReAhP&language=pl-pl&details=true&metric=true",
                DataFormat.Json);
            var accuWeatherResponse = client.Get(request);

            if (accuWeatherResponse.IsSuccessful)
            {
                var accuWeatherForecast = JArray.Parse(accuWeatherResponse.Content);
                foreach (var forecastField in accuWeatherForecast)
                {
                    hourlyAccuWeatherCollection.Add(new AccuWeatherForecastModel
                    {
                        IconCode = int.Parse(forecastField["WeatherIcon"].ToString()),
                        TimeStamp = DateTime.Parse(forecastField["DateTime"].ToString()),
                        RainChance = int.Parse(forecastField["RainProbability"].ToString()),
                        Temperature = double.Parse(forecastField["Temperature"]["Value"].ToString()),
                    });
                }
            }
            return hourlyAccuWeatherCollection;
        }
        public static ObservableCollection<OpenWeatherForecastModel> GetOpenWeatherForecast(string cityCode)
        {
            Dictionary<string, int> cityCodes = new Dictionary<string, int>
            {
                { "sb", 7530791 },
                { "ww", 756135 },
                { "kr", 3094802 },
                { "el", 3093133 },
                { "dw", 3081368 },
                { "po", 7530858 },
                { "gd", 7531002 },
                { "zs", 3083829 },
                { "cb", 3102014 },
                { "lu", 765876 },
                { "bi", 858789 },
                { "sk", 3096472 },
            };

            int cityCodeNo;
            if (cityCodes.ContainsKey(cityCode))
                cityCodes.TryGetValue(cityCode, out cityCodeNo);
            else cityCodeNo = 7530791;

            ObservableCollection<OpenWeatherForecastModel> hourlyOpenWeatherCollection = new ObservableCollection<OpenWeatherForecastModel>();

            var client = new RestClient("https://api.openweathermap.org/data");
            var request = new RestRequest($"2.5/forecast?id={cityCodeNo}&units=metric&lang=pl&appid=500f22be89cf299948aba55708e5f0f2", 
                DataFormat.Json);
            var openWeatherResponse = client.Get(request);

            if (openWeatherResponse.IsSuccessful)
            {
                var openWeatherForecast = JObject.Parse(openWeatherResponse.Content);
                foreach (var forecastField in openWeatherForecast["list"])
                {
                    hourlyOpenWeatherCollection.Add(new OpenWeatherForecastModel
                    {
                        Temperature = double.Parse(forecastField["main"]["temp"].ToString()),
                        TimeStamp = DateTime.Parse(forecastField["dt_txt"].ToString()),
                    });
                }
            }
            return hourlyOpenWeatherCollection;
        }
    }
}
