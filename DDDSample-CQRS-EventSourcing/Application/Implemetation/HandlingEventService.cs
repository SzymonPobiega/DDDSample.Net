using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Location;

namespace DDDSample.Application.Implemetation
{
   /// <summary>
   /// Handling event registration service.
   /// </summary>
   public class HandlingEventService : IHandlingEventService
   {
      private readonly ICargoRepository _cargoRepository;
      private readonly ILocationRepository _locationRepository;

      public HandlingEventService(ILocationRepository locationRepository, ICargoRepository cargoRepository)
      {
         _locationRepository = locationRepository;
         _cargoRepository = cargoRepository;
      }

      public void RegisterHandlingEvent(DateTime completionTime, TrackingId trackingId, UnLocode unLocode, HandlingEventType type)
      {
         Cargo cargo = _cargoRepository.Find(trackingId);                  
         Location location = _locationRepository.Find(unLocode);
         cargo.RegisterHandlingEvent(type, location, DateTime.Now, completionTime);
      }
   }
}