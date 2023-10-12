namespace Minefield.Services
{
    public class ConsoleService : IConsoleService
    {
        public ConsoleKey ReadKey()
        {
            var info = Console.ReadKey();
            return info.Key;
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
