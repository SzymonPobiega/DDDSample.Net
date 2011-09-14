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
       private readonly IEventPublisher _eventPublisher;

       public BookingService(ILocationRepository locationRepository, ICargoRepository cargoRepository, IRoutingService routingService, IEventPublisher eventPublisher)
      {
         _locationRepository = locationRepository;
         _cargoRepository = cargoRepository;
         _routingService = routingService;
          _eventPublisher = eventPublisher;
      }

      public TrackingId BookNewCargo(UnLocode originUnLocode, UnLocode destinationUnLocode, DateTime arrivalDeadline)
      {
         Location origin = _locationRepository.Find(originUnLocode);
         Location destination = _locationRepository.Find(destinationUnLocode);

         var routeSpecification = new RouteSpecification(origin, destination, arrivalDeadline);
         var trackingId = _cargoRepository.NextTrackingId();
         var cargo = new Cargo(trackingId, routeSpecification);

         _cargoRepository.Store(cargo);
         return trackingId;
      }

      public IList<Itinerary> RequestPossibleRoutesForCargo(TrackingId trackingId)
      {
         Cargo cargo = _cargoRepository.Find(trackingId);
         return _routingService.FetchRoutesForSpecification(cargo.RouteSpecification);
      }

      public void AssignCargoToRoute(TrackingId trackingId, Itinerary itinerary)
      {
         Cargo cargo = _cargoRepository.Find(trackingId);
         cargo.AssignToRoute(itinerary, _eventPublisher);
      }

      public void ChangeDestination(TrackingId trackingId, UnLocode destinationUnLocode)
      {
         Cargo cargo = _cargoRepository.Find(trackingId);
         Location destination = _locationRepository.Find(destinationUnLocode);

         var routeSpecification = new RouteSpecification(cargo.RouteSpecification.Origin, destination, cargo.RouteSpecification.ArrivalDeadline);
         cargo.SpecifyNewRoute(routeSpecification);
      }
   }
}