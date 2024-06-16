using Microsoft.Data.Sqlite;

namespace _2048Game
{
    public static class Database
    {
        public static void CreateDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Create save game table if not existing
                string createSaveTable = @"CREATE TABLE IF NOT EXISTS SAVED_GAME(
                        Id INTEGER PRIMARY KEY,
                        Name TEXT NOT NULL,
                        SaveProgress TEXT NOT NULL
                )";
                using (var command = new SqliteCommand(createSaveTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Create high score table if not existing
                string createHighScoresTable = @"CREATE TABLE IF NOT EXISTS HIGH_SCORE(
                        Id INTEGER PRIMARY KEY,
                        User TEXT NOT NULL,
                        Score INT NOT NULL
                )";
                using (var command = new SqliteCommand(createHighScoresTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        public static void InsertSaveGame(string saveName, string saveGame)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Create new save game
                string insertNewSaveGame = @"INSERT INTO SAVED_GAME(Name, SaveProgress)
                        VALUES (@saveName, @saveGame)";
                using (var command = new SqliteCommand(insertNewSaveGame, connection))
                {
                    command.Parameters.AddWithValue("@saveName", saveName);
                    command.Parameters.AddWithValue("@saveGame", saveGame);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        public static List<string> GetSaveGameNames()
        {
            var result = new List<string>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Get all existing save game names
                string getAllSaveGameNames = "SELECT * FROM SAVED_GAME";
                using (var command = new SqliteCommand(getAllSaveGameNames, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(reader["Name"].ToString());
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }
        public static string GetSaveGameByName(string name)
        {
            string result = string.Empty;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Get save game by name
                string getSaveGameByName = "SELECT * FROM SAVED_GAME WHERE Name = @name";
                using (var command = new SqliteCommand(getSaveGameByName, connection))
                {
                    command.Parameters.AddWithValue("@name", name);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = reader["SaveProgress"].ToString();
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }
        public static void DeleteSaveGame(string name)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Delete save game
                string insertNewSaveGame = @"DELETE FROM SAVED_GAME WHERE Name = @saveName";
                using (var command = new SqliteCommand(insertNewSaveGame, connection))
                {
                    command.Parameters.AddWithValue("@saveName", name);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        public static bool CheckIfSaveGameNameExists(string name)
        {
            bool result = false;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Get save game by name
                string getSaveGameByName = "SELECT * FROM SAVED_GAME WHERE Name = @name";
                using (var command = new SqliteCommand(getSaveGameByName, connection))
                {
                    command.Parameters.AddWithValue("@name", name);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = true;
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }
        public static void InsertHighScore(string user, int highScore)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Create new high score
                string insertNewHighScore = @"INSERT INTO HIGH_SCORE(User, Score)
                        VALUES (@user, @score)";
                using (var command = new SqliteCommand(insertNewHighScore, connection))
                {
                    command.Parameters.AddWithValue("@user", user);
                    command.Parameters.AddWithValue("@score", highScore);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
        public static void GetHighScores()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Get all high scores ordered by score descending
                string getAllHighScores = @"SELECT * FROM HIGH_SCORE ORDER BY Score DESC";
                using (var command = new SqliteCommand(getAllHighScores, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        var hasRows = false;
                        int count = 1;
                        while (reader.Read())
                        {
                            if (!hasRows)
                            {
                                hasRows = true;
                            }
                            Console.WriteLine($"{count}. Score: {reader["Score"], 5}, Name: {reader["User"]}");
                            count++;
                        }

                        if (!hasRows)
                        {
                            Console.WriteLine("No high scores currently available!");
                        }
                    }
                }

                connection.Close();
            }
        }
        public static int GetHighestScore()
        {
            int result = 0;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Get top high score
                string getTopHighScore = @"SELECT * FROM HIGH_SCORE ORDER BY Score DESC LIMIT 1";
                using (var command = new SqliteCommand(getTopHighScore, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = int.Parse(reader["Score"].ToString());
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }
        public static bool CheckIfHighScoreNameExists(string name)
        {
            bool result = false;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Get high score by name
                string getHighScoreByName = "SELECT * FROM HIGH_SCORE WHERE User = @name";
                using (var command = new SqliteCommand(getHighScoreByName, connection))
                {
                    command.Parameters.AddWithValue("@name", name);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = true;
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }

        #region Private members
        private static readonly string connectionString = "DataSource = 2048game.db";
        #endregion
    }
}
