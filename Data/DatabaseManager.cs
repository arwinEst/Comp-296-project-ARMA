using Microsoft.Data.Sqlite;
using System;

namespace Comp_296_project_ARMA.Data
{
    public class DatabaseManager
    {
        public void Initialize() {
            //Path of database
            var connection = new SqliteConnection("Data Source=arma.db");

            connection.Open();

            connection.Close();

        }

    }
}

