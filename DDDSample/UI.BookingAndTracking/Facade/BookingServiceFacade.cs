using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSampleNET.Application;

namespace UI.BookingAndTracking.Facade
{
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

      public string BookNewCargo(string origin, string destination, DateTime arrivalDeadline)
      {
         return _bookingService.BookNewCargo(
            new UnLocode(origin),
            new UnLocode(destination),
            arrivalDeadline)
            .IdString;
      }

      public IList<SelectListItem> ListShippingLocations()
      {
         return _locationRepository.FindAll().Select(x => new SelectListItem { Text = x.Name, Value = x.UnLocode.CodeString }).ToList();
      }

      public CargoRoutingDTO LoadCargoForRouting(string trackingId)
      {
         Cargo c = _cargoRepository.Find(new TrackingId(trackingId));
         if (c == null)
         {
            throw new ArgumentException("Cargo with specified tracking id not found.");
         }
         return new CargoRoutingDTOAssembler().ToDTO(c);
      }
   }

   public class CargoRoutingDTOAssembler
   {
      public CargoRoutingDTO ToDTO(Cargo cargo)
      {
         return new CargoRoutingDTO(
            cargo.TrackingId.IdString,
            cargo.RouteSpecification.Origin.UnLocode.CodeString,
            cargo.RouteSpecification.Destination.UnLocode.CodeString,
            cargo.RouteSpecification.ArrivalDeadline);
      }
   }

   public class CargoRoutingDTO
   {
      private readonly string _trackingId;
      private readonly string _origin;
      private readonly string _destination;
      private readonly DateTime _arrivalDeadline;

      public CargoRoutingDTO(string trackingId, string origin, string destination, DateTime arrivalDeadline)
      {
         _trackingId = trackingId;
         _arrivalDeadline = arrivalDeadline;
         _destination = destination;
         _origin = origin;
      }

      public DateTime ArrivalDeadline
      {
         get { return _arrivalDeadline; }
      }

      public string Destination
      {
         get { return _destination; }
      }

      public string Origin
      {
         get { return _origin; }
      }

      public string TrackingId
      {
         get { return _trackingId; }
      }
   }
}