
namespace VavaMVP.Entities
{
    internal class Teams
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public bool IsWinner { get; set; }

        public Teams Shallowcopy()
        {
            return (Teams)this.MemberwiseClone();
        }
    }
}
