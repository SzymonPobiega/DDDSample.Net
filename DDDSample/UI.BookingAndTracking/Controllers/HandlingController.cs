using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDSample.DomainModel.Operations.Handling;
using DDDSample.UI.BookingAndTracking.Facade;

namespace DDDSample.UI.BookingAndTracking.Controllers
{   
   public class HandlingController : Controller
   {
      private readonly HandlingEventServiceFacade _handlingEventFacade;

      public HandlingController(HandlingEventServiceFacade handlingEventFacade)
      {
         _handlingEventFacade = handlingEventFacade;
      }

      [AcceptVerbs(HttpVerbs.Get)]
      public ActionResult RegisterHandlingEvent(string trackingId)
      {
         AddHandlingLocations();
         AddHandlingEventTypes();
         return View();
      }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult RegisterHandlingEvent(string trackingId, DateTime? completionTime, string location, HandlingEventType type)
      {
         bool validationError = false;
         if (!completionTime.HasValue)
         {
            ViewData.ModelState.AddModelError("completionTime", "Event completion date is required and must be a valid date.");
            validationError = true;            
         }         
         if (string.IsNullOrEmpty(trackingId))
         {
            ViewData.ModelState.AddModelError("trackingId", "Tracking id must be specified.");
            validationError = true;
         }         
         if (validationError)
         {
            AddHandlingLocations();
            AddHandlingEventTypes();
            return View();
         }
         _handlingEventFacade.RegisterHandlingEvent(completionTime.Value, trackingId, location, type );
         return RedirectToAction("Track", "Tracking", new {trackingId});
      }
      
      #region Utility
      public void AddHandlingLocations()
      {
         ViewData["location"] = _handlingEventFacade.ListHandlingLocations();
      }
      public void AddHandlingEventTypes()
      {
         ViewData["type"] = Enum.GetNames(typeof(HandlingEventType))
            .Select(x => new SelectListItem { Text = x, Value = x});
      }      
      #endregion
   }
}
