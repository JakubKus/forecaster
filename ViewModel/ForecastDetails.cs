using GalaSoft.MvvmLight;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace Forecaster.Model
{
    public class ForecastDetails : ObservableObject
    {
        static HttpClient client = new HttpClient();
        private string cityName, description, stormChance, airQuality, pressure, iconUrl;
        public string CityName
        {
            get => cityName;
            set => Set(() => CityName, ref cityName, value);
        }
        public string Description
        {
            get => description;
            set => Set(() => Description, ref description, value);
        }
        public string StormChance
        {
            get => stormChance;
            set => Set(() => StormChance, ref stormChance, value);
        }
        public string AirQuality
        {
            get => airQuality;
            set => Set(() => AirQuality, ref airQuality, value);
        }
        public string Pressure
        {
            get => pressure;
            set => Set(() => Pressure, ref pressure, value);
        }
        public string IconUrl
        {
            get => iconUrl;
            set => Set(() => IconUrl, ref iconUrl, value);
        }
        public static ObservableCollection<ForecastDetails> GetForecast(string cityCode)
        {
            ObservableCollection<ForecastDetails> forecastDetails = new ObservableCollection<ForecastDetails>();
            ObservableCollection<AccuWeatherForecastModel> accuWeatherForecast = new ObservableCollection<AccuWeatherForecastModel>();
            ObservableCollection<OpenWeatherForecastModel> openWeatherForecast = new ObservableCollection<OpenWeatherForecastModel>();

            accuWeatherForecast = GetAccuWeatherForecast(cityCode);
            openWeatherForecast = GetOpenWeatherForecast(cityCode);

            Dictionary<string, string> cityNames = new Dictionary<string, string>
            {
                { "sb", "Bielsko-Biała" },
                { "ww", "Warszawa" },
                { "kr", "Kraków" },
                { "el", "Łódź" },
                { "dw", "Wrocław" },
                { "po", "Poznań" },
                { "gd", "Gdańsk" },
                { "zs", "Szczecin" },
                { "cb", "Bydgoszcz" },
                { "lu", "Lublin" },
                { "bi", "Białystok" },
                { "sk", "Katowice" },
            };

            string _cityName;
            if (cityNames.ContainsKey(cityCode))
                cityNames.TryGetValue(cityCode, out _cityName);
            else _cityName = "Bielsko-Biała";

            forecastDetails.Add(new ForecastDetails
            {
                CityName = _cityName.ToUpper(),
                Description = accuWeatherForecast[0].Description.Length > openWeatherForecast[0].Description.Length
                    ? accuWeatherForecast[0].Description : openWeatherForecast[0].Description,
                AirQuality = $"{accuWeatherForecast[0].AirQuality}",
                StormChance = $"{accuWeatherForecast[0].StormChance}",
                Pressure = $"{openWeatherForecast[0].Pressure}",
                IconUrl = $"https://developer.accuweather.com/sites/default/files/{accuWeatherForecast[0].IconCode.ToString().PadLeft(2, '0')}-s.png",
            });
            return forecastDetails;
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

            ObservableCollection<AccuWeatherForecastModel> detailsAccuWeather = new ObservableCollection<AccuWeatherForecastModel>();

            var client = new RestClient("http://dataservice.accuweather.com/forecasts/v1");
            var request = new RestRequest($"daily/1day/{cityCodeNo}?apikey=7ZG96kedelNLDaqTybwT5RHScsAReAhP&language=pl-pl&details=true&metric=true",
                DataFormat.Json);
            var accuWeatherResponse = client.Get(request);

            if (accuWeatherResponse.IsSuccessful)
            {
                var accuWeatherForecast = JObject.Parse(accuWeatherResponse.Content);
                var detailsField = accuWeatherForecast["DailyForecasts"][0];

                detailsAccuWeather.Add(new AccuWeatherForecastModel
                {
                    Description = detailsField["Day"]["LongPhrase"].ToString(),
                    AirQuality = detailsField["AirAndPollen"][0]["Category"].ToString(),
                    StormChance = int.Parse(detailsField["Day"]["ThunderstormProbability"].ToString()),
                    IconCode = int.Parse(detailsField["Day"]["Icon"].ToString()),
                });
            }
            return detailsAccuWeather;
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

            ObservableCollection<OpenWeatherForecastModel> detailsOpenWeather = new ObservableCollection<OpenWeatherForecastModel>();

            var client = new RestClient("https://api.openweathermap.org/data");
            var request = new RestRequest($"2.5/weather?id={cityCodeNo}&units=metric&lang=pl&appid=500f22be89cf299948aba55708e5f0f2",
                DataFormat.Json);
            var openWeatherResponse = client.Get(request);

            if (openWeatherResponse.IsSuccessful)
            {
                var openWeatherForecast = JObject.Parse(openWeatherResponse.Content);
                detailsOpenWeather.Add(new OpenWeatherForecastModel
                {
                    Description = openWeatherForecast["weather"][0]["description"].ToString(),
                    Pressure = int.Parse(openWeatherForecast["main"]["pressure"].ToString()),
                });
            }
            return detailsOpenWeather;
        }
    }
}
