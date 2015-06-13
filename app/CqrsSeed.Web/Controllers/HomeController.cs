using System;
using System.Web.Mvc;
using CqrsSeed.BC.Inventory.Commands;
using CqrsSeed.BC.Inventory.ReadModel;
using CqrsSeed.Infrastructure.Core.Bus;

namespace CqrsSeed.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private ICommandSender _bus;
        private IReadModelFacade _readModel;

        public HomeController(ICommandSender bus, IReadModelFacade readModel)
        {
            _bus = bus;
            _readModel = readModel;
        }

        public ActionResult Index()
        {
            ViewData.Model = _readModel.GetInventoryItems();

            return View();
        }

        public ActionResult Details(Guid id)
        {
            ViewData.Model = _readModel.GetInventoryItemDetails(id);
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string name)
        {
            _bus.Send(new CreateInventoryItem(Guid.NewGuid(), name));

            return RedirectToAction("Index");
        }

        public ActionResult ChangeName(Guid id)
        {
            ViewData.Model = _readModel.GetInventoryItemDetails(id);
            return View();
        }

        [HttpPost]
        public ActionResult ChangeName(Guid id, string name, int version)
        {
            var command = new RenameInventoryItem(id, name, version);
            _bus.Send(command);

            return RedirectToAction("Index");    
        }

        public ActionResult Deactivate(Guid id, int version)
        {
            _bus.Send(new DeactivateInventoryItem(id, version));
            return RedirectToAction("Index");
        }

        public ActionResult CheckIn(Guid id)
        {
            ViewData.Model = _readModel.GetInventoryItemDetails(id);
            return View();
        }

        [HttpPost]
        public ActionResult CheckIn(Guid id, int number, int version)
        {
            _bus.Send(new CheckInItemsToInventory(id, number, version));
            return RedirectToAction("Index");
        }

        public ActionResult Remove(Guid id)
        {
            ViewData.Model = _readModel.GetInventoryItemDetails(id);
            return View();
        }

        [HttpPost]
        public ActionResult Remove(Guid id, int number, int version)
        {
            _bus.Send(new RemoveItemsFromInventory(id, number, version));
            return RedirectToAction("Index");
        }
    }
}
