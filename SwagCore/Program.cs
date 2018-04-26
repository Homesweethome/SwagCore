using System;
using System.IO;
using System.Linq;
using IrcDotNet;
using Microsoft.Extensions.Configuration;
using SwagCore.Ai;
using SwagCore.Core;
using SwagCore.Irc;
using SwagCore.Irc.Core;

namespace SwagCore
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
            SwagContainer.Resolve<IPluginContainer>().LoadPlugins();
            SwagContainer.Resolve<IIrcBot>().Connect(Configuration["Irc:Server"], new IrcUserRegistrationInfo()
            {
                NickName = Configuration["Irc:UserName"],
                Password = Configuration["Irc:Password"],
                RealName = Configuration["Irc:UserName"],
                UserName = Configuration["Irc:UserName"]
            });
            SwagContainer.Resolve<IIrcBot>().NewMessageRecieved += Program_NewMessageRecieved;

            System.Threading.Thread.Sleep(5000);

            SwagContainer.Resolve<IIrcBot>().JoinChannel("#test");

            Console.ReadKey();
        }

        private static async void Program_NewMessageRecieved(object sender, EventArgs e)
        {
            var messageEvent = e as NewMessageEventArgs;
            Console.WriteLine(messageEvent.Channel.Name + " " + messageEvent.UserMessage.Message);

            if (messageEvent.UserMessage.Message.StartsWith("!к"))
            {
                var trimmedMessage = messageEvent.UserMessage.Message.Remove(0, 2).Trim();
                var result = await SwagContainer.Resolve<IDialogflow>().SendMessage(trimmedMessage);
                Console.WriteLine(result.Action + " " + result.Speech);
                var plugins = SwagContainer.Resolve<IPluginContainer>().Plugins;
                Console.WriteLine("Total plugins: " + plugins.Count + " " + result.Action);
                var plugin = plugins.SingleOrDefault(x => x.ActionName == result.Action);

                string response = "";
                if (plugin == null) //if plugin with action not found - just say something
                {
                    response = result.Speech;
                }
                else
                {
                    response = await plugin.GetReponse(result.Parameters);                    
                }

                SwagContainer.Resolve<IIrcBot>()
                    .SendMessageToChannel(messageEvent.Channel, response);
            }
        }
    }
}
