using SwagCore.Plugin.Weather;
using Unity;

namespace SwagCore.Ai.Test.Core
{
    public static class SwagContainer
    {
        private static IUnityContainer _container;

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public static void Init()
        {
            if (_container == null)
            {
                _container = new UnityContainer();
                _container.RegisterSingleton<IDialogflow, Dialogflow>();
                _container.RegisterSingleton<PluginContainer>();
                _container.RegisterSingleton<WeatherPlugin>();
            }
        }

        public static void Start()
        {
            //todo: можем выполнить какие-либо
        }
    }
}
