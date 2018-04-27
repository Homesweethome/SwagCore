using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SwagCore.Ai.Test.Core;
using SwagCore.Plugin.Base;

namespace SwagCore.Ai.Test
{
    class Program
    {
        public static IConfiguration Configuration { get; private set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            SwagContainer.Init();
            SwagContainer.Resolve<IDialogflow>().Connect(Configuration["DialogflowKey"]);
            SwagContainer.Resolve<PluginContainer>().LoadPlugins();

            var p = new Program();
            var line = Console.ReadLine();
            p.Do(line);

            Console.ReadKey();
        }

        private async Task Do(string line)
        {
            

            var result = await SwagContainer.Resolve<IDialogflow>().SendMessage(line);
            var plugin = SwagContainer.Resolve<PluginContainer>().Plugins
                .SingleOrDefault(x => x.ActionsName.Contains(result.Action));

            if (plugin == null) //if plugin with action not found - just say something
            {
                Console.WriteLine(result.Speech);
            }
            else
            {
                var pluginResponse = plugin.GetReponse(result.Parameters, result.Action).Result;
                Console.WriteLine(pluginResponse);
            }
            Console.ReadKey();
        }
    }
}
