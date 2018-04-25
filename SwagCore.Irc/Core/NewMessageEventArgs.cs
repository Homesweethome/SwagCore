using System;
using System.Collections.Generic;
using System.Text;
using IrcDotNet;
using SwagCore.Irc.Models;

namespace SwagCore.Irc.Core
{
    public class NewMessageEventArgs: EventArgs
    {
        public UserMessage UserMessage { get; set; }
        public IrcChannel Channel { get; set; }
        public IrcUser User { get; set; }

        public NewMessageEventArgs(UserMessage userMessage, IrcChannel channel)
        {
            UserMessage = userMessage;
            Channel = channel;
        }

        public NewMessageEventArgs(UserMessage userMessage, IrcUser user)
        {
            UserMessage = userMessage;
            User = user;
        }
    }
}
