using System;

public class ScoreEntry
{
	public int Id { get; set; }
	public string SongName { get; set; }
	public int Score { get; set; }
	public int MaxCombo { get; set; }
	public double Accuracy { get; set; }
	public int MarvelousCount { get; set; }
	public int PerfectCount { get; set; }
	public int GreatCount { get; set; }
	public int GoodCount { get; set; }
    public int BadCount { get; set; }
	public int MissCount { get; set; }
	public string Grade { get; set; }
	public string DatePlayed { get; set; }
}
