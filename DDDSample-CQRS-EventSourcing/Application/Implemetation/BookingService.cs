using System;
using System.Collections.Generic;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;
using DDDSample.Reporting.Persistence.NHibernate;
using NHibernate;

namespace DDDSample.Application.Implemetation
{
   /// <summary>
   /// Implementation of <see cref="IBookingService"/>.
   /// </summary>
   public class BookingService : IBookingService
   {
      private readonly ICargoRepository _cargoRepository;
      private readonly CargoDataAccess _cargoDataAccess;
      private readonly IRoutingService _routingService;

      public BookingService(ICargoRepository cargoRepository, IRoutingService routingService, CargoDataAccess cargoDataAccess)
      {
         _cargoRepository = cargoRepository;
         _routingService = routingService;
         _cargoDataAccess = cargoDataAccess;
      }

      public Guid BookNewCargo(UnLocode originUnLocode, UnLocode destinationUnLocode, DateTime arrivalDeadline, out TrackingId trackingId)
      {
         RouteSpecification routeSpecification = new RouteSpecification(originUnLocode, destinationUnLocode, arrivalDeadline);
         trackingId = _cargoRepository.NextTrackingId();
         Cargo cargo = new Cargo(trackingId, routeSpecification);

         _cargoRepository.Store(cargo);
         return cargo.Id;
      }

      public IList<Itinerary> RequestPossibleRoutesForCargo(Guid cargoId)
      {
         Cargo cargo = _cargoRepository.Find(cargoId);

         return cargo.RequestPossibleRoutes(_routingService);
      }

      public void AssignCargoToRoute(Guid cargoId, Itinerary itinerary)
      {
         Cargo cargo = _cargoRepository.Find(cargoId);
         cargo.AssignToRoute(itinerary);
      }

      public void ChangeDestination(Guid cargoId, UnLocode destinationUnLocode)
      {         
         Cargo cargo = _cargoRepository.Find(cargoId);
         cargo.SpecifyNewRoute(destinationUnLocode);
      }
   }
}