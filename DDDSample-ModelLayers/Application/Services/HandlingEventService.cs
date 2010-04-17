using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Location;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Operations.Handling;
using DDDSample.DomainModel.Potential.Location;

namespace DDDSample.Application.Services
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