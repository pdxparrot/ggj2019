namespace pdxpartyparrot.Game.State
{
    public class GameStateLoadStatus
    {
        public float LoadPercent { get; }

        public string Status { get; }

        public GameStateLoadStatus(float loadPercent, string status)
        {
            LoadPercent = loadPercent;
            Status = status;
        }
    }
}
