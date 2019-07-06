using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace Forecaster.Model
{
    public class DailyForecast : ObservableObject
    {
        static HttpClient client = new HttpClient();
        private DateTime timeStamp;
        private string iconUrl, minTemperature, maxTemperature, rainChance;
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
        public string MinTemperature
        {
            get => minTemperature;
            set => Set(() => MinTemperature, ref minTemperature, value);
        }
        public string MaxTemperature
        {
            get => maxTemperature;
            set => Set(() => MaxTemperature, ref maxTemperature, value);
        }
        public string RainChance
        {
            get => rainChance;
            set => Set(() => RainChance, ref rainChance, value);
        }
        public static ObservableCollection<DailyForecast> GetForecast(string cityCode)
        {
            ObservableCollection<DailyForecast> dailyForecastCollection = new ObservableCollection<DailyForecast>();
            ObservableCollection<AccuWeatherForecastModel> accuWeatherForecastCollection = new ObservableCollection<AccuWeatherForecastModel>();
            ObservableCollection<OpenWeatherForecastModel> openWeatherForecastCollection = new ObservableCollection<OpenWeatherForecastModel>();

            accuWeatherForecastCollection = GetAccuWeatherForecast(cityCode);
            openWeatherForecastCollection = GetOpenWeatherForecast(cityCode);

            for (int i = 0; i < 4; i++)
            {
                dailyForecastCollection.Add(new DailyForecast
                {
                    IconUrl = $"https://developer.accuweather.com/sites/default/files/{accuWeatherForecastCollection[i].IconCode.ToString().PadLeft(2, '0')}-s.png",
                    TimeStamp = accuWeatherForecastCollection[i].TimeStamp,
                    MinTemperature = Math.Round(
                        0.5 * (accuWeatherForecastCollection[i].MinTemperature + openWeatherForecastCollection[i].MinTemperature), 1).ToString(),
                    MaxTemperature = Math.Round(
                        0.5 * (accuWeatherForecastCollection[i].MaxTemperature + openWeatherForecastCollection[i].MaxTemperature), 1).ToString(),
                    RainChance = accuWeatherForecastCollection[i].RainChance.ToString(),
                });
            }
            return dailyForecastCollection;
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

            ObservableCollection<AccuWeatherForecastModel> dailyAccuWeatherCollection = new ObservableCollection<AccuWeatherForecastModel>();

            var client = new RestClient("http://dataservice.accuweather.com/forecasts/v1");
            var request = new RestRequest($"daily/5day/{cityCodeNo}?apikey=7ZG96kedelNLDaqTybwT5RHScsAReAhP&language=pl-pl&details=true&metric=true",
                DataFormat.Json);
            var accuWeatherResponse = client.Get(request);

            if (accuWeatherResponse.IsSuccessful)
            {
                var accuWeatherForecast = JObject.Parse(accuWeatherResponse.Content);
                foreach (var forecastField in accuWeatherForecast["DailyForecasts"])
                {
                    dailyAccuWeatherCollection.Add(new AccuWeatherForecastModel
                    {
                        IconCode = int.Parse(forecastField["Day"]["Icon"].ToString()),
                        TimeStamp = DateTime.Parse(forecastField["Date"].ToString()),
                        RainChance = int.Parse(forecastField["Day"]["RainProbability"].ToString()),
                        MinTemperature = double.Parse(forecastField["Temperature"]["Minimum"]["Value"].ToString()),
                        MaxTemperature = double.Parse(forecastField["Temperature"]["Maximum"]["Value"].ToString()),
                    });
                }
            }
            return dailyAccuWeatherCollection;
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

            ObservableCollection<OpenWeatherForecastModel> dailyOpenWeatherCollection = new ObservableCollection<OpenWeatherForecastModel>();

            var client = new RestClient("https://api.openweathermap.org/data");
            var request = new RestRequest($"2.5/forecast?id={cityCodeNo}&units=metric&lang=pl&appid=500f22be89cf299948aba55708e5f0f2",
                DataFormat.Json);
            var openWeatherResponse = client.Get(request);

            if (openWeatherResponse.IsSuccessful)
            {
                var openWeatherForecast = JObject.Parse(openWeatherResponse.Content);
                foreach (var forecastField in openWeatherForecast["list"])
                {
                    dailyOpenWeatherCollection.Add(new OpenWeatherForecastModel
                    {
                        MinTemperature = double.Parse(forecastField["main"]["temp_min"].ToString()),
                        MaxTemperature = double.Parse(forecastField["main"]["temp_max"].ToString()),
                        TimeStamp = DateTime.Parse(forecastField["dt_txt"].ToString()),
                    });
                }
            }
            return dailyOpenWeatherCollection;
        }
    }
}
