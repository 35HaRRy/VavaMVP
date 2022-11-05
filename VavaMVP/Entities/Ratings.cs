
namespace VavaMVP.Entities
{
    internal class Ratings
    {
        public Games Game { get; set; }
        public string Name { get; set; }
        public Positions Position { get; set; }
        public int Ratio { get; set; }
        public bool IsInitial { get; set; }
        public bool IsForTeam { get; set; }

        public Ratings Shallowcopy()
        {
            return (Ratings)this.MemberwiseClone();
        }
    }
}
