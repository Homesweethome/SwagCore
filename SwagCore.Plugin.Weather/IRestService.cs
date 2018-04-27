using System.Threading.Tasks;

namespace SwagCore.Plugin.Weather
{
    public interface IRestService
    {
        Task<T> GetAsync<T>(string url);
    }
}