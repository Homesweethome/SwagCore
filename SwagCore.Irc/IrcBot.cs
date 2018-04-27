using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using IrcDotNet;
using SwagCore.Irc.Core;
using SwagCore.Irc.Models;

namespace SwagCore.Irc
{
    public class IrcBot: IDisposable, IIrcBot
    {
        private const int _clientTimeout = 1000;

        private StandardIrcClient _ircClient;

        private bool _isRunning;

        public string QuitMessage { get; set; }

        public IrcBot()
        {

        }

        public void Connect(string server, IrcRegistrationInfo registrationInfo)
        {
            _ircClient = new StandardIrcClient();
            _ircClient.FloodPreventer = new IrcStandardFloodPreventer(4, 2000);
            _ircClient.Connected += ClientOnConnected;
            _ircClient.Disconnected += ClientOnDisconnected;
            _ircClient.Registered += ClientOnRegistered;
            _ircClient.Connect(server, false, registrationInfo);
        }

        ~IrcBot()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_ircClient != null)
            {
                _ircClient.Quit(_clientTimeout, QuitMessage);
                _ircClient.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        private void ClientOnRegistered(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;

            OnClientRegistered(client);
        }

        private void ClientOnDisconnected(object sender, EventArgs e)
        {
            var client = (IrcClient)sender;
            OnClientDisconnect(client);
        }

        private void ClientOnConnected(object sender, EventArgs e)
        {
            _ircClient.LocalUser.NoticeReceived += LocalUserOnNoticeReceived;
            _ircClient.LocalUser.MessageReceived += LocalUserOnMessageReceived;
            _ircClient.LocalUser.JoinedChannel += LocalUserOnJoinedChannel;
            _ircClient.LocalUser.LeftChannel += LocalUserOnLeftChannel;

            OnClientRegistered(_ircClient);
        }

        private void LocalUserOnNoticeReceived(object sender, IrcMessageEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;
            OnLocalUserNoticeReceived(localUser, e);
        }

        private void LocalUserOnMessageReceived(object sender, IrcMessageEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;
            if (e.Source is IrcUser)
            {
                Console.WriteLine("LocalUserOnMessageReceived");
                if (ReadChatCommand(localUser, e))
                    return;
            }
            OnLocalUserMessageReceived(localUser, e);
        }

        private void LocalUserOnJoinedChannel(object sender, IrcChannelEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;
            e.Channel.UserJoined += IrcClientChannelUserJoined;
            e.Channel.UserLeft += IrcClientChannelUserLeft;
            e.Channel.MessageReceived += IrcClientChannelMessageReceived;
            e.Channel.NoticeReceived += IrcClientChannelNoticeReceived;
            OnLocalUserJoinedChannel(localUser, e);
        }

        private void LocalUserOnLeftChannel(object sender, IrcChannelEventArgs e)
        {
            var localUser = (IrcLocalUser)sender;

            e.Channel.UserJoined -= IrcClientChannelUserJoined;
            e.Channel.UserLeft -= IrcClientChannelUserLeft;
            e.Channel.MessageReceived -= IrcClientChannelMessageReceived;
            e.Channel.NoticeReceived -= IrcClientChannelNoticeReceived;

            OnLocalUserLeftChannel(localUser, e);
        }

        private void IrcClientChannelUserJoined(object sender, IrcChannelUserEventArgs e)
        {
            var channel = (IrcChannel)sender;
            OnChannelUserLeft(channel, e);
        }

        private void IrcClientChannelUserLeft(object sender, IrcChannelUserEventArgs e)
        {
            var channel = (IrcChannel)sender;
            OnChannelUserJoined(channel, e);
        }

        private void IrcClientChannelNoticeReceived(object sender, IrcMessageEventArgs e)
        {
            var channel = (IrcChannel)sender;
            OnChannelNoticeReceived(channel, e);
        }

        private void IrcClientChannelMessageReceived(object sender, IrcMessageEventArgs e)
        {
            var channel = (IrcChannel)sender;
            if (e.Source is IrcUser)
            {
                Console.WriteLine("IrcClientChannelMessageReceived");
                if (ReadChatCommand(channel, e))
                    return;
            }
            OnChannelMessageReceived(channel, e);
        }

        private bool ReadChatCommand(IrcChannel channel, IrcMessageEventArgs eventArgs)
        {
            var line = eventArgs.Text;
            if (line.Length > 1)
            {
                var userMessage = new UserMessage(eventArgs.Source.Name, line);
                NewMessageRecieved?.Invoke(this, new NewMessageEventArgs(userMessage, channel) {});
                return true;
            }
            return false;
        }

        private bool ReadChatCommand(IrcLocalUser user, IrcMessageEventArgs eventArgs)
        {
            var line = eventArgs.Text;
            if (line.Length > 1)
            {
                var userMessage = new UserMessage(eventArgs.Source.Name, line);
                NewMessageRecieved?.Invoke(this, new NewMessageEventArgs(userMessage, user) { });
                return true;
            }
            return false;
        }

        protected virtual void OnClientConnect(IrcClient client) { }
        protected virtual void OnClientDisconnect(IrcClient client) { }
        protected virtual void OnClientRegistered(IrcClient client) { }
        protected virtual void OnLocalUserJoinedChannel(IrcLocalUser localUser, IrcChannelEventArgs e) { }
        protected virtual void OnLocalUserLeftChannel(IrcLocalUser localUser, IrcChannelEventArgs e) { }
        protected virtual void OnLocalUserNoticeReceived(IrcLocalUser localUser, IrcMessageEventArgs e) { }
        protected virtual void OnLocalUserMessageReceived(IrcLocalUser localUser, IrcMessageEventArgs e) { }
        protected virtual void OnChannelUserJoined(IrcChannel channel, IrcChannelUserEventArgs e) { }
        protected virtual void OnChannelUserLeft(IrcChannel channel, IrcChannelUserEventArgs e) { }
        protected virtual void OnChannelNoticeReceived(IrcChannel channel, IrcMessageEventArgs e) { }
        protected virtual void OnChannelMessageReceived(IrcChannel channel, IrcMessageEventArgs e) { }

        public event EventHandler NewMessageRecieved;


        #region Actions

        public void SendMessageToChannel(IrcChannel channel, string message)
        {
            _ircClient.LocalUser.SendMessage(channel, message);
        }

        public void SendActionToChannel(IrcChannel channel, string message)
        {
            _ircClient.LocalUser.SendNotice(channel, message);
        }

        public void SendMessageToUser(IrcUser user, string message)
        {
            _ircClient.LocalUser.SendMessage(user, message);
        }

        public void JoinChannel(string channelName)
        {
            _ircClient.Channels.Join(channelName);
        }

        public void LeaveChannel(string channelName)
        {
            _ircClient.Channels.Leave(channelName);
        }
        #endregion
    }
}
