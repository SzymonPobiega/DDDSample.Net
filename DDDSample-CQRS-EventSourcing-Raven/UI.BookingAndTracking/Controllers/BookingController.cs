using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDSample.CommandHandlers;
using DDDSample.Commands;
using DDDSample.UI.BookingAndTracking.Facade;
using DDDSample.UI.BookingAndTracking.Models;

namespace DDDSample.UI.BookingAndTracking.Controllers
{   
   public class BookingController : Controller
   {
      private readonly RoutingFacade _routingFacade;
      private readonly IBus _bus;
      private readonly BookingServiceFacade _bookingFacade;

      public BookingController(BookingServiceFacade bookingFacade, RoutingFacade routingFacade, IBus bus)
      {
         _bookingFacade = bookingFacade;
         _routingFacade = routingFacade;
         _bus = bus;
      }

      [AcceptVerbs(HttpVerbs.Get)]
      public ActionResult ListCargos()
      {
         ViewData["cargos"] = _bookingFacade.ListAllCargos();
         return View();
      }

      [AcceptVerbs(HttpVerbs.Get)]
      public ActionResult NewCargo()
      {
         AddShipingLocations();
         return View();
      }

      [AcceptVerbs(HttpVerbs.Get)]
      public ActionResult CargoDetails(string trackingId)
      {         
         return View(GetDetailsModel(trackingId));
      }

      [AcceptVerbs(HttpVerbs.Get)]
      public ActionResult ChangeDestination(string trackingId)
      {
         Reporting.Cargo cargo = _bookingFacade.LoadCargoForRouting(trackingId);         

         IList<SelectListItem> shippingLocations = _bookingFacade.ListShippingLocations();
         ViewData["destination"] = shippingLocations.Where(x => x.Value != cargo.Origin).ToList();

         return View(GetDetailsModel(trackingId));
      }

      [AcceptVerbs(HttpVerbs.Post)]      
      public ActionResult ChangeDestination(string trackingId, string destination)
      {
         Reporting.Cargo cargo = _bookingFacade.LoadCargoForRouting(trackingId);
         _bus.Send(new ChangeDestinationCommand
                      {
                         CargoId = cargo.AggregateId,
                         NewDestination = destination
                      });
         return RedirectToDetails(trackingId);
      }      

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult NewCargo(BookNewCargoCommand command)
      {
         if (command.ArrivalDeadline == DateTime.MinValue)
         {
            ModelState.AddModelError("ArrivalDeadline", "Arrival deadline is required and must be a valid date.");            
         }
         if (command.Origin == command.Destination)
         {
            ModelState.AddModelError("Destination", "Destination of a cargo must be different from its origin.");            
         }
         if (!ModelState.IsValid)
         {
            AddShipingLocations();
            return View();
         }
         _bus.Send(command);
         return RedirectToAction("ListCargos");
      }

      [AcceptVerbs(HttpVerbs.Get)]
      public ActionResult AssignToRoute(string trackingId)
      {         
         return View(GetAssignToRouteModel(trackingId));
      }

      [AcceptVerbs(HttpVerbs.Post)]
      public ActionResult AssignToRoute(string trackingId, List<LegDTO> legs)
      {
         Reporting.Cargo cargo = _bookingFacade.LoadCargoForRouting(trackingId);
         _bus.Send(new AssignCargoToRouteCommand
                      {
                         CargoId = cargo.AggregateId,
                         Legs = legs
                      });
         return RedirectToDetails(trackingId);
      }
      
      #region Utility
      public void AddShipingLocations()
      {
         IList<SelectListItem> shippingLocations = _bookingFacade.ListShippingLocations();
         ViewData["origin"] = shippingLocations;
         ViewData["destination"] = shippingLocations;
      }      

      public ActionResult RedirectToDetails(string trackingId)
      {
         return RedirectToAction("CargoDetails", new { trackingId });
      }

      public AssignToRouteModel GetAssignToRouteModel(string trackingId)
      {
         Reporting.Cargo cargo = _bookingFacade.LoadCargoForRouting(trackingId);
         return new AssignToRouteModel(
            cargo,
            _routingFacade.FetchRoutesForSpecification(cargo.Origin, cargo.Destination, cargo.ArrivalDeadline)
            );
      }

      public Reporting.Cargo GetDetailsModel(string trackingId)
      {
         return _bookingFacade.LoadCargoForRouting(trackingId);
      }      
      #endregion
   }
}
