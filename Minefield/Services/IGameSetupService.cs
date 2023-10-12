using Minefield.Models;

namespace Minefield.Services
{
    public interface IGameSetupService
    {
        BoardSetup GetBoardSetup();
    }
}
