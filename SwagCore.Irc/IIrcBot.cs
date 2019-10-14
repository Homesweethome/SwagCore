using System;
using System.Collections.Generic;
using IrcDotNet;

namespace SwagCore.Irc
{
    public interface IIrcBot
    {
        string QuitMessage { get; set; }
        void Connect(string server, IrcRegistrationInfo registrationInfo);
        void Dispose();
        event EventHandler NewMessageRecieved;

        void SendMessageToChannel(IrcChannel channel, string message);
        void SendActionToChannel(IrcChannel channel, string message);
        void SendMessageToUser(IrcUser user, string message);

        void JoinChannel(string channelName);
        void LeaveChannel(string channelName);

        IList<IrcChannel> GetChannels();
    }
}