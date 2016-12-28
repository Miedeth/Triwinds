namespace Triwinds.Models.Interfaces
{
    public interface IBlockable
    {
        int BlockChance { get; set; }
        bool CanBlockArrows { get; set; }
        bool CanBlockSpells { get; set; }
    }
}
