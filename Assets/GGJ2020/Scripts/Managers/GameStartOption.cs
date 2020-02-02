namespace GGJ2020.Managers
{
    public sealed class GameStartOption
    {
        public int PlayerCount { get; }

        public GameStartOption(int playerCount)
        {
            PlayerCount = playerCount;
        }
    }
}
