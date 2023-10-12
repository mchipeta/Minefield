namespace Minefield.Services
{
    public interface IConsoleService
    {
        void WriteLine(string message);
        ConsoleKey ReadKey();
    }
}
