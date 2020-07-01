using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Triwinds.Engine.Models;
using Triwinds.Engine.Services;

namespace Triwinds.UI.Controllers
{
    [Authorize]
    public class CombatController : Controller
    {
        public IActionResult CreateBattle()
        {
            Combatant playerCharacter = new Combatant();
            playerCharacter.Name = "Sample";
            playerCharacter.Image = "Swordsman";
            playerCharacter.PlayerControlled = true;
            playerCharacter.MaxHitPoints = 16;
            playerCharacter.HitPoints = 16;

            CombatService combatService = new CombatService();
            Battle battle = combatService.CreateQuickBattle(playerCharacter);

            return View("Arena", battle);
        }
    }
}
