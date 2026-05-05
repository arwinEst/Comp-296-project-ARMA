using System;

namespace Comp_296_project_ARMA.Objects
{
    public static class PlayFieldConstants
    {
        public const int LaneCount = 4;
        public const int LaneWidth = 120;
        public const int ReceptorY = 1260;
        public const int NoteHeight = 30;
        public static readonly float StartX = (2560 - (LaneWidth * LaneCount)) / 2f;
    }
}