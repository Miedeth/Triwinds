using System;
using System.Collections.Generic;
using Triwinds.Models.Combat;

namespace Triwinds.Engine.Interfaces
{
    public interface ICombatService
    {
        Battle CreateQuickBattle(Combatant playerCharacter);

        Battle GetBattle(Guid battleId);

        Turn ProcessTurn(Guid battleId);

        bool MoveCombatant(Guid battleId, Guid playerId, int row, int column);

        bool EndCombatantTurn(Guid battleId, Guid combatantId);

        List<Guid> GetAttackableTargets(Guid battleId, Guid playerId);

        AttackResult AttackCombatant(Guid battleId, Guid attackerId, Guid defenderId);
    }
}
