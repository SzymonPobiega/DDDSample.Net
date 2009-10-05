using System;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using NHibernate;

namespace DDDSample.Application.Implemetation
{
   /// <summary>
   /// Implementation of <see cref="IBookingService"/>.
   /// </summary>
   public class BookingService : IBookingService
   {
      private readonly ILocationRepository _locationRepository;
      private readonly ICargoRepository _cargoRepository;

      public BookingService(ILocationRepository locationRepository, ICargoRepository cargoRepository)
      {
         _locationRepository = locationRepository;
         _cargoRepository = cargoRepository;
      }

      public TrackingId BookNewCargo(UnLocode originUnLocode, UnLocode destinationUnLocode, DateTime arrivalDeadline)
      {
         Location origin = _locationRepository.Find(originUnLocode);
         Location destination = _locationRepository.Find(destinationUnLocode);

         RouteSpecification routeSpecification = new RouteSpecification(origin, destination, arrivalDeadline);
         TrackingId trackingId = _cargoRepository.NextTrackingId();
         Cargo cargo = new Cargo(trackingId, routeSpecification);

         _cargoRepository.Store(cargo);
         return trackingId;
      }

      public IEnumerable<Itinerary> RequestPossibleRoutesForCargo(TrackingId trackingId)
      {
         throw new NotImplementedException();
      }

      public void AssignCargoToRoute(Itinerary itinerary, TrackingId trackingId)
      {
         throw new NotImplementedException();
      }

      public void ChangeDestination(TrackingId trackingId, UnLocode destinationUnLocode)
      {
         Cargo cargo = _cargoRepository.Find(trackingId);
         Location destination = _locationRepository.Find(destinationUnLocode);

         RouteSpecification routeSpecification = new RouteSpecification(cargo.RouteSpecification.Origin, destination, cargo.RouteSpecification.ArrivalDeadline);
         cargo.SpecifyNewRoute(routeSpecification);
      }
   }
}