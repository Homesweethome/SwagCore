using System;
using System.Collections.Generic;
using System.Text;

namespace SwagCore.Ai.Models
{
    public class AiResponseModel
    {
        public string Action { get; set; }
        public string Speech { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
