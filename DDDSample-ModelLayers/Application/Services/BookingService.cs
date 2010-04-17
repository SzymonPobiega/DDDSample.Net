using System;
using System.Collections.Generic;
using DDDSample.Domain.Location;
using DDDSample.DomainModel.DecisionSupport.Routing;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Potential.Customer;
using DDDSample.DomainModel.Potential.Location;
using NHibernate;

namespace DDDSample.Application.Services
{
   /// <summary>
   /// Implementation of <see cref="IBookingService"/>.
   /// </summary>
   public class BookingService : IBookingService
   {
      private readonly ILocationRepository _locationRepository;
      private readonly ICargoRepository _cargoRepository;
      private readonly ICustomerRepository _customerRepository;
      private readonly IRoutingService _routingService;      

      public BookingService(ILocationRepository locationRepository, ICargoRepository cargoRepository, IRoutingService routingService, ICustomerRepository customerRepository)
      {
         _locationRepository = locationRepository;
         _customerRepository = customerRepository;
         _cargoRepository = cargoRepository;
         _routingService = routingService;
      }

      public TrackingId BookNewCargo(string customerLogin, UnLocode originUnLocode, UnLocode destinationUnLocode, DateTime arrivalDeadline)
      {
         var origin = _locationRepository.Find(originUnLocode);
         var destination = _locationRepository.Find(destinationUnLocode);
         var customer = _customerRepository.Find(customerLogin);

         var routeSpecification = new RouteSpecification(origin, destination, arrivalDeadline);
         var trackingId = _cargoRepository.NextTrackingId();
         var cargo = new Cargo(trackingId, routeSpecification, customer);

         _cargoRepository.Store(cargo);
         return trackingId;
      }

      public IList<Itinerary> RequestPossibleRoutesForCargo(TrackingId trackingId)
      {
         Cargo cargo = _cargoRepository.Find(trackingId);
         return _routingService.FetchRoutesFor(cargo);
      }

      public void AssignCargoToRoute(TrackingId trackingId, Itinerary itinerary)
      {
         Cargo cargo = _cargoRepository.Find(trackingId);
         cargo.AssignToRoute(itinerary);
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