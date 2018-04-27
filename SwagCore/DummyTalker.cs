using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwagCore.Core;
using SwagCore.Irc;
using SwagCore.Irc.Models;

namespace SwagCore
{
    public class DummyTalker
    {
        private IIrcBot _ircBot;

        public DummyTalker(IIrcBot ircBot)
        {
            _ircBot = ircBot;
        }

        public string DummyTalk(UserMessage message)
        {
            string result = "";

            if (message.Message.ToLower().Contains("запретить") &&
                !message.Message.ToLower().Contains("законодательном"))
            {
                result = "На законодательном уровне!";
            }

            if (message.Message.ToLower().StartsWith("!info"))
            {
                var stringList = SwagContainer.Resolve<IPluginContainer>().Plugins.Select(x => x.PluginName).ToList();
                result = "Доступные плагины: " + string.Join(", ", stringList);
            }

            if (message.Message.ToLower().StartsWith("!join") &&
                message.UserName == Program.Configuration["Irc:BotAdmin"])
            {
                var targetChannel = message.Message.ToLower().Replace("!join", "").Trim();
                if (!targetChannel.StartsWith("#"))
                    targetChannel = "#" + targetChannel;
                _ircBot.JoinChannel(targetChannel);
            }

            if (message.Message.ToLower().StartsWith("!leave") &&
                message.UserName == Program.Configuration["Irc:BotAdmin"])
            {
                var targetChannel = message.Message.ToLower().Replace("!leave", "").Trim();
                if (!targetChannel.StartsWith("#"))
                    targetChannel = "#" + targetChannel;
                _ircBot.LeaveChannel(targetChannel);
            }

            if (message.Message.ToLower().StartsWith("!quit") &&
                message.UserName == Program.Configuration["Irc:BotAdmin"])
            {
                _ircBot.Dispose();
            }

            return result;
        }
    }
}
