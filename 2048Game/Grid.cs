namespace _2048Game
{
    public class Grid
    {
        private Tile[,] gridTiles;
        private int score;
        private int highestScore;
        private int colIndex = 0;
        private int rowIndex = 0;

        public Grid() 
        {
            gridTiles = new Tile[4,4];
            CreateInitialTiles();
            GetHighestScore();
        }

        public int GetCurrentScore() 
        {
            return score; 
        }
        public GameState GetCurrentState()
        {
            // Get current state of the grid for save
            var gameState = new GameState(gridTiles, score);
            return gameState;
        }
        public void SetLoadedGridState(GameState stateToLoad)
        {
            // Set state of grid based on loaded data
            gridTiles = stateToLoad.Grid;
            score = stateToLoad.Score;
        }
        public void PrintGrid()
        {
            for (int i = 0; i < gridTiles.GetLength(0); i++)
            {
                for(int j = 0; j < gridTiles.GetLength(1); j++)
                {
                    Console.Write($" [{gridTiles[i, j].Value}] ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine($" Score: {score}; Best: {highestScore};");
        }
        public bool RegisterMove(ConsoleKey keyPress)
        {
            // Perform grid data change based on the pressed arrow key
            switch (keyPress)
            {
                case ConsoleKey.LeftArrow:
                    LeftArrowPressed();
                    break;
                case ConsoleKey.UpArrow:
                    UpArrowPressed();
                    break;
                case ConsoleKey.RightArrow:
                    RightArrowPressed();
                    break;
                case ConsoleKey.DownArrow:
                    DownArrowPressed();
                    break;
            }

            var gameOver = CheckIfGameIsOver();

            if (!gameOver && CanAddTile())
            {
                GenerateIngameRandomTile();
            }

            return gameOver;
        }

        #region Private members
        private void CreateInitialTiles()
        {
            // Create initial tiles on game start
            InitialRandomTiles();
            for (int i = 0; i < gridTiles.GetLength(0); i++)
            {
                for(int j = 0; j < gridTiles.GetLength(1); j++)
                {
                    if (gridTiles[i, j] == null)
                    {
                        gridTiles[i, j] = new Tile();
                    }
                }
            }
        }
        private void InitialRandomTiles()
        {
            // Populate 2 random tiles with starting number upon grid creation
            var requiredTiles = 0;

            while (requiredTiles < 2)
            {
                GenerateInitialRandomTile();
                requiredTiles++;
            }
        }
        private void GenerateInitialRandomTile() 
        {
            // Generate initial random tiles upon creation of grid
            var rnd = new Random();
            var row = rnd.Next(0, gridTiles.GetLength(0));
            var col = rnd.Next(0, gridTiles.GetLength(1));
            if (gridTiles[row, col] == null)
            {
                gridTiles[row, col] = new Tile(2);
            }
            else
            {
                GenerateInitialRandomTile();
            }
        }
        private void GenerateIngameRandomTile()
        {
            // Generate a single random number for a tile after action
            var rnd = new Random();
            bool isGenerated = false;
            while (!isGenerated)
            {
                var row = rnd.Next(0, gridTiles.GetLength(0));
                var col = rnd.Next(0, gridTiles.GetLength(1));
                double randomDouble = rnd.NextDouble();

                if (gridTiles[row, col].Value == 0)
                {
                    if (randomDouble < 0.2)
                    {
                        gridTiles[row, col].Value = 4;
                    }
                    else
                    {
                        gridTiles[row, col].Value = 2;
                    }
                    isGenerated = true;
                }
            }
        }
        private bool CanAddTile()
        {
            // Check if random tile can be populated with a new value
            var canAddTile = false;
            for (int i = 0; i < gridTiles.GetLength(0); i++)
            {
                for(int j = 0; j < gridTiles.GetLength(1); j++)
                {
                    if (gridTiles[i,j].Value == 0)
                    {
                        canAddTile = true;
                        break;
                    }
                }
                if (canAddTile)
                {
                    break;
                }
            }

            return canAddTile;
        }
        private bool CheckIfGameIsOver()
        {
            // Check if no more actions can be done
            var count = 0;
            var endIndex = gridTiles.GetLength(0);

            for (int i = 0; i < gridTiles.GetLength(0); i++)
            {
                for (int j = 0; j < gridTiles.GetLength(1); j++)
                {
                    if (i < endIndex - 1 && j < endIndex - 1)
                    {
                        if (gridTiles[i, j].Value != gridTiles[i, j + 1].Value &&
                            gridTiles[i + 1, j].Value != gridTiles[i, j].Value)
                        {
                            if (gridTiles[i, j].Value != 0)
                            {
                                count++;
                            }
                        }
                    }
                    else if (i < endIndex - 1 && j < endIndex)
                    {
                        if (gridTiles[i, j].Value != gridTiles[i + 1, j].Value)
                        {
                            if (gridTiles[i, j].Value != 0)
                            {
                                count++;
                            }
                        }
                    }
                    else if (i < endIndex && j < endIndex - 1)
                    {
                        if (gridTiles[i, j].Value != gridTiles[i, j + 1].Value)
                        {
                            if (gridTiles[i, j].Value != 0)
                            {
                                count++;
                            }
                        }
                    }
                    else if (i < endIndex && j < endIndex)
                    {
                        if (gridTiles[i, j].Value != 0)
                        {
                            count++;
                        }
                    }
                }
            }

            if (count == 16)
            {
                return true;
            }

            return false;
        }
        private int GetHighestScore()
        {
            highestScore = Database.GetHighestScore();
            return highestScore;
        }
        private void DownArrowPressed()
        {
            for (int i = 0; i < gridTiles.GetLength(0); i++)
            {
                rowIndex = gridTiles.GetLength(1) - 1;
                for (int j = gridTiles.GetLength(1) - 1; j >= 0; j--)
                {
                    if (gridTiles[j, i].Value > 0)
                    {
                        if (rowIndex >= j)
                        {
                            ColValueMove(j, i, ConsoleKey.DownArrow);
                        }
                    }
                }
            }
        }
        private void RightArrowPressed()
        {
            for (int i = 0; i < gridTiles.GetLength(0); i++)
            {
                colIndex = gridTiles.GetLength(0) - 1;
                for (int j = gridTiles.GetLength(1) - 1; j >= 0; j--)
                {
                    if (gridTiles[i, j].Value > 0)
                    {
                        if (colIndex >= j)
                        {
                            RowValueMove(i, j, ConsoleKey.RightArrow);
                        }
                    }
                }
            }
        }
        private void UpArrowPressed()
        {
            for (int i = 0; i < gridTiles.GetLength(0); i++)
            {
                rowIndex = 0;
                for (int j = 0; j < gridTiles.GetLength(1); j++)
                {
                    if (gridTiles[j,i].Value > 0)
                    {
                        if (rowIndex <= j)
                        {
                            ColValueMove(j, i, ConsoleKey.UpArrow);
                        }
                    }
                }
            }
        }
        private void LeftArrowPressed()
        {
            for (int i = 0; i < gridTiles.GetLength(0); i++)
            {
                colIndex = 0;
                for (int j = 0; j < gridTiles.GetLength(1); j++)
                {
                    if (gridTiles[i, j].Value > 0)
                    {
                        if (colIndex <= j)
                        {
                            RowValueMove(i, j, ConsoleKey.LeftArrow);
                        }
                    }
                }
            }
        }
        private void ColValueMove(int row, int col, ConsoleKey keyPress)
        {
            var startTile = gridTiles[rowIndex, col];
            var tileToCompare = gridTiles[row, col];

            if (startTile.Value == 0 || (startTile.Value == tileToCompare.Value))
            {
                if ((rowIndex < row && keyPress == ConsoleKey.UpArrow) ||
                    (rowIndex > row && keyPress == ConsoleKey.DownArrow))
                {
                    var currScore = startTile.Value + tileToCompare.Value;
                    if (startTile.Value > 0)
                    {
                        score += currScore;

                        if (score > highestScore)
                        {
                            highestScore = score;
                        }
                    }

                    tileToCompare.Value = 0;
                    startTile.Value = currScore;
                }
            }
            else if(keyPress == ConsoleKey.UpArrow && rowIndex < gridTiles.GetLength(0))
            {
                rowIndex++;

                ColValueMove(row, col, keyPress);
            }
            else if (keyPress == ConsoleKey.DownArrow && rowIndex > 0)
            {
                rowIndex--;

                ColValueMove(row, col, keyPress);
            }
        }
        private void RowValueMove(int row, int col, ConsoleKey keyPress)
        {
            var startTile = gridTiles[row, colIndex];
            var tileToCompare = gridTiles[row, col];

            if (startTile.Value == 0 || (startTile.Value == tileToCompare.Value))
            {
                if ((colIndex < col && keyPress == ConsoleKey.LeftArrow) ||
                    (colIndex > col && keyPress == ConsoleKey.RightArrow))
                {
                    var currScore = startTile.Value + tileToCompare.Value;
                    if (startTile.Value > 0)
                    {
                        score += currScore;

                        if (score > highestScore)
                        {
                            highestScore = score;
                        }
                    }

                    tileToCompare.Value = 0;
                    startTile.Value = currScore;
                }
            }
            else if(keyPress == ConsoleKey.RightArrow && colIndex > 0)
            {
                colIndex--;

                RowValueMove(row, col, keyPress);
            }
            else if (keyPress == ConsoleKey.LeftArrow && colIndex < gridTiles.GetLength(0))
            {
                colIndex++;

                RowValueMove(row, col, keyPress);
            }
        }
        #endregion
    }
}
