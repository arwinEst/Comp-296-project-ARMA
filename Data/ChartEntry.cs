using System;

namespace Comp_296_project_ARMA.Data
{
    public class ChartEntry
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string AudioFile { get; set; }
        public double BPM { get; set; }
        public double Offset { get; set; }
        public string FilePath { get; set; }
    }
}
