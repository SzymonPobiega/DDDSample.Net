using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain;
using DDDSample.DomainModel;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Operations.Handling;

namespace DDDSample.Application.EventHandlers
{
   /// <summary>
   /// Handles <see cref="CargoHasBeenAssignedToRouteEvent"/> and creates corresponding handling history event.
   /// </summary>
   public class CargoHasBeenAssignedToRouteEventHandler : IEventHandler<CargoHasBeenAssignedToRouteEvent>
   {
      private readonly IHandlingEventRepository _repository;

      public CargoHasBeenAssignedToRouteEventHandler(IHandlingEventRepository repository)
      {
         _repository = repository;
      }

      public void Handle(CargoHasBeenAssignedToRouteEvent @event)
      {
         HandlingHistory existing = _repository.LookupHandlingHistoryOfCargo(@event.Source.TrackingId);
         if (existing != null)
         {
            return;
         }
         HandlingHistory handlingHistory = new HandlingHistory(@event.Source.TrackingId);
         _repository.Store(handlingHistory);
      }
   }
}