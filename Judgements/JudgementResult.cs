using System;

namespace Comp_296_project_ARMA.Judgements
{
    public class JudgementResult
    {
        public Judgement Judgement { get; set; }
        public double HitDifference { get; set; } // How early or late the hit was in ms
        public int Lane { get; set;  }
    }
}
       