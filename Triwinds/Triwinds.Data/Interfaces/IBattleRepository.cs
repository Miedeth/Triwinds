using System;
using Triwinds.Models.Combat;

namespace Triwinds.Data.Interfaces
{
    public interface IBattleRepository
    {
        Battle GetBattle(Guid battleId);
        void SaveBattle(Battle battle);
        void DeleteBattle(Guid battleId);
    }
}
