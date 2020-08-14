$(function () {
    $('#readyModal').modal('show');
});

function ProcessNextTurn() {
    var id = $('#CombatId').val();

    $.getJSON("/Combat/ProcessTurn", { battleId: id }, function (data) {
        $("#CurrentCombatant").val(JSON.stringify(data.currentCombatant));

        if (data.battleState == 0) { // Players turn
            $('#combatMenu').modal('show');
        }
    });
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