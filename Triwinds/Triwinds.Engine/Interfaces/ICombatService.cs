﻿using System;
using Triwinds.Models.Combat;

namespace Triwinds.Engine.Interfaces
{
    public interface ICombatService
    {
        Battle CreateQuickBattle(Combatant playerCharacter);
        Battle GetBattle(Guid battleId);

        Turn ProcessTurn(Guid battleId);
    }
}