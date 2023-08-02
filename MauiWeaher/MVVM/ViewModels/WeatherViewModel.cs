using MauiWeaher.MVVM.Models;
using PropertyChanged;
using System;
using System.Text.Json;
using System.Windows.Input;

namespace MauiWeaher.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class WeatherViewModel
    {
        #region Properties

        public WeatherData WeatherData { get; set; }
        public string PlaceName { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        #endregion

        #region Fields

        private HttpClient _httpClient;

        #endregion


        public WeatherViewModel()
        {
            _httpClient = new HttpClient();
        }

        #region Commands

        public ICommand SearchCommand => new Command(async (address) => {
        
            PlaceName = address.ToString();
            var location = await GetCoordinatesAsync(address.ToString());
            await GetWeather(location);

        });

        #endregion

        private async Task GetWeather(Location location)
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={location.Latitude}&longitude={location.Longitude}&daily=weathercode,temperature_2m_max,temperature_2m_min&current_weather=true&timezone=Europe%2FLondon";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                using(var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<WeatherData>(responseStream);

                    WeatherData = data;

                    for (int i = 0; i < WeatherData.daily.time.Length; i++)
                    {
                        var daily2 = new Daily2()
                        {
                            time = WeatherData.daily.time[i],
                            temperature_2m_max = WeatherData.daily.temperature_2m_max[i],
                            temperature_2m_min = WeatherData.daily.temperature_2m_min[i],
                            weathercode = WeatherData.daily.weathercode[i]
                        };

                        WeatherData.daily2.Add(daily2);
                    }
                }
            }
        }

        private async Task<Location> GetCoordinatesAsync(string address)
        {
            IEnumerable<Location> locations = await Geocoding.GetLocationsAsync(address);

            Location location = locations?.FirstOrDefault();

            if (location != null)
                Console.WriteLine($"Latitude: {location.Latitude}");

            return location;
        }
    }
}
