using Newtonsoft.Json;

namespace _2048Game
{
    public class Game
    {
        private Grid grid;
        public Game()
        {
            grid = new Grid();
        }

        public void Start()
        {
            Console.WriteLine("Type one of the following commands:");
            Console.WriteLine(" - new_game - start a new game;");
            Console.WriteLine(" - load_game - load a saved game;");
            Console.WriteLine(" - high_score - display recorded high scores;");
            Console.WriteLine(" - quit - quit the game;");
            Console.WriteLine();

            var gameCommand = Console.ReadLine();
            if (gameCommand.ToLower() == "new_game")
            {
                // Start a new game
                Play();
            }
            else if(gameCommand.ToLower() == "load_game")
            {
                Console.Clear();
                LoadGame();
            }
            else if (gameCommand.ToLower() == "high_score")
            {
                // Display high scores
                DisplayHighScores();
            }
            else if (gameCommand.ToLower() == "quit")
            {
                // Quit the game
                Environment.Exit(0);
            }
            else
            {
                // Execute same method if wrong command is entered
                Console.Clear();
                Start();
            }
        }

        #region Private members
        private void Play()
        {
            Console.Clear();
            Console.WriteLine("Game has started!");
            Console.WriteLine();

            Console.WriteLine("Press the 'Insert' key anytime to save progress!");
            Console.WriteLine("Press the 'End' key to end game and return to main menu!");

            RegisterInputs();
        }
        private void RegisterInputs()
        {
            var isGameOver = false;

            grid.PrintGrid();

            while (!isGameOver)
            {
                var keyPress = Console.ReadKey().Key;
                if (keyPress == ConsoleKey.UpArrow ||
                    keyPress == ConsoleKey.DownArrow ||
                    keyPress == ConsoleKey.RightArrow ||
                    keyPress == ConsoleKey.LeftArrow)
                {
                    // Register arrow key action
                    Console.Clear();
                    isGameOver = grid.RegisterMove(keyPress);
                    grid.PrintGrid();
                    if (isGameOver)
                    {
                        Console.WriteLine("Game Over!");
                        Console.WriteLine();
                        Console.WriteLine("You can now save your score in the scoring table!");
                        SaveScore();
                    }
                }
                else if (keyPress == ConsoleKey.Insert)
                {
                    // Save progress
                    Console.Clear();
                    Console.WriteLine("Select a name to save your game!");
                    Console.WriteLine("If left empty progress will not be saved!");
                    SaveGame();

                    Console.Clear();
                    grid.PrintGrid();
                }
                else if (keyPress == ConsoleKey.End)
                {
                    // End game
                    ResetGame();
                }
                else
                {
                    Console.Clear();
                    RegisterInputs();
                }
            }
        }
        private void ResetGame()
        {
            Console.Clear();
            grid = new Grid();
            Start();
        }
        private void SaveGame()
        {
            var name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name))
            {
                var existingRecord = Database.CheckIfSaveGameNameExists(name);

                Console.Clear();

                if (existingRecord)
                {
                    Console.WriteLine("A save game with the same name already exists!");
                    Console.WriteLine("Please select a new name to save your game.");
                    SaveGame();
                }
                else
                {
                    SaveState(name);
                    Console.WriteLine("State saved. Press 'Enter' to proceed with your game");
                    Console.ReadLine();
                }
            }
        }
        private void SaveState(string saveName)
        {
            var state = grid.GetCurrentState();
            var stateToJson = JsonConvert.SerializeObject(state);
            Database.InsertSaveGame(saveName, stateToJson);
        }
        private void LoadGame()
        {
            Console.Clear();
            // Load all saved game names
            var savedGames = GetSavedGames();
            if (savedGames.Any())
            {
                // Select save game to load by name
                SelectGameToLoad(savedGames);
                Play();
            }
            else
            {
                // Go back to main menu if no save games available
                Console.WriteLine("No save games found!");
                Console.WriteLine();

                Console.WriteLine("Please press 'Enter' to go back to main menu.");
                Console.ReadLine();

                ResetGame();
            }
        }
        private void SelectGameToLoad(List<string> savedGames)
        {
            Console.WriteLine("Please type the name of the save game you want to load:");
            foreach (var game in savedGames)
            {
                Console.WriteLine($" - {game}");
            }
            Console.WriteLine("Type 'delete save_name' to delete a saved game or 'back' to go back to the menu!");
            Console.WriteLine();

            var name = Console.ReadLine();
            if(name.Contains("delete "))
            {
                DeleteSaveGame(name, savedGames);
            }
            else if (savedGames.Contains(name))
            {
                Console.Clear();
                LoadState(name);
            }
            else if(name == "back")
            {
                Console.Clear();
                Start();
            }
            else
            {
                Console.Clear();
                SelectGameToLoad(savedGames);
            }
        }
        private void DeleteSaveGame(string name, List<string> savedGames)
        {
            var gameName = name.Split(' ');
            if (savedGames.Contains(gameName[1]))
            {
                Console.Clear();
                var existingRecord = Database.CheckIfSaveGameNameExists(name);
                if (existingRecord)
                {
                    Database.DeleteSaveGame(gameName[1]);
                    Console.WriteLine($"Save game {gameName[1]} was deleted!");
                    Console.WriteLine("Please press 'Enter' to go back to load game screen.");
                    Console.ReadLine();
                }
            }
            LoadGame();
        }
        private void LoadState(string name)
        {
            var state = Database.GetSaveGameByName(name);
            var jsonToState = JsonConvert.DeserializeObject<GameState>(state);
            grid.SetLoadedGridState(jsonToState);
        }
        private List<string> GetSavedGames()
        {
            return Database.GetSaveGameNames();
        }
        private void SaveScore()
        {
            Console.WriteLine("Please type in your name!");
            var name = Console.ReadLine();

            if (!string.IsNullOrEmpty(name))
            {
                var existingRecord = Database.CheckIfHighScoreNameExists(name);

                Console.Clear();

                if (existingRecord)
                {
                    Console.WriteLine("A high score with the same name already exists!");
                    SaveScore();
                }
                else
                {
                    Database.InsertHighScore(name, grid.GetCurrentScore());
                    DisplayHighScores();
                }
            }
            else
            {
                Console.Clear();
                SaveScore();
            }
        }
        private void DisplayHighScores()
        {
            Console.Clear();

            Console.WriteLine("High Scores:");
            Console.WriteLine();

            Database.GetHighScores();
            Console.WriteLine();

            Console.WriteLine("Press 'Enter' to continue:");
            Console.ReadLine();

            ResetGame();
        }
        #endregion
    }
}
