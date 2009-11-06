using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Application;

namespace DDDSample.UI.BookingAndTracking.Facade
{
   /// <summary>
   /// Facade for cargo booking services.
   /// </summary>
   public class BookingServiceFacade
   {
      private readonly RouteCandidateDTOAssember _routeCandidateAssembler;
      private readonly CargoRoutingDTOAssembler _cargoRoutingAssembler;
      private readonly IBookingService _bookingService;
      private readonly ILocationRepository _locationRepository;
      private readonly ICargoRepository _cargoRepository;

      public BookingServiceFacade(IBookingService bookingService, ILocationRepository locationRepository, ICargoRepository cargoRepository, RouteCandidateDTOAssember routeCandidateAssembler, CargoRoutingDTOAssembler cargoRoutingAssembler)
      {
         _bookingService = bookingService;
         _cargoRoutingAssembler = cargoRoutingAssembler;
         _routeCandidateAssembler = routeCandidateAssembler;
         _cargoRepository = cargoRepository;
         _locationRepository = locationRepository;
      }

      /// <summary>
      /// Books new cargo for specified origin, destination and arrival deadline.
      /// </summary>
      /// <param name="origin">Origin of a cargo in UnLocode format.</param>
      /// <param name="destination">Destination of a cargo in UnLocode format.</param>
      /// <param name="arrivalDeadline">Arrival deadline.</param>
      /// <returns>Cargo tracking id.</returns>
      public string BookNewCargo(string origin, string destination, DateTime arrivalDeadline)
      {
         return _bookingService.BookNewCargo(
            new UnLocode(origin),
            new UnLocode(destination),
            arrivalDeadline)
            .IdString;
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
      /// Loads DTO of cargo for cargo routing function.
      /// </summary>
      /// <param name="trackingId">Cargo tracking id.</param>
      /// <returns>DTO.</returns>
      public CargoRoutingDTO LoadCargoForRouting(string trackingId)
      {
         Cargo c = _cargoRepository.Find(new TrackingId(trackingId));
         if (c == null)
         {
            throw new ArgumentException("Cargo with specified tracking id not found.");
         }
         return _cargoRoutingAssembler.ToDTO(c);
      }

      /// <summary>
      /// Returns a complete list of cargos stored in the system.
      /// </summary>
      /// <returns>A collection of cargo DTOs.</returns>
      public IList<CargoRoutingDTO> ListAllCargos()
      {
         return _cargoRepository.FindAll().Select(x => _cargoRoutingAssembler.ToDTO(x)).ToList();
      }

      /// <summary>
      /// Changes destination of an existing cargo.
      /// </summary>
      /// <param name="trackingId">Cargo tracking id.</param>
      /// <param name="destination">New destination UnLocode in string format.</param>
      public void ChangeDestination(string trackingId, string destination)
      {
         _bookingService.ChangeDestination(new TrackingId(trackingId), new UnLocode(destination));
      }

      /// <summary>
      /// Fetches all possible routes for delivering cargo with provided tracking id.
      /// </summary>
      /// <param name="trackingId">Cargo tracking id.</param>
      /// <returns>Possible delivery routes</returns>
      public IList<RouteCandidateDTO> RequestPossibleRoutesForCargo(String trackingId)
      {         
         return _bookingService.RequestPossibleRoutesForCargo(new TrackingId(trackingId))
            .Select(x => _routeCandidateAssembler.ToDTO(x)).ToList();
      }

      /// <summary>
      /// Binds cargo to selected delivery route.
      /// </summary>
      /// <param name="trackingId">Cargo tracking id.</param>
      /// <param name="route">Route definition.</param>
      public void AssignCargoToRoute(String trackingId, RouteCandidateDTO route)
      {
         _bookingService.AssignCargoToRoute(new TrackingId(trackingId), _routeCandidateAssembler.FromDTO(route));
      }
   }
}