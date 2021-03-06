﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Triwinds.Models.Combat
{
    public class Battle
    {
        public Guid Id { get; set; }
        public Queue<Combatant> Combatants { get; set; }

        public int Rows { get; set; }
        public int Columns { get; set; }

        public BattleState BattleState { get; set; }

        private List<Location> OccupiedLocations
        {
            get
            {
                List<Location> occupiedLocations = Combatants.Select(x => x.CurrentLocation).ToList();
                return occupiedLocations;
            }
        }

        public Battle(List<Combatant> combatants)
        {
            Id = Guid.NewGuid();
            Random rng = new Random();
            Combatants = new Queue<Combatant>();
            Rows = 8;
            Columns = 8;

            foreach (Combatant combatant in combatants)
            {
                combatant.Id = Guid.NewGuid();
                combatant.TurnStartLocation = new Location();
                combatant.TurnStartLocation.Column = combatant.PlayerControlled ? 0 : 7;
                combatant.TurnStartLocation.Row = rng.Next(8);
                combatant.CurrentLocation = combatant.TurnStartLocation;
                combatant.Moves = 3;
                GetMovableLocations(combatant);

                Combatants.Enqueue(combatant);
            }
        }

        public bool EndTurn(Guid playerId)
        {
            Combatant currentCombatant = Combatants.Peek();

            // Make sure the right player is ending their turn
            if (currentCombatant.Id != playerId)
            {
                return false;
            }

            // Remove the combatant from the queue.
            Combatant combatant = Combatants.Dequeue();

            // Prepare their next turn
            combatant.TurnStartLocation = combatant.CurrentLocation;
            GetMovableLocations(combatant);

            // Add them to the end of the turn order
            Combatants.Enqueue(combatant);

            // Remove any dead combatants
            Combatants = new Queue<Combatant>(Combatants.Where(c => c.HitPoints > 0));

            return true;
        }

        public int GetDistance(Location location1, Location location2)
        {
            int rowDifference = Math.Abs(location1.Row - location2.Row);
            int columnDifference = Math.Abs(location1.Column - location2.Column);

            return rowDifference > columnDifference ? rowDifference : columnDifference;
        }

        private void GetMovableLocations(Combatant combatant)
        {
            // Clear existing list
            combatant.MovableLocations = new List<Location>();

            // Compute the min and max row/columns they can move to
            int startingRow = combatant.TurnStartLocation.Row - combatant.Moves;
            int startingColumn = combatant.TurnStartLocation.Column - combatant.Moves;
            int endingRow = combatant.TurnStartLocation.Row + combatant.Moves;
            int endingColumn = combatant.TurnStartLocation.Column + combatant.Moves;

            if (startingRow < 0)
            {
                startingRow = 0;
            }

            if (startingColumn < 0)
            {
                startingColumn = 0;
            }

            if (endingRow > Rows - 1)
            {
                endingRow = Rows - 1;
            }

            if (endingColumn > Columns - 1)
            {
                endingColumn = Columns - 1;
            }

            // Make a list of all those that aren't occupied
            for (int row = startingRow; row <= endingRow; row++)
            {
                for (int column = startingColumn; column <= endingColumn; column++)
                {
                    Location movingCandidate = new Location() { Row = row, Column = column };
                    if (!OccupiedLocations.Contains(movingCandidate))
                    {
                        combatant.MovableLocations.Add(movingCandidate);
                    }
                }
            }

            // Add the combatants current location to the list of places they can move
            combatant.MovableLocations.Add(combatant.CurrentLocation);
        }
    }
}
