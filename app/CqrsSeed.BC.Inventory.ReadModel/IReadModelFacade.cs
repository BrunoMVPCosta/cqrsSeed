using System;
using System.Collections.Generic;
using CqrsSeed.BC.Inventory.ReadModel.Dtos;

namespace CqrsSeed.BC.Inventory.ReadModel
{
    public interface IReadModelFacade
    {
        IEnumerable<InventoryItemListDto> GetInventoryItems();
        InventoryItemDetailsDto GetInventoryItemDetails(Guid id);
    }
}