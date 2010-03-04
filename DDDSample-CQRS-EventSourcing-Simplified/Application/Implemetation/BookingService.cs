using System;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;

namespace DDDSample.Application.Implemetation
{
   /// <summary>
   /// Implementation of <see cref="IBookingService"/>.
   /// </summary>
   public class BookingService : IBookingService
   {
      private readonly ICargoRepository _cargoRepository;

      public BookingService(ICargoRepository cargoRepository)
      {
         _cargoRepository = cargoRepository;
      }

      public Guid BookNewCargo(UnLocode originUnLocode, UnLocode destinationUnLocode, DateTime arrivalDeadline, out TrackingId trackingId)
      {
         RouteSpecification routeSpecification = new RouteSpecification(originUnLocode, destinationUnLocode, arrivalDeadline);
         trackingId = _cargoRepository.NextTrackingId();
         Cargo cargo = new Cargo(trackingId, routeSpecification);

         _cargoRepository.Store(cargo);
         return cargo.Id;
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