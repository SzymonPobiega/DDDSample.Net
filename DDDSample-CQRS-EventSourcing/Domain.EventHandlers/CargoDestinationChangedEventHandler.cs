using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using DDDSample.Messages;
using NServiceBus;

namespace DDDSample.Domain.EventHandlers
{
   /// <summary>
   /// Handles <see cref="CargoDestinationChangedEvent"/> by publishing corresponding message on the bus.
   /// </summary>
   public class CargoDestinationChangedEventHandler : IEventHandler<CargoDestinationChangedEvent>
   {
      private readonly IBus _bus;

      public CargoDestinationChangedEventHandler(IBus bus)
      {
         _bus = bus;
      }

      public void Handle(CargoDestinationChangedEvent @event)
      {
         _bus.Publish(new CargoDestinationChangedMessage
                         {
                            TrackingId = @event.Cargo.TrackingId.IdString,
                            Origin = @event.NewSpecification.Origin.Name,
                            Destination = @event.NewSpecification.Destination.Name,
                            ArrivalDeadline = @event.NewSpecification.ArrivalDeadline
                         });
      }
   }
}