using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Application;
using DDDSample.Reporting.Persistence.NHibernate;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   /// <summary>
   /// Facade for cargo booking services.
   /// </summary>
   public class BookingServiceFacade
   {
      private readonly RouteCandidateDTOAssember _routeCandidateAssembler;
      private readonly IBookingService _bookingService;
      private readonly ILocationRepository _locationRepository;
      private readonly CargoDataAccess _cargoDataAccess;

      public BookingServiceFacade(IBookingService bookingService, ILocationRepository locationRepository, RouteCandidateDTOAssember routeCandidateAssembler, CargoDataAccess cargoDataAccess)
      {
         _bookingService = bookingService;
         _cargoDataAccess = cargoDataAccess;
         _routeCandidateAssembler = routeCandidateAssembler;
         _locationRepository = locationRepository;
      }      

      /// <summary>
      /// Loads DTO of cargo for cargo routing function.
      /// </summary>
      /// <param name="trackingId">Cargo tracking id.</param>
      /// <returns>DTO.</returns>
      public Reporting.Cargo LoadCargoForRouting(string trackingId)
      {
         Reporting.Cargo c = _cargoDataAccess.Find(trackingId);
         if (c == null)
         {
            throw new ArgumentException("Cargo with specified tracking id not found.");
         }
         return c;
      }

      /// <summary>
      /// Returns a list of all defined shipping locations in format acceptable by MVC framework 
      /// drop down list.
      /// </summary>
      /// <returns>A list of shipping locations.</returns>
      public IList<SelectListItem> ListShippingLocations()
      {
         return _locationRepository.FindAll().Select(x => new SelectListItem { Text = x.Name, Value = x.UnLocode.CodeString }).ToList();
      }

      /// <summary>
      /// Returns a complete list of cargos stored in the system.
      /// </summary>
      /// <returns>A collection of cargo DTOs.</returns>
      public IList<Reporting.Cargo> ListAllCargos()
      {
         return _cargoDataAccess.FindAll();
      }     

      /// <summary>
      /// Changes destination of an existing cargo.
      /// </summary>
      /// <param name="trackingId">Cargo tracking id.</param>
      /// <param name="destination">New destination UnLocode in string format.</param>
      public void ChangeDestination(string trackingId, string destination)
      {
         Guid cargoId = _cargoDataAccess.Find(trackingId).Id;
         _bookingService.ChangeDestination(cargoId, new UnLocode(destination));
      }

      /// <summary>
      /// Books new cargo for specified origin, destination and arrival deadline.
      /// </summary>
      /// <param name="origin">Origin of a cargo in UnLocode format.</param>
      /// <param name="destination">Destination of a cargo in UnLocode format.</param>
      /// <param name="arrivalDeadline">Arrival deadline.</param>
      public void BookNewCargo(string origin, string destination, DateTime arrivalDeadline)
      {
         TrackingId trackingId;
         _bookingService.BookNewCargo(
            new UnLocode(origin),
            new UnLocode(destination),
            arrivalDeadline, out trackingId);          
      }      

      /// <summary>
      /// Binds cargo to selected delivery route.
      /// </summary>
      /// <param name="trackingId">Cargo tracking id.</param>
      /// <param name="route">Route definition.</param>
      public void AssignCargoToRoute(String trackingId, RouteCandidateDTO route)
      {
         Guid cargoId = _cargoDataAccess.Find(trackingId).Id;
         _bookingService.AssignCargoToRoute(cargoId, _routeCandidateAssembler.FromDTO(route));
      }
   }
}