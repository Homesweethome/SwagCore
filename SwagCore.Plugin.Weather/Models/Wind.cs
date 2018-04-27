using System;
using System.Collections.Generic;
using System.Text;

namespace SwagCore.Plugin.Weather.Models
{
    public class Wind
    {
        private static readonly string[] DirectionArray = {"северный", "северо-северо-восточный", "северо-восточный",
            "восточно-северо-восточный", "восточный", "восточно-юго-восточный", "юго-восточный", "юго-юго-восточный",
            "южный", "юго-юго-западный", "юго-западный", "западо-юго-западный", "западный",
            "западно-северо-западный", "северо-западный", "северо-северо-западный"};

        public double speed { get; set; }
        public double deg { get; set; }

        public string direction
        {
            get
            {
                int val = (int)Math.Round(deg / 22.5 + .5);

                return DirectionArray[(val % 16)];
            }
        }
    }
}
