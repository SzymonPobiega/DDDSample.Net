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
   public class CargoDestinationChangedEventHandler : IEventHandler<Cargo.Cargo, CargoDestinationChangedEvent>
   {
      private readonly IBus _bus;

      public CargoDestinationChangedEventHandler(IBus bus)
      {
         _bus = bus;
      }

      public void Handle(Cargo.Cargo source, CargoDestinationChangedEvent @event)
      {
         _bus.Publish(new CargoDestinationChangedMessage
                         {
                            CargoId = source.Id,
                            Origin = @event.NewSpecification.Origin.CodeString,
                            Destination = @event.NewSpecification.Destination.CodeString,
                            ArrivalDeadline = @event.NewSpecification.ArrivalDeadline
                         });
      }
   }
}