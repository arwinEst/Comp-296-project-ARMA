using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Comp_296_project_ARMA.Objects;

namespace Comp_296_project_ARMA.Objects
{
    public class Chart
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string AudioFile { get; set; }
        public string Background { get; set; }
        public double BPM { get; set; }
        public double Offset { get; set; }
        public List<NoteObject> Notes { get; set; } = new List<NoteObject>();
    }

}