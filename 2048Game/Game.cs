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
                    Start();
                }
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
                        SaveScore();
                    }
                }
                else if (keyPress == ConsoleKey.Insert)
                {
                    // Save progress
                    Console.WriteLine("Select a name to save your game!");
                    Console.WriteLine("If left empty progress will not be saved!");
                    var name = Console.ReadLine();
                    if (!string.IsNullOrEmpty(name))
                    {
                        SaveState(name);
                        Console.WriteLine("State saved. Press 'Enter' to proceed with your game");
                        Console.ReadLine();
                    }
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
        private void SaveState(string saveName)
        {
            var state = grid.GetCurrentState();
            var stateToJson = JsonConvert.SerializeObject(state);
            Database.InsertSaveGame(saveName, stateToJson);
        }
        private void SelectGameToLoad(List<string> savedGames)
        {
            Console.WriteLine("Please type the name of the save game you want to load:");
            foreach (var game in savedGames)
            {
                Console.WriteLine($" - {game}");
            }
            Console.WriteLine("Or type 'back' to go back to the menu!");
            Console.WriteLine();

            var name = Console.ReadLine();
            if (savedGames.Contains(name))
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
            Console.WriteLine();
            Console.WriteLine("You can now save your score in the scoring table!");
            Console.WriteLine("Please type in your name!");
            var name = Console.ReadLine();
            Database.InsertHighScore(name, grid.GetCurrentScore());
            DisplayHighScores();
        }
        private void DisplayHighScores()
        {
            Console.Clear();
            Console.WriteLine("High Scores");
            Database.GetHighScores();
            Console.WriteLine("Press 'Enter' to continue:");
            Console.ReadLine();
            ResetGame();
        }
        #endregion
    }
}
