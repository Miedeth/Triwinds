using System;
using Triwinds.Models.Enums;

namespace Triwinds.Models
{
    public class DamageRange
    {
        public UInt32 MinDamage { get; set; }
        public UInt32 MaxDamage { get; set; }
        public DamageTypes DamageType { get; set; }
    }
}
