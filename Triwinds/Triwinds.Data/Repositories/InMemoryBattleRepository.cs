using System;
using System.Collections.Generic;
using Triwinds.Models.Combat;
using Triwinds.Data.Interfaces;

namespace Triwinds.Data.Repositories
{
    public class InMemoryBattleRepository : IBattleRepository
    {
        Dictionary<Guid, Battle> Battles = new Dictionary<Guid, Battle>();

        public void DeleteBattle(Guid battleId)
        {
            if (Battles.ContainsKey(battleId))
            {
                Battles.Remove(battleId);
            }
        }

        public Battle GetBattle(Guid battleId)
        {
            if (Battles.ContainsKey(battleId))
            {
                return Battles[battleId];
            }

            return null;
        }

        public void SaveBattle(Battle battle)
        {
            if (Battles.ContainsKey(battle.Id))
            {
                Battles[battle.Id] = battle;
            }
            else
            {
                Battles.Add(battle.Id, battle);
            }
        }
    }
}
