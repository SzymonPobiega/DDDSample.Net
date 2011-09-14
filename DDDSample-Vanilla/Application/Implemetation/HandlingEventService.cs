using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using DDDSample.Domain.Location;

namespace DDDSample.Application.Implemetation
{
   /// <summary>
   /// Handling event registration service.
   /// </summary>
   public class HandlingEventService : IHandlingEventService
   {
      private readonly IHandlingEventRepository _handlingEventRepository;
      private readonly ILocationRepository _locationRepository;
      private readonly ICargoRepository _cargoRepository;
       private readonly IEventPublisher _eventPublisher;

       public HandlingEventService(IHandlingEventRepository handlingEventRepository, ILocationRepository locationRepository, ICargoRepository cargoRepository, IEventPublisher eventPublisher)
      {
         _handlingEventRepository = handlingEventRepository;
         _cargoRepository = cargoRepository;
          _eventPublisher = eventPublisher;
          _locationRepository = locationRepository;
      }

      public void RegisterHandlingEvent(DateTime completionTime, TrackingId trackingId, UnLocode unLocode, HandlingEventType type)
      {
         var cargo = _cargoRepository.Find(trackingId);
         var location = _locationRepository.Find(unLocode);
         var @event = new HandlingEvent(type, location, DateTime.Now, completionTime, cargo, _eventPublisher);
         _handlingEventRepository.Store(@event);
      }
   }
}