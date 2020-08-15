using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Triwinds.Engine.Models;
using Triwinds.Models.Combat;

namespace Triwinds.Engine.AI
{
    public class BasicAI
    {
        public Location MoveCombatant(Battle battle, Guid movingCombatantId)
        {
            // Find the monster and it's target
            Combatant ai = battle.Combatants.FirstOrDefault(c => c.Id == movingCombatantId);
            Combatant player = battle.Combatants.FirstOrDefault(c => c.PlayerControlled == true);
            Location move = ai.CurrentLocation;

            int distance = battle.GetDistance(ai.CurrentLocation, player.CurrentLocation);

            // If already in attack range no need to move
            if (distance <= 1)
            {
                return ai.CurrentLocation;
            }

            // Move as close to the player as possible
            for (int i = 0; i < ai.Moves; i++)
            {
                distance = battle.GetDistance(move, player.CurrentLocation);
                if (distance <= 1)
                {
                    continue;
                }

                if (player.CurrentLocation.Row > move.Row)
                {
                    move.Row++;
                }

                if (player.CurrentLocation.Row < move.Row)
                {
                    move.Row--;
                }

                distance = battle.GetDistance(move, player.CurrentLocation);
                if (distance <= 1)
                {
                    continue;
                }

                if (player.CurrentLocation.Column > move.Column)
                {
                    move.Column++;
                }

                if (player.CurrentLocation.Column < move.Column)
                {
                    move.Column--;
                }
            }

            return move;
        }

        public AttackDecisionResult PerformAttack(Battle battle, Guid combatantId)
        {
            Combatant ai = battle.Combatants.FirstOrDefault(c => c.Id == combatantId);
            Combatant player = battle.Combatants.FirstOrDefault(c => c.PlayerControlled == true);

            AttackDecisionResult attackResult = new AttackDecisionResult();

            int distance = battle.GetDistance(ai.CurrentLocation, player.CurrentLocation);

            if (distance > 1)
            {
                attackResult.DidAttack = false;
                return attackResult;
            }

            attackResult.DidAttack = true;
            attackResult.CombatantAttacked = player.Id;
            attackResult.CombatantAttackedLocation = player.CurrentLocation;

            return attackResult;
        }
    }
}
