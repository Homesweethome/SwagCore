using System;
using System.Collections.Generic;
using System.Text;

namespace SwagCore.Plugin.Weather.Models
{
    public class Main
    {
        public double temp { get; set; }
        public double pressure { get; set; }
        public int humidity { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }

        public double pressureHuman => Math.Round(0.75006375541921 * pressure, 2);
    }
}
