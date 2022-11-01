
namespace VavaMVP.Entities
{
    internal class Matches
    {
        public Games Game { get; set; }
        public string FileName { get; set; }
        public Players Player { get; set; }
        public Teams Team { get; set; }
        public Positions Position { get; set; }
        public Ratings[] UnPivotRatings { get; set; }
    }
}
