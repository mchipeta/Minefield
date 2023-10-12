using Minefield.Enums;

namespace Minefield.Models
{
    public class GameState
    {
        public bool IsPlaying { get; set; }
        public GameResult GameResult { get; set; }
    }
}
