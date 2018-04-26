using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SwagCore.Plugin.Base;

namespace SwagCore.Ai.Test
{
    public class PluginContainer
    {
        public List<IBasePlugin> Plugins { get; private set; }

        public void LoadPlugins()
        {
            Plugins = new List<IBasePlugin>();

            var files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Plugins")).ToList().Where(x => x.Contains("SwagCore.Plugin") && x.EndsWith(".dll"));
            foreach (var file in files)
            {
                var myAssembly = AssemblyLoader.LoadFromAssemblyPath(file);
                var myType = myAssembly.GetTypes().SingleOrDefault(x => x.GetInterfaces().Any(y => y.Name == nameof(IBasePlugin)));
                if (myType == null)
                    continue;
                IBasePlugin myInstance = (IBasePlugin)Activator.CreateInstance(myType);

                var configuration = Program.Configuration.GetSection("Plugins:" + myInstance.PluginName);   //find plugin parameters in appsettings.json
                if (configuration != null)
                {
                    var children = configuration.GetChildren();
                    var parameters = children.Select(x => new KeyValuePair<string, string>(x.Key, x.Value));
                    myInstance.Init(new Dictionary<string, string>(parameters));
                }
                if (!Plugins.Any(x => x.PluginName == myInstance.PluginName || x.ActionName == myInstance.ActionName))  //if plugin keywords empty - add plugin
                    Plugins.Add(myInstance);
            }
        }
    }
}
