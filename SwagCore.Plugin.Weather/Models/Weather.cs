using System;
using System.Collections.Generic;
using System.Text;

namespace SwagCore.Plugin.Weather.Models
{
    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }
}
