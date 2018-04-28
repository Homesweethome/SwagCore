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
        private static readonly string[] ChatCommandPrefixes = { "!c", "!с", "!s" };

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

            while (true)
            {
                System.Threading.Thread.Sleep(50);
            }
        }

        private static async void Program_NewMessageRecieved(object sender, EventArgs e)
        {
            var messageEvent = e as NewMessageEventArgs;
            Console.WriteLine(messageEvent.Channel.Name + " " + messageEvent.UserMessage.Message);

            if (ChatCommandPrefixes.Any(x => messageEvent.UserMessage.Message.StartsWith(x)))
            {
                var trimmedMessage = messageEvent.UserMessage.Message.Remove(0, 2).Trim();
                var result = await SwagContainer.Resolve<IDialogflow>().SendMessage(trimmedMessage);
                Console.WriteLine(result.Action + " " + result.Speech);
                var plugins = SwagContainer.Resolve<IPluginContainer>().Plugins;
                var plugin = plugins.SingleOrDefault(x => x.ActionsName.Contains(result.Action));

                string response = "";
                if (plugin == null) //if plugin with action not found - just say something
                {
                    response = result.Speech;
                }
                else
                {
                    response = await plugin.GetReponse(result.Parameters, result.Action);                    
                }

                SwagContainer.Resolve<IIrcBot>()
                    .SendMessageToChannel(messageEvent.Channel, response);
            }
            else
            {
                var dummyTalker = SwagContainer.Resolve<DummyTalker>();
                var response = dummyTalker.DummyTalk(messageEvent.UserMessage);
                if (!string.IsNullOrEmpty(response))
                {
                    SwagContainer.Resolve<IIrcBot>()
                        .SendMessageToChannel(messageEvent.Channel, response);
                }
            }
        }
    }
}
