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

        public void RegisterChart(Chart chart, string filePath)
        {

            var connection = new SqliteConnection("Data Source = arma.db");
            
            connection.Open();

            // Check if chart already exists
            string checkQuery = "SELECT COUNT(*) FROM Charts WHERE FilePath = @filePath";
            using (var checkCmd = new SqliteCommand(checkQuery, connection))
            {
                checkCmd.Parameters.AddWithValue("@filePath", filePath);
                long count = (long)checkCmd.ExecuteScalar();
                if (count > 0) return; // Already registered
            }

            string insertQuery = @"
                INSERT INTO Charts (Title, Artist, AudioFile, BPM, Offset, FilePath)
                VALUES (@Title, @Artist, @AudioFile, @BPM, @Offset, @FilePath)";
            
            using (var command = new SqliteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@Title", chart.Title);
                command.Parameters.AddWithValue("@Artist", chart.Artist);
                command.Parameters.AddWithValue("@AudioFile", chart.AudioFile);
                command.Parameters.AddWithValue("@BPM", chart.BPM);
                command.Parameters.AddWithValue("@Offset", chart.Offset);
                command.Parameters.AddWithValue("@FilePath", filePath);
                command.ExecuteNonQuery();
            }
            connection.Close();

        }

        public int SaveScore(string songName, int score, int combo, double accuracy, int marvelousCount,
            int perfectCount, int greatCount, int goodCount, int badCount, int missCount, string getGrade)
        {
            var connection = new SqliteConnection("Data Source = arma.db");

            connection.Open();

            string insertQuery = @"
                    INSERT INTO Scores (SongName, Score, MaxCombo, Accuracy, MarvelousCount, PerfectCount, GreatCount, GoodCount, BadCount, MissCount, GetGrade, DatePlayed)
                    VALUES (@songName, @score, @maxCombo, @accuracy, @marvelousCount, @perfectCount, @greatCount, @goodCount, @badCount, @missCount, @getGrade, @datePlayed);
                    SELECT last_insert_rowid();";
            using (var command = new SqliteCommand(insertQuery, connection))
            {
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
                                SongName = reader.GetString(1),
                                Score = reader.GetInt32(2),
                                MaxCombo = reader.GetInt32(3),
                                Accuracy = reader.GetDouble(4),
                                MarvelousCount = reader.GetInt32(5),
                                PerfectCount = reader.GetInt32(6),
                                GreatCount = reader.GetInt32(7),
                                GoodCount = reader.GetInt32(8),
                                BadCount = reader.GetInt32(9),
                                MissCount = reader.GetInt32(10),
                                Grade = reader.GetString(11),
                                DatePlayed = reader.GetString(12)
                            };
                        }
                    }
                }
            }
            return null;
        }

    }

}


