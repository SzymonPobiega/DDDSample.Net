using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Application;

namespace UI.BookingAndTracking.Facade
{
   /// <summary>
   /// Facade for cargo booking services.
   /// </summary>
   public class BookingServiceFacade
   {
      private readonly IBookingService _bookingService;
      private readonly ILocationRepository _locationRepository;
      private readonly ICargoRepository _cargoRepository;

      public BookingServiceFacade(IBookingService bookingService, ILocationRepository locationRepository, ICargoRepository cargoRepository)
      {
         _bookingService = bookingService;
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
         return new CargoRoutingDTOAssembler().ToDTO(c);
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
   }
}