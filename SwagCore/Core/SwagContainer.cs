using System;
using System.Collections.Generic;
using System.Text;
using SwagCore.Ai;
using SwagCore.Irc;
using Unity;

namespace SwagCore.Core
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
                _container.RegisterSingleton<IIrcBot, IrcBot>();
                _container.RegisterSingleton<IDialogflow, Dialogflow>();
            }
        }

        public static void Start()
        {
            //todo: можем выполнить какие-либо
        }
    }
}
