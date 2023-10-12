namespace Minefield.Factories
{
    public class RandomFactory : IRandomFactory
    {
        public Random Create()
        {
            return new Random((int)DateTime.Now.Ticks);
        }
    }
}
