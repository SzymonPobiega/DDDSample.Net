using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using DDDSample.Domain.Location;
using DDDSampleNET.Application;
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
         return View(GetBookingModel());
      }

      public ActionResult CargoDetails(string trackingId)
      {         
         return View(GetDetailsModel(trackingId));
      }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult ChangeDestination(string trackingId)
      {
         throw new InvalidOperationException();
      }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult NewCargo(string origin, string destination, DateTime arrivalDeadline)
      {
         string trackingId = _bookingFacade.BookNewCargo(origin, destination, arrivalDeadline);
         return RedirectToAction("CargoDetails", new { trackingId });
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
