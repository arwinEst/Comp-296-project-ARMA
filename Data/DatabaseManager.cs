using Comp_296_project_ARMA.Objects;
using Microsoft.Data.Sqlite;
using System;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

namespace Comp_296_project_ARMA.Data
{
    public class DatabaseManager
    {
        public void Initialize()
        {

            
            //Path of database
            var connection = new SqliteConnection("Data Source=arma.db");

            connection.Open();

            using (var pragma = new SqliteCommand("PRAGMA foreign_keys = ON;", connection))
                pragma.ExecuteNonQuery();

            string createChartsTable = @"
                CREATE TABLE IF NOT EXISTS Charts (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Artist TEXT NOT NULL,
                    AudioFile TEXT NOT NULL,
                    BPM REAL NOT NULL,
                    Offset REAL NOT NULL,
                    FilePath TEXT NOT NULL
                )";
            using (var command = new SqliteCommand(createChartsTable, connection))
                command.ExecuteNonQuery();

            string createScoresTable = @"
                CREATE TABLE IF NOT EXISTS Scores (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ChartId INTEGER NOT NULL REFERENCES Charts(Id) ON DELETE CASCADE,
                    songName TEXT NOT NULL,
                    score INTEGER NOT NULL,
                    maxCombo INTEGER NOT NULL,
                    accuracy DOUBLE NOT NULL,
                    marvelousCount INTEGER NOT NULL,
                    perfectCount INTEGER NOT NULL,
                    greatCount INTEGER NOT NULL,
                    goodCount INTEGER NOT NULL,
                    badCount INTEGER NOT NULL,
                    missCount INTEGER NOT NULL,
                    getGrade TEXT,
                    datePlayed DATETIME
                )";
            using (var command = new SqliteCommand(createScoresTable, connection))
                command.ExecuteNonQuery();

            connection.Close();

        }

        public int RegisterChart(Chart chart, string filePath)
        {

            var connection = new SqliteConnection("Data Source = arma.db");
            connection.Open();

            // Check if chart already exists
            string checkQuery = "SELECT Id FROM Charts WHERE FilePath = @filePath";
            using (var checkCmd = new SqliteCommand(checkQuery, connection))
            {
                checkCmd.Parameters.AddWithValue("@filePath", filePath);
                var existing = checkCmd.ExecuteScalar();
                if (existing != null)
                {
                    return Convert.ToInt32(existing);
                }
            }

            string insertQuery = @"
                INSERT INTO Charts (Title, Artist, AudioFile, BPM, Offset, FilePath)
                VALUES (@Title, @Artist, @AudioFile, @BPM, @Offset, @FilePath);
                SELECT last_insert_rowid();";
            using (var command = new SqliteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Title", chart.Title);
                command.Parameters.AddWithValue("@Artist", chart.Artist);
                command.Parameters.AddWithValue("@AudioFile", chart.AudioFile);
                command.Parameters.AddWithValue("@BPM", chart.BPM);
                command.Parameters.AddWithValue("@Offset", chart.Offset);
                command.Parameters.AddWithValue("@FilePath", filePath);

                int dbId = Convert.ToInt32(command.ExecuteScalar());
                return dbId;
            }

        }

        public int SaveScore(int chartId,string songName, int score, int combo, double accuracy, int marvelousCount,
            int perfectCount, int greatCount, int goodCount, int badCount, int missCount, string getGrade)
        {
            var connection = new SqliteConnection("Data Source = arma.db");

            connection.Open();

            using (var pragma = new SqliteCommand("PRAGMA foreign_keys = ON;", connection))
                pragma.ExecuteNonQuery();

            string insertQuery = @"
                    INSERT INTO Scores (ChartId, SongName, Score, MaxCombo, Accuracy, MarvelousCount, PerfectCount, GreatCount, GoodCount, BadCount, MissCount, GetGrade, DatePlayed)
                    VALUES (@chartId, @songName, @score, @maxCombo, @accuracy, @marvelousCount, @perfectCount, @greatCount, @goodCount, @badCount, @missCount, @getGrade, @datePlayed);
                    SELECT last_insert_rowid();";
            using (var command = new SqliteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@chartId", chartId);
                command.Parameters.AddWithValue("@songName", songName);
                command.Parameters.AddWithValue("@score", score);
                command.Parameters.AddWithValue("@maxCombo", combo);
                command.Parameters.AddWithValue("@accuracy", accuracy);
                command.Parameters.AddWithValue("@marvelousCount", marvelousCount);
                command.Parameters.AddWithValue("@perfectCount", perfectCount);
                command.Parameters.AddWithValue("@greatCount", greatCount);
                command.Parameters.AddWithValue("@goodCount", goodCount);
                command.Parameters.AddWithValue("@badCount", badCount);
                command.Parameters.AddWithValue("@missCount", missCount);
                command.Parameters.AddWithValue("@getGrade", getGrade);
                command.Parameters.AddWithValue("@datePlayed", DateTime.Now.ToString());

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public ScoreEntry GetScore(int id)
        {
            using (var connection = new SqliteConnection("Data Source = arma.db"))
            {
                connection.Open();
                string query = "SELECT * FROM Scores WHERE Id = @Id";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ScoreEntry
                            {
                                Id = reader.GetInt32(0),
                                ChartId = reader.GetInt32(1),
                                SongName = reader.GetString(2),
                                Score = reader.GetInt32(3),
                                MaxCombo = reader.GetInt32(4),
                                Accuracy = reader.GetDouble(5),
                                MarvelousCount = reader.GetInt32(6),
                                PerfectCount = reader.GetInt32(7),
                                GreatCount = reader.GetInt32(8),
                                GoodCount = reader.GetInt32(9),
                                BadCount = reader.GetInt32(10),
                                MissCount = reader.GetInt32(11),
                                Grade = reader.GetString(12),
                                DatePlayed = reader.GetString(13)
                            };
                        }
                    }
                }
            }
            return null;
        }

    }

}


