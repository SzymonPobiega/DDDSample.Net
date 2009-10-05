using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using DDDSample.Domain.Location;
using DDDSample.Application;
using Microsoft.Practices.ServiceLocation;
using UI.BookingAndTracking.Facade;
using UI.BookingAndTracking.Models;

namespace UI.BookingAndTracking.Controllers
{   
   public class BookingController : Controller
   {
      private readonly BookingServiceFacade _bookingFacade;

      public BookingController(BookingServiceFacade bookingFacade)
      {
         _bookingFacade = bookingFacade;         
      }

      public ActionResult NewCargo()
      {
         AddShipingLocations();
         return View();
      }

      public ActionResult CargoDetails(string trackingId)
      {         
         return View(GetDetailsModel(trackingId));
      }

      [AcceptVerbs(HttpVerbs.Get)]
      public ActionResult ChangeDestination(string trackingId)
      {
         AddShipingLocations();
         return View(GetDetailsModel(trackingId));
      }

      [AcceptVerbs(HttpVerbs.Post)]      
      public ActionResult ChangeDestination(string trackingId, string destination)
      {
         _bookingFacade.ChangeDestination(trackingId, destination);
         return RedirectToAction("CargoDetails", new { trackingId });
      }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult NewCargo(string origin, string destination, DateTime? arrivalDeadline)
      {
         bool validationError = false;
         if (!arrivalDeadline.HasValue)
         {
            ViewData.ModelState.AddModelError("arrivalDeadline", "Arrival deadline is required and must be a valid date.");
            validationError = true;            
         }
         if (origin == destination)
         {
            ViewData.ModelState.AddModelError("destination", "Destination of a cargo must be different from its origin.");
            validationError = true;            
         }
         if (validationError)
         {
            AddShipingLocations();
            return View();
         }
         string trackingId = _bookingFacade.BookNewCargo(origin, destination, arrivalDeadline.Value);
         return RedirectToAction("CargoDetails", new { trackingId });
      }


      public void AddShipingLocations()
      {
         ViewData["ShippingLocations"] = _bookingFacade.ListShippingLocations();
      }

      public CargoRoutingDTO GetDetailsModel(string trackingId)
      {
         return _bookingFacade.LoadCargoForRouting(trackingId);
      }

      public Booking GetBookingModel()
      {
         return new Booking(_bookingFacade.ListShippingLocations());
      }
   }
}
