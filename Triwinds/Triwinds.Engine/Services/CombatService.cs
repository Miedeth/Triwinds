using System;
using System.Collections.Generic;
using System.Linq;
using Triwinds.Data.Interfaces;
using Triwinds.Engine.AI;
using Triwinds.Engine.Interfaces;
using Triwinds.Engine.Models;
using Triwinds.Models.Combat;

namespace Triwinds.Engine.Services
{
    public class CombatService : ICombatService
    {
        private IBattleRepository _battleRepository;

        public CombatService(IBattleRepository battleRepository)
        {
            _battleRepository = battleRepository;
        }

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
            _battleRepository.SaveBattle(battle);

            return battle;
        }

        public Battle GetBattle(Guid battleId)
        {
            Battle battle = _battleRepository.GetBattle(battleId);
            return battle;
        }

        public Turn ProcessTurn(Guid battleId)
        {
            Battle battle = GetBattle(battleId);
            Combatant currentCombatantTurn = battle.Combatants.Peek();

            battle.BattleState = currentCombatantTurn.PlayerControlled ? BattleState.PlayerTurn : BattleState.AiTurn;

            if (battle.Combatants.Where(c => c.PlayerControlled).Count() == 0)
            {
                battle.BattleState = BattleState.Lost;
            }

            if (battle.Combatants.Where(c => c.PlayerControlled == false).Count() == 0)
            {
                battle.BattleState = BattleState.Won;
            }

            Turn turn = new Turn()
            {
                BattleState = battle.BattleState,
                CurrentCombatant = currentCombatantTurn
            };

            if (battle.BattleState == BattleState.AiTurn)
            {
                turn = CalculateAiActions(turn, battle, currentCombatantTurn);
            }

            return turn;
        }

        public bool MoveCombatant(Guid battleId, Guid playerId, int row, int column)
        {
            Battle battle = GetBattle(battleId);
            Combatant combatant = battle.Combatants.FirstOrDefault(c => c.Id == playerId);

            if (combatant == null)
            {
                return false;
            }

            bool validMove = combatant.MovableLocations.Any(l => l.Row == row && l.Column == column);
            if (validMove)
            {
                combatant.CurrentLocation = new Location() { Row = row, Column = column };
                _battleRepository.SaveBattle(battle);
            }

            return validMove;
        }

        public bool EndCombatantTurn(Guid battleId, Guid combatantId)
        {
            Battle battle = GetBattle(battleId);
            bool success = battle.EndTurn(combatantId);

            _battleRepository.SaveBattle(battle);

            return success;
        }

        public List<Guid> GetAttackableTargets(Guid battleId, Guid playerId)
        {
            Battle battle = GetBattle(battleId);
            Combatant player = battle.Combatants.FirstOrDefault(c => c.Id == playerId);

            List<Guid> attackableTargets = new List<Guid>();
            foreach (Combatant combatant in battle.Combatants.Where(c => c.Id != player.Id && !c.PlayerControlled))
            {
                int distance = battle.GetDistance(player.CurrentLocation, combatant.CurrentLocation);

                if (distance <= 1)
                {
                    attackableTargets.Add(combatant.Id);
                }
            }

            return attackableTargets;
        }

        public AttackResult AttackCombatant(Guid battleId, Guid attackerId, Guid defenderId)
        {
            Battle battle = GetBattle(battleId);
            AttackResult attackResult = new AttackResult();
                       
            Combatant attacker = battle.Combatants.FirstOrDefault(c => c.Id == attackerId);
            Combatant defender = battle.Combatants.FirstOrDefault(c => c.Id == defenderId);

            // Currently impossible to miss.  Will calculate later when dodge mechanics added   
            // Only miss currently if out of range
            int distance = battle.GetDistance(attacker.CurrentLocation, defender.CurrentLocation);
            attackResult.AttackHit = distance == 1;

            if (attackResult.AttackHit)
            {
                attackResult.Damage = MasterRandomGenerator.RollDice(4);
                defender.HitPoints -= attackResult.Damage;

                _battleRepository.SaveBattle(battle);
            }

            return attackResult;
        }

        private Turn CalculateAiActions(Turn turn, Battle battle, Combatant currentCombatantTurn)
        {
            BasicAI ai = new BasicAI();
            Location moveToLocation = ai.MoveCombatant(battle, currentCombatantTurn.Id);
            MoveCombatant(battle.Id, currentCombatantTurn.Id, moveToLocation.Row, moveToLocation.Column);
            turn.MoveTo = string.Format("r{0}c{1}", moveToLocation.Row, moveToLocation.Column);
            AttackDecisionResult attackDecisionResult = ai.PerformAttack(battle, currentCombatantTurn.Id);

            if (attackDecisionResult.DidAttack)
            {
                turn.Attacted = true;
                AttackResult attackResult = AttackCombatant(battle.Id, currentCombatantTurn.Id, attackDecisionResult.CombatantAttacked);
                turn.AttackedCombatantId = attackDecisionResult.CombatantAttacked;
                turn.AttackedCombatantLocation = string.Format("r{0}c{1}", attackDecisionResult.CombatantAttackedLocation.Row, attackDecisionResult.CombatantAttackedLocation.Column);
                turn.AttackHit = attackResult.AttackHit;

                if (attackResult.AttackHit)
                {
                    turn.Damage = attackResult.Damage;
                }
            }
            else
            {
                turn.Attacted = false;
            }

            EndCombatantTurn(battle.Id, currentCombatantTurn.Id);

            return turn;
        }
    }
}
