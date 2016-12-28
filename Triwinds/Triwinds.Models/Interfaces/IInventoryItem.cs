using System;
using Triwinds.Models.Enums;
using Triwinds.Models.Inventory;

namespace Triwinds.Models.Interfaces
{
    public interface IInventoryItem
    {
        // Id of the item
        Guid InventoryId { get; set; }

        // Name of the item
        string Name { get; set; }

        // Character holding item
        int? CharacterId { get; set; }

        // What type of item is this
        InventoryTypes InventoryType { get; set; }
    }
}
