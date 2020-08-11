using System;
using System.Collections.Generic;
using System.Text;

namespace Triwinds.Models.Combat
{
    public class Turn
    {
        public BattleState BattleState { get; set; }

        public Combatant CurrentCombatant { get; set; }
    }
}
