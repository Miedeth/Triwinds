using System;
using System.Collections.Generic;
using System.Text;

namespace Triwinds.Engine.Models
{
    public class Combatant
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public bool PlayerControlled { get; set; }
        public Location Location { get; set; }

        public int MaxHitPoints { get; set; }
        public int HitPoints { get; set; }
        public int ActionPoints { get; set; }
    }
}
