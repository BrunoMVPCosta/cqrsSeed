using System;
using System.Collections.Generic;
using System.Web.Http;
using CqrsSeed.BC.Inventory.Commands;
using CqrsSeed.BC.Inventory.ReadModel;
using CqrsSeed.BC.Inventory.ReadModel.Dtos;
using CqrsSeed.Infrastructure.Core.Bus;

namespace CqrsSeed.Web.Api.Controllers
{
    public class InventoryController : ApiController
    {
        private readonly ICommandSender _bus;
        private readonly IReadModelFacade _readModel;

        public InventoryController(ICommandSender bus, IReadModelFacade readModel)
        {
            _bus = bus;
            _readModel = readModel;
        }

        public IEnumerable<InventoryItemListDto> Get()
        {
            return _readModel.GetInventoryItems();
        }

        public InventoryItemDetailsDto Get(Guid id)
        {
            return _readModel.GetInventoryItemDetails(id);
        }

        public void Post(InventoryItemDetailsDto request)
        {
            _bus.Send(new CreateInventoryItem(Guid.NewGuid(), request.Name));
        }

        public void Put(Guid id, InventoryItemDetailsDto request)
        {
            var command = new RenameInventoryItem(request.Id, request.Name, request.Version);
            _bus.Send(command);
        }

        public void Delete(Guid id, int version)
        {
            var command = new DeactivateInventoryItem(id, version);
            _bus.Send(command);
        }

        [HttpPost]
        public void CheckIn(Guid id, int version, [FromBody]int count)
        {
            _bus.Send(new CheckInItemsToInventory(id, count, version));
        }
          
        [HttpPost]
        public void RemoveItems(Guid id, int version, [FromBody]int count)
        {
            _bus.Send(new RemoveItemsFromInventory(id, count, version));
        }
    }
}
