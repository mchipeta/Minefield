namespace Minefield.Models
{
    public class BoardSetup
    {
        public GridPosition? StartPosition { get; set; }
        public List<string> Mines { get; set; } = new();
    }
}
