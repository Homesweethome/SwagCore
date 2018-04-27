using System;
using System.Collections.Generic;
using System.Text;

namespace SwagCore.Plugin.Weather.Models
{
    public class Sys
    {
        private static readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);

        public int type { get; set; }
        public int id { get; set; }
        public double message { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }

        public DateTime sunriseTime => unixEpoch.AddSeconds(Convert.ToInt32(sunrise));
        public DateTime sunsetTime => unixEpoch.AddSeconds(Convert.ToInt32(sunset));
    }
}
