using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using DDDSample.Messages;
using NServiceBus;

namespace DDDSample.Domain.EventHandlers
{
   /// <summary>
   /// Handles <see cref="CargoRegisteredEvent"/> by publishing corresponding message on the bus.
   /// </summary>
   public class CargoRegisteredEventHandler : IEventHandler<Cargo.Cargo, CargoRegisteredEvent>
   {
      private readonly IBus _bus;

      public CargoRegisteredEventHandler(IBus bus)
      {
         _bus = bus;
      }

      public void Handle(Cargo.Cargo source, CargoRegisteredEvent @event)
      {
         _bus.Publish(new CargoRegisteredMessage
                         {
                            CargoId = source.Id,
                            TrackingId = @event.TrackingId.IdString,
                            Origin = @event.RouteSpecification.Origin.CodeString,
                            Destination = @event.RouteSpecification.Destination.CodeString,
                            ArrivalDeadline = @event.RouteSpecification.ArrivalDeadline
                         });
      }
   }
}