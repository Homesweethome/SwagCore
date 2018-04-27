using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Imgur.API.Authentication;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using SwagCore.Plugin.Base;

namespace SwagCore.Plugin.Upload
{
    public class UploadPlugin: IBasePlugin
    {
        public string PluginName => "Upload";
        public List<string> ActionsName => new List<string>() { "action.upload" };

        private IImgurClient _imgurClient;

        private static Random _random = new Random();

        public UploadPlugin()
        {
            
        }

        public async Task<string> GetReponse(Dictionary<string, object> parameters, string action)
        {
            if (_imgurClient == null)
            {
                return "Не могу загрузить без активного загрузчика";
            }

            if (!parameters.ContainsKey("url"))
            {
                return
                    "Что-то пошло не так, попытка загрузить не ссылку (UploadPlugin.GetResponse - url is null)";
            }
            if (parameters["url"].ToString().EndsWith("."))
                parameters["url"] += "jpg";

            Console.WriteLine("Запрос загрузки URL: " + parameters["url"]);

            try
            {
                if (!parameters["url"].ToString().EndsWith(".jpg") && !parameters["url"].ToString().EndsWith(".jpeg") &&
                    !parameters["url"].ToString().EndsWith(".png") && !parameters["url"].ToString().EndsWith(".gif"))
                {
                    return "Прости, пока могу загружать только изображения.";
                }

                var endpoint = new ImageEndpoint(_imgurClient);
                var response = await endpoint.UploadImageUrlAsync(parameters["url"].ToString());
                return _doneList[_random.Next(0, _doneList.Count - 1)] +  " " + response.Link;
            }
            catch (Exception e)
            {
                return "Не получилось загрузить изображение, ошибка: " + e.Message;
            }
        }

        public void Init(Dictionary<string, string> parameters)
        {
            if (parameters.ContainsKey("ClientId") && parameters.ContainsKey("ClientSecret"))
            {
                _imgurClient = new ImgurClient(parameters["ClientId"], parameters["ClientSecret"]);
            }
        }

        private List<string> _doneList = new List<string>()
        {
            "Готово!",
            "Загружено",
            "Вот ссылка",
            "Работа сделана",
            "Держи"
        };
    }
}
