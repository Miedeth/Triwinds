using System;
using Triwinds.Models.Combat;

namespace Triwinds.Engine.Models
{
    public class AttackDecisionResult
    {
        public bool DidAttack { get; set; }

        public Guid CombatantAttacked { get; set; }

        public Location CombatantAttackedLocation { get; set; }
    }
}
