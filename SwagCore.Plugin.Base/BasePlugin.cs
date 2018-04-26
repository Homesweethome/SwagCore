using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwagCore.Plugin.Base
{
    public interface IBasePlugin
    {
        string PluginName { get; }
        string ActionName { get; }

        Task<string> GetReponse(Dictionary<string, object> parameters);
        void Init(Dictionary<string, string> parameters);
    }
}
