namespace _2048Game
{
    public class GameState
    {
        public GameState(Tile[,] grid, int score)
        {
            Grid = grid;
            Score = score;
        }
        public Tile[,] Grid { get; set; }
        public int Score { get; set; }

    }
}
