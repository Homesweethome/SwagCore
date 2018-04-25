using System;
using System.Collections.Generic;
using System.Text;

namespace SwagCore.Irc.Models
{
    public class UserMessage
    {
        public string UserName { get; set; }
        public string Message { get; set; }

        public UserMessage() { }

        public UserMessage(string userName, string message)
        {
            UserName = userName;
            Message = message;
        }
    }
}
