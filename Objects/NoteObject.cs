using System;

namespace Comp_296_project_ARMA.Objects
{
    public class NoteObject
    {
        public int Lane { get; set; }
        public double HitTime { get; set; } // Time in milliseconds when the note should be hit
        public int HoldEndTime { get; set; }
        public bool IsHold { get; set; } // 0: Tap, 1: Hold, 2: Slide
        public double Duration { get; set; } // Only for Hold and Slide notes
    }

}


