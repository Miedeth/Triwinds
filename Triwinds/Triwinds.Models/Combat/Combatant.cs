using System;
using System.Collections.Generic;

namespace Triwinds.Models.Combat
{
    public class Combatant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool PlayerControlled { get; set; }
        public Location Location { get; set; }

        public int MaxHitPoints { get; set; }
        public int HitPoints { get; set; }

        public int Moves { get; set; }

        public List<Location> MovableLocations { get; set; }

        public List<string> MovableLocationIds 
        {
            get
            {
                List<string> ids = new List<string>();
                foreach (Location location in MovableLocations)
                {
                    ids.Add(string.Format("r{0}c{1}", location.Row, location.Column));                    
                }

                return ids;
            }
        }
    }
}
