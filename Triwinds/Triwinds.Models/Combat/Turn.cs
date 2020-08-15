using System;

namespace Triwinds.Models.Combat
{
    public class Turn
    {
        public string MoveTo { get; set; }

        public bool Attacted { get; set; }

        public Guid AttackedCombatantId { get; set; }

        public string AttackedCombatantLocation { get; set; }

        public bool AttackHit { get; set; }

        public int Damage { get; set; }

        public BattleState BattleState { get; set; }

        public Combatant CurrentCombatant { get; set; }
    }
}
