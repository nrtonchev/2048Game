namespace _2048Game
{
    public class Tile
    {
        public Tile()
        {
            Value = 0;
        }

        public Tile(int value)
        {
            Value = value;    
        }

        public int Value { get; set; }
    }
}
