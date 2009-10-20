using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;

namespace DDDSample.Application.SynchronousEventHandlers
{
   /// <summary>
   /// Handles <see cref="CargoWasHandledEvent"/> and synchronizes <see cref="Cargo"/> aggregate
   /// according to up-to-date handling history information.
   /// </summary>
   public class CargoWasHandledEventHandler : IEventHandler<CargoWasHandledEvent>
   {
      private readonly IHandlingEventRepository _handlingEventRepository;

      public void Handle(CargoWasHandledEvent @event)
      {                  
         HandlingHistory handlingHistory = _handlingEventRepository.LookupHandlingHistoryOfCargo(@event.Data.Cargo.TrackingId);
         @event.Data.Cargo.DeriveDeliveryProgress(handlingHistory);
      }
   }
}
