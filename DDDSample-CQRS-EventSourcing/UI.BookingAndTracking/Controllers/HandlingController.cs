using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDSample.Commands;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Reporting.Persistence.NHibernate;
using NServiceBus;

namespace DDDSample.UI.BookingAndTracking.Controllers
{   
   public class HandlingController : Controller
   {
      private readonly IBus _bus;
      private readonly CargoDataAccess _cargoDataAccess;
      private readonly ILocationRepository _locationRepository;      

      public HandlingController(IBus bus, CargoDataAccess cargoDataAccess, ILocationRepository locationRepository)
      {         
         _cargoDataAccess = cargoDataAccess;
         _locationRepository = locationRepository;
         _bus = bus;
      }

      [AcceptVerbs(HttpVerbs.Get)]
      public ActionResult RegisterHandlingEvent(string trackingId)
      {
         AddHandlingLocations();
         AddHandlingEventTypes();
         return View();
      }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult RegisterHandlingEvent(string trackingId, RegisterHandlingEventCommand command)
      {
         if (command.CompletionTime == DateTime.MinValue)
         {
            ModelState.AddModelError("command.CompletionTime", "Event completion date is required and must be a valid date.");
         }
         if (string.IsNullOrEmpty(trackingId))
         {
            ModelState.AddModelError("trackingId", "Tracking id must be specified.");
         }         
         if (!ModelState.IsValid)
         {
            AddHandlingLocations();
            AddHandlingEventTypes();
            return View();
         }
         Reporting.Cargo cargo = _cargoDataAccess.Find(trackingId);
         command.CargoId = cargo.Id;
         _bus.Publish(command);
         return RedirectToAction("Index", "Home");
      }
      
      #region Utility
      public void AddHandlingLocations()
      {
         ViewData["command.Location"] = 
            _locationRepository.FindAll()
            .Select(x => new SelectListItem { Text = x.Name, Value = x.UnLocode.CodeString })
            .ToList();
      }
      public void AddHandlingEventTypes()
      {
         ViewData["command.Type"] = Enum.GetNames(typeof(HandlingEventType))
            .Select(x => new SelectListItem { Text = x, Value = x});
      }      
      #endregion
   }
}
