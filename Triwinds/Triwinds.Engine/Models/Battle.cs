using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text;

namespace Triwinds.Engine.Models
{
    public class Battle
    {
        public List<Combatant> Combatants { get; set; }

        public Battle(List<Combatant> combatants)
        {
            Combatants = new List<Combatant>();

            foreach (Combatant combatant in combatants)
            {
                combatant.ActionPoints = MasterRandomGenerator.RollDice(10) + 20;

                combatant.Location = new Location();
                combatant.Location.Column = combatant.PlayerControlled ? 0 : 7;
                combatant.Location.Row = MasterRandomGenerator.RollDice(8) - 1;

                Combatants.Add(combatant);
            }
        }
    }
}
