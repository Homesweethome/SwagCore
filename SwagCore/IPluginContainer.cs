using System.Collections.Generic;
using SwagCore.Plugin.Base;

namespace SwagCore
{
    public interface IPluginContainer
    {
        List<IBasePlugin> Plugins { get; }
        void LoadPlugins();
    }
}