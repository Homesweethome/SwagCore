using System;
using System.IO;
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
            SwagContainer.Resolve<IIrcBot>().Connect(Configuration["Irc:Server"], new IrcUserRegistrationInfo()
            {
                NickName = Configuration["Irc:UserName"],
                Password = Configuration["Irc:Password"],
                RealName = Configuration["Irc:UserName"],
                UserName = Configuration["Irc:UserName"]
            });
            SwagContainer.Resolve<IIrcBot>().NewMessageRecieved += Program_NewMessageRecieved;
            System.Threading.Thread.Sleep(1000);
            SwagContainer.Resolve<IIrcBot>().JoinChannel("#test");

            Console.ReadKey();
        }

        private static void Program_NewMessageRecieved(object sender, EventArgs e)
        {
            var messageEvent = e as NewMessageEventArgs;
            Console.WriteLine(messageEvent.Channel.Name + " " + messageEvent.UserMessage.Message);

            if (messageEvent.Channel.Name == "#test")
            {
                var result = SwagContainer.Resolve<IDialogflow>().SendMessage(messageEvent.UserMessage.Message);

                SwagContainer.Resolve<IIrcBot>()
                    .SendMessageToChannel(messageEvent.Channel, result);
            }
        }
    }
}
