using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenWeatherMapSharp.Enums;
using SwagCore.Plugin.Base;

namespace SwagCore.Plugin.Weather
{
    public class WeatherPlugin: IBasePlugin
    {
        private string _apiKey = "";

        public string PluginName => "Weather";
        public string ActionName => "action.weather";

        public void Init(Dictionary<string, string> parameters)
        {
            if (parameters.ContainsKey("apiKey"))
            {
                _apiKey = parameters["apiKey"];
            }
        }

        public async Task<string> GetReponse(Dictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("geo-city"))
            {
                return "Что-то пошло не так, попытка получить погоду для не города (WeatherPlugin.GetResponse - geo-city is null)";
            }
            string parameter = parameters["geo-city"].ToString();

            var owms = new OpenWeatherMapSharp.OpenWeatherMapService(_apiKey);
            var weather = await owms.GetWeatherAsync(parameter, LanguageCode.RU);
            if (!weather.IsSuccess)
            {
                return "Не удалось получить текущую погоду для " + parameter + ". Ошибка: " + weather.Error;
            }
            return "В " + parameter + " (" + weather.Response.System.Country +"): " + weather.Response.MainWeather.Temperature + "°C, "
                + weather.Response.Weather[0].Description + ", облачность " + weather.Response.Clouds.CloudinessPercent + "%"
                + ", скорость ветра " + weather.Response.Wind.Speed + "км/ч"
                + ", влажность " + weather.Response.MainWeather.Humidity
                + ", давление " + weather.Response.MainWeather.Pressure;
        }
    }
}
