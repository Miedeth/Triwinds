using System;
using System.Collections.Generic;
using System.Text;
using Triwinds.Engine.Models;

namespace Triwinds.Engine.Services
{
    public class CombatService
    {
        public Battle CreateQuickBattle(Combatant playerCharacter)
        {
            Combatant monster = new Combatant
            {
                Name = "Fluffy",
                Image = "Fluffy",
                PlayerControlled = false,
                MaxHitPoints = 8,
                HitPoints = 8
            };

            List<Combatant> combatants = new List<Combatant>();
            combatants.Add(playerCharacter);
            combatants.Add(monster);

            Battle battle = new Battle(combatants);
            return battle;
        }
    }
}
