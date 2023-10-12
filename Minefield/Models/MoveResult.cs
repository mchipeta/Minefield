namespace Minefield.Models
{
    public class MoveResult
    {
        public bool IsSuccessful { get; set; }
        public GridPosition? NewPosition { get; set; }
    }
}
