using NickCustomMusicMod.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace NASB2CustomMusicMod.Management
{
    internal class CustomMusicTrack : MusicTrack
    {
        public float LoopStartPointSec;
        public float LoopEndPointSec;
        public CustomLoopPoints LoopPoints;
    }
}
