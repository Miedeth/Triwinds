using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Triwinds.Models.Combat;
using Triwinds.Engine.Interfaces;

namespace Triwinds.UI.Controllers
{
    [Authorize]
    public class CombatController : Controller
    {
        private ICombatService _combatService;

        public CombatController(ICombatService combatService)
        {
            _combatService = combatService;
        }

        [HttpGet]
        public IActionResult CreateBattle()
        {
            Combatant playerCharacter = new Combatant();
            playerCharacter.Name = "Sample";
            playerCharacter.Image = "Swordsman";
            playerCharacter.PlayerControlled = true;
            playerCharacter.MaxHitPoints = 16;
            playerCharacter.HitPoints = 16;

            Battle battle = _combatService.CreateQuickBattle(playerCharacter);

            return View("Arena", battle);
        }

        [HttpGet]
        public JsonResult ProcessTurn(Guid battleId)
        {
            Turn turn = _combatService.ProcessTurn(battleId);

            return Json(turn);
        }

        [HttpPost]
        public JsonResult MovePlayer(Guid battleId, Guid playerId, string squareId)
        {
            // Get the row and colum from the r#c# format
            string[] ids = squareId.Replace("r", string.Empty).Split("c");

            int row, column;
            bool rowIsInt = int.TryParse(ids[0], out row);
            bool columnIsInt = int.TryParse(ids[1], out column);

            if (rowIsInt && columnIsInt)
            {
                bool validMove = _combatService.MovePlayer(battleId, playerId, row, column);
                return Json(validMove);
            }

            return Json(false);
        }
    }
}
