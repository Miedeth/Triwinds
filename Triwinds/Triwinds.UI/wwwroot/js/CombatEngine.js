$(function () {
    $('#readyModal').modal('show');
});

function ProcessNextTurn() {
    var id = $('#CombatId').val();

    $.getJSON("/Combat/ProcessTurn", { battleId: id }, function (data) {
        $("#CurrentCombatant").val(JSON.stringify(data.currentCombatant));

        if (data.battleState == 0) { // Player's turn
            $('#combatMenu').modal('show');
        }
        if (data.battleState == 1) { // AI's turn
            PlayBackAITurnActions(data);
        }
        if (data.battleState == 2) { // Win
            $("#EndBattleMessage").text("Congragulations You Win!");
            $('#endBattleModal').modal('show');
        }
        if (data.battleState == 3) { // Loss
            $("#EndBattleMessage").text("You are dead.  Better luck next time.");
            $('#endBattleModal').modal('show');
        }
    });
}

function PlayBackAITurnActions(data) {
    $('#' + data.currentCombatant.id).appendTo($('#' + data.moveTo)).animate({}, 1000);

    if (data.attacted == true) {
        var currentHp = $("#" + data.attackedCombatantId + "HP").text();
        var newHp = currentHp - data.damage;
        $("#" + data.attackedCombatantId + "HP").text(newHp);

        $("#" + data.attackedCombatantId).fadeOut(100).fadeIn(100).fadeOut(100).fadeIn(100).fadeOut(100).fadeIn(100);

        if (newHp <= 0) {
            $("#" + data.attackedCombatantId).fadeOut(50);
        }
    }

    setTimeout(function () {
        ProcessNextTurn();
    }, 750);
}

function ShowMoveableSquares() {
    var currentCombatant = JSON.parse($("#CurrentCombatant").val());

    $.each(currentCombatant.movableLocationIds, function (id) {

        var squareId = currentCombatant.movableLocationIds[id];
        $('#' + squareId).addClass("moveable");
        $('#' + squareId).click(function () { MovePlayer(squareId); });
    });
}

function MovePlayer(squareId) {
    var id = $('#CombatId').val();
    var currentCombatant = JSON.parse($("#CurrentCombatant").val());

    // Check to see if move is valid
    $.post("/Combat/MovePlayer", { battleId: id, playerId: currentCombatant.id, squareId: squareId }, function (data) {
        // If valid perform the move        
        if (data == true) {
            $('#' + currentCombatant.id).appendTo($('#' + squareId)).animate({ }, 1000);
        }
    });

    // Remove movable class and click events
    $.each(currentCombatant.movableLocationIds, function (id) {
        console.log(currentCombatant.movableLocationIds[id]);

        var squareId = currentCombatant.movableLocationIds[id];
        $('#' + squareId).removeClass("moveable");
        $('#' + squareId).prop("onclick", null).off("click");
    });

    $('#combatMenu').modal('show');
}

function Attack() {
    var id = $('#CombatId').val();
    var currentCombatant = JSON.parse($("#CurrentCombatant").val());

    $.getJSON("/Combat/GetAttackableTargets", { battleId: id, playerId: currentCombatant.id }, function (data) {
        if (data.length == 0) {
            alert("No valid targets");

            setTimeout(function () {
                ProcessNextTurn();
            }, 200);
        }
        else if (data.length == 1) { // If there is only one possible target make the attack
            AttackCombatant(id, currentCombatant.id, data[0]);
        }
    });
}

function EndPlayerTurn() {
    var id = $('#CombatId').val();
    var currentCombatant = JSON.parse($("#CurrentCombatant").val());

    $.post("/Combat/EndPlayerTurn", { battleId: id, playerId: currentCombatant.id }, function (data) {
        ProcessNextTurn();
    });
}

function AttackCombatant(battleId, attackerId, defenderId) {
    $.post("/Combat/AttackCombatant", { battleId: battleId, attackerId: attackerId, defenderId: defenderId }, function (data) {

        if (data.attackHit == true) {
            var currentHp = $("#" + defenderId + "HP").text();
            var newHp = currentHp - data.damage;
            $("#" + defenderId + "HP").text(newHp);

            $("#" + defenderId).fadeOut(100).fadeIn(100).fadeOut(100).fadeIn(100).fadeOut(100).fadeIn(100);

            if (newHp <= 0) {
                $("#" + defenderId).fadeOut(50);
            }
        }

        setTimeout(function () {
            // Performing an action ends the players turn
            EndPlayerTurn();
        }, 750);        
    });
}