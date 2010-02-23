using System;
using System.Collections.Generic;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;

namespace DDDSample.Application.Implemetation
{
   /// <summary>
   /// Implementation of <see cref="IBookingService"/>.
   /// </summary>
   public class BookingService : IBookingService
   {
      private readonly ILocationRepository _locationRepository;
      private readonly ICargoRepository _cargoRepository;
      private readonly IRoutingService _routingService;

      public BookingService(ILocationRepository locationRepository, ICargoRepository cargoRepository, IRoutingService routingService)
      {
         _locationRepository = locationRepository;
         _cargoRepository = cargoRepository;
         _routingService = routingService;
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

      public IList<Itinerary> RequestPossibleRoutesForCargo(TrackingId trackingId)
      {
         Cargo cargo = _cargoRepository.Find(trackingId);

         return cargo.RequestPossibleRoutes(_routingService);
      }

      public void AssignCargoToRoute(TrackingId trackingId, Itinerary itinerary)
      {
         Cargo cargo = _cargoRepository.Find(trackingId);
         cargo.AssignToRoute(itinerary);
      }

      public void ChangeDestination(TrackingId trackingId, UnLocode destinationUnLocode)
      {         
         Location destination = _locationRepository.Find(destinationUnLocode);
         
         Cargo cargo = _cargoRepository.Find(trackingId);
         cargo.SpecifyNewRoute(destination);
      }
   }
}