using Comp_296_project_ARMA.Objects;
using Microsoft.Data.Sqlite;
using System;

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

            connection.Close();

        }

        public void RegisterChart(Chart chart, string filePath)
        {

            var connection = new SqliteConnection("Data Source=arma.db");
            
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
    }

}


