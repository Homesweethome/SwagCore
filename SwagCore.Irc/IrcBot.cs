using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using IrcDotNet;

namespace SwagCore.Irc
{
    public class IrcBot: IDisposable
    {
        private string[] _chatCommandPrefixes = {"!c", "!с", "!s"};
        private const int _clientTimeout = 1000;

        private IrcClient _ircClient;

        private bool _isRunning;

        public string QuitMessage { get; set; }

        public IrcBot(string server, IrcRegistrationInfo registrationInfo)
        {
            _ircClient = new StandardIrcClient();
            _ircClient.FloodPreventer = new IrcStandardFloodPreventer(4, 2000);
            _ircClient.Connected += ClientOnConnected;
            _ircClient.Disconnected += ClientOnDisconnected;
            _ircClient.Registered += ClientOnRegistered;
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
            throw new NotImplementedException();
        }


        private void ClientOnDisconnected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ClientOnConnected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
