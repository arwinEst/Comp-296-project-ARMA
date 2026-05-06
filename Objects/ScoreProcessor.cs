using System;
using Comp_296_project_ARMA.Judgements;

namespace Comp_296_project_ARMA.Objects
{
    public class ScoreProcessor
    {
        public int Score { get; private set; }
        public int Combo { get; private set; }
        public int MaxCombo { get; private set; }
        public int MarvelousCount { get; private set; }
        public int PerfectCount { get; private set; }
        public int GreatCount { get; private set; }
        public int GoodCount { get; private set; }
        public int BadCount { get; private set; }
        public int MissCount { get; private set; }
        public double Accuracy { get; private set; }

        private int _totalNotes;
        private int _notesHit;

        private const int MarvelousScore = 300;
        private const int PerfectScore = 300;
        private const int GreatScore = 200;
        private const int GoodScore = 100;
        private const int BadScore = 50;
        private const int MissScore = 0;

        public ScoreProcessor(int totalNotes)
        {
            _totalNotes = totalNotes;
        }

        public void ApplyJudgement(JudgementResult result)
        {
            switch (result.Judgement)
            {
                case Judgements.Judgement.Marvelous:
                    Score += MarvelousScore;
                    Combo++;
                    MarvelousCount++;
                    _notesHit++;
                    break;
                case Judgements.Judgement.Perfect:
                    Score += PerfectScore;
                    Combo++;
                    PerfectCount++;
                    _notesHit++;
                    break;
                case Judgements.Judgement.Great:
                    Score += GreatScore;
                    Combo++;
                    GreatCount++;
                    _notesHit++;
                    break;
                case Judgements.Judgement.Good:
                    Score += GoodScore;
                    Combo++;
                    GoodCount++;
                    _notesHit++;
                    break;
                case Judgements.Judgement.Bad:
                    Score += BadScore;
                    Combo++;
                    BadCount++;
                    _notesHit++;
                    break;
                case Judgements.Judgement.Miss:
                    Combo = 0;
                    MissCount++;
                    break;
            }
            if (Combo > MaxCombo)
                MaxCombo = Combo;

            UpdateAccuarcy();
        }

        public void UpdateAccuarcy()
        {
            int _totalHits = MarvelousCount + PerfectCount + GreatCount + GoodCount + BadCount + MissCount;

            if (_totalHits == 0) return; // Avoid division by zero

            double points =
                (MarvelousCount * MarvelousScore) +
                (PerfectCount * PerfectScore) +
                (GreatCount * GreatScore) +
                (GoodCount * GoodScore) +
                (BadCount * BadScore) +
                (MissCount * MissScore);

            double maxPoints = _totalHits * MarvelousScore; // Max points if all notes were Marvelous
            Accuracy = Math.Round((points / maxPoints) * 100.0, 2);
        }

        public string GetGrade()
        {
            if (Accuracy >= 100) return "SS";
            else if (Accuracy >= 95) return "S";
            else if (Accuracy >= 90) return "A";
            else if (Accuracy >= 80) return "B";
            else if (Accuracy >= 70) return "C";
            else if (Accuracy >= 60) return "D";
            else return "F";
        }
    }
}