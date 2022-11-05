
namespace VavaMVP.Entities
{
    internal class Matches
    {
        public Games Game { get; set; }
        public string FilePath { get; set; }
        public List<RatedPlayers> RatedPlayers { get; set; }
        public Teams Home { get; set; }
        public Teams Away { get; set; }
    }
}
