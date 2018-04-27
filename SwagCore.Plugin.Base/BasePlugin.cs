using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SwagCore.Plugin.Base
{
    public interface IBasePlugin
    {
        string PluginName { get; }
        List<string> ActionsName { get; }

        Task<string> GetReponse(Dictionary<string, object> parameters, string action);
        void Init(Dictionary<string, string> parameters);
    }
}
