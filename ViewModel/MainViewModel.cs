using Forecaster.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;

namespace Forecaster.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            GetInitialForecastCommand = new RelayCommand(GetInitialForecast);
            GetForecastCommand = new RelayCommand<string>((s) => GetForecast(s));
        }
        public RelayCommand GetInitialForecastCommand { get; private set; }
        public RelayCommand<string> GetForecastCommand { get; private set; }
        public ObservableCollection<ForecastDetails> ForecastDetailsCollection { get; private set; }
        public ObservableCollection<HourlyForecast> HourlyForecastCollection { get; private set; }
        public ObservableCollection<DailyForecast> DailyForecastCollection { get; private set; }
        private void GetInitialForecast()
        {
            GetForecastDetails();
            GetHourlyForecast();
            GetDailyForecast();
        }
        private void GetForecast(string cityCode)
        {
            GetForecastDetails(cityCode);
            GetHourlyForecast(cityCode);
            GetDailyForecast(cityCode);
        }
        private void GetForecastDetails(string cityCode = "sb")
        {
            ObservableCollection<ForecastDetails> forecastDetailsCollection = new ObservableCollection<ForecastDetails>();
            forecastDetailsCollection = ForecastDetails.GetForecast(cityCode);
            ForecastDetailsCollection = new ObservableCollection<ForecastDetails>(forecastDetailsCollection);
            RaisePropertyChanged(() => ForecastDetailsCollection);
        }
        private void GetHourlyForecast(string cityCode = "sb")
        {
            ObservableCollection<HourlyForecast> hourlyForecastCollection = new ObservableCollection<HourlyForecast>();
            hourlyForecastCollection = HourlyForecast.GetForecast(cityCode);
            HourlyForecastCollection = new ObservableCollection<HourlyForecast>(hourlyForecastCollection);
            RaisePropertyChanged(() => HourlyForecastCollection);
        }
        private void GetDailyForecast(string cityCode = "sb")
        {
            ObservableCollection<DailyForecast> dailyForecastCollection = new ObservableCollection<DailyForecast>();
            dailyForecastCollection = DailyForecast.GetForecast(cityCode);
            DailyForecastCollection = new ObservableCollection<DailyForecast>(dailyForecastCollection);
            RaisePropertyChanged(() => DailyForecastCollection);
        }
    }
}
