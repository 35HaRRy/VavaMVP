
namespace VavaMVP.Entities
{
    internal class RatedPlayers
    {
        public Teams Team { get; set; }
        public Players Player { get; set; }
        public Positions Position { get; set; }
        public List<Ratings> UnPivotRatings { get; set; }
    }
}
