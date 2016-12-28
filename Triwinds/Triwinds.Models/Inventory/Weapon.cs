using System;
using Triwinds.Models.Enums;
using Triwinds.Models.Interfaces;

namespace Triwinds.Models.Inventory
{
    public class Weapon : IInventoryItem, IIAttackable
    {
        public Guid InventoryId { get; set; }

        public string Name { get; set; }

        public int? CharacterId { get; set; }

        public InventoryTypes InventoryType { get; set; }

        public ushort Range { get; set; }

        public DamageRange Damage { get; set; }
    }
}
