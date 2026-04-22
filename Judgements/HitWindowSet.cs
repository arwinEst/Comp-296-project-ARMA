using System;

namespace Comp_296_project_ARMA.Judgements
{
    public class HitWindowSet
    {
        public double Marvelous { get; } = 20; // Default value for Marvelous
        public double Perfect { get; } = 40; // Default value for Perfect
        public double Great { get; } = 60; // Default value for Great
        public double Good { get; } = 80; // Default value for Good
        public double Bad { get; } = 120; // Default value for Bad

        public HitWindowSet(double marvelous, double perfect, double great, double good, double bad)
        {
            Marvelous = marvelous;
            Perfect = perfect;
            Great = great;
            Good = good;
            Bad = bad;
        }

        public Judgement GetJudgement(double timingDifference)
        {
            double absDifference = Math.Abs(timingDifference);
            if (absDifference <= Marvelous)
                return Judgement.Marvelous;
            else if (absDifference <= Perfect)
                return Judgement.Perfect;
            else if (absDifference <= Great)
                return Judgement.Great;
            else if (absDifference <= Good)
                return Judgement.Good;
            else if (absDifference <= Bad)
                return Judgement.Bad;
            else
                return Judgement.Miss;
        }

    }
}
    