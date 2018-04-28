using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SwagCore.Plugin.Base;
using SwagCore.Plugin.Weather.Models;

namespace SwagCore.Plugin.Weather
{
    public class WeatherPlugin: IBasePlugin
    {
        private string _apiKey = "";

        public string PluginName => "Weather";
        public List<string> ActionsName => new List<string>(){"action.weather"};

        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/";
        private readonly IRestService _restService;

        public WeatherPlugin()
        {
            _restService = new RestService();
        }

        public void Init(Dictionary<string, string> parameters)
        {
            if (parameters.ContainsKey("apiKey"))
            {
                _apiKey = parameters["apiKey"];
            }
        }

        public async Task<string> GetReponse(Dictionary<string, object> parameters, string action)
        {
            if (action == "action.weather")
                return await GetCurrentWeather(parameters);
            return "Произошла попытка запросить у плагина погоды не погоду: " + action;
        }

        private async Task<string> GetCurrentWeather(Dictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("geo-city"))
            {
                return
                    "Что-то пошло не так, попытка получить погоду для не города (WeatherPlugin.GetResponse - geo-city is null)";
            }

            string parameter = parameters["geo-city"].ToString();

            try
            {
                string queryStr = CreateQueryStructureByCityName(parameter);
                var weather = await _restService.GetAsync<WeatherMainModel>(queryStr);
                if (weather.cod == 404)
                {
                    var transliterated = Transliteration.Front(parameter);
                    queryStr = CreateQueryStructureByCityName(transliterated);
                    weather = await _restService.GetAsync<WeatherMainModel>(queryStr);
                    if (weather.cod == 404)
                    {
                        return "Что-то пошло не так, попытка получить погоду привела к ошибке 404: города " + parameter
                                                                                                            + " (" +
                                                                                                            transliterated +
                                                                                                            ")" +
                                                                                                            " не существует!";
                    }
                }
                var result = "В " + parameter + " (" + weather?.sys?.country + "): " + weather?.main?.temp + "°C, "
                             + weather?.weather[0]?.description + ", облачность " + weather?.clouds?.all + "%"
                             + ", скорость ветра " + weather?.wind?.speed + "м/с" + " направление " + weather?.wind?.direction
                             + ", влажность " + weather?.main?.humidity + "%"
                             + ", давление " + weather?.main?.pressureHuman + " мм рт ст";

                return result;
            }
            catch (Exception e)
            {
                return "Не удалось получить текущую погоду для " + parameter + ". Ошибка: " + e.Message;
            }
        }

        private string CreateQueryStructureByCityName(string cityName)
        {
            return $"{BaseUrl}weather?q={cityName}&appid={_apiKey}&units=metric&lang=RU";
        }
    }
}
