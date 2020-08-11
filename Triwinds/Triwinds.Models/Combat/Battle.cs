using System;
using System.Collections.Generic;
using System.Linq;
using Triwinds.Models.Combat;

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
                List<Location> occupiedLocations = Combatants.Select(x => x.Location).ToList();
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
                combatant.Location = new Location();
                combatant.Location.Column = combatant.PlayerControlled ? 0 : 7;
                combatant.Location.Row = rng.Next(8);
                combatant.Moves = 3;
                GetMovableLocations(combatant);

                Combatants.Enqueue(combatant);
            }
        }

        private void GetMovableLocations(Combatant combatant)
        {
            // Clear existing list
            combatant.MovableLocations = new List<Location>();

            // Compute the min and max row/columns they can move to
            int startingRow = combatant.Location.Row - combatant.Moves;
            int startingColumn = combatant.Location.Column - combatant.Moves;
            int endingRow = combatant.Location.Row + combatant.Moves + 1;
            int endingColumn = combatant.Location.Column + combatant.Moves;

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
            for (int row = startingRow; row <+ endingRow; row++)
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
            combatant.MovableLocations.Add(combatant.Location);
        }
    }
}
