using System;
using System.Linq;
using System.Collections.Generic;
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

      public HandlingEventService(IHandlingEventRepository handlingEventRepository, ILocationRepository locationRepository)
      {
         _handlingEventRepository = handlingEventRepository;
         _locationRepository = locationRepository;
      }

      public void RegisterHandlingEvent(DateTime completionTime, TrackingId trackingId, UnLocode unLocode, HandlingEventType type)
      {
         HandlingHistory handlingHistory = _handlingEventRepository.LookupHandlingHistoryOfCargo(trackingId);
         Location location = _locationRepository.Find(unLocode);
         handlingHistory.RegisterHandlingEvent(type, location, DateTime.Now, completionTime);         
      }
   }
}