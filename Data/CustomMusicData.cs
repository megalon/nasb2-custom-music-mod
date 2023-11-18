using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NickCustomMusicMod.Data
{
    class CustomMusicData
    {
        public float loopStartPointSec;
        public float loopEndPointSec;
        public CustomLoopPoints loopPoints;
    }

    class CustomLoopPoints
    {
        [JsonProperty("start")]
        public int Start { get; private set; }

        [JsonProperty("end")]
        public int End { get; private set; }

        public CustomLoopPoints(int startPoint, int endPoint) 
        {
            Start = startPoint;
            End = endPoint;
        }
    }
}
