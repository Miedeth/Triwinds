using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Triwinds.Data.Interfaces;
using Triwinds.Engine.Interfaces;
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

            Turn turn = new Turn()
            {
                BattleState = battle.BattleState,
                CurrentCombatant = currentCombatantTurn
            };

            return turn;
        }

        public bool MovePlayer(Guid battleId, Guid playerId, int row, int column)
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
            }

            return validMove;
        }
    }
}
