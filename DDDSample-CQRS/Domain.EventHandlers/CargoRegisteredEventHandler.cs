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
   public class CargoRegisteredEventHandler : IEventHandler<CargoRegisteredEvent>
   {
      private readonly IBus _bus;

      public CargoRegisteredEventHandler(IBus bus)
      {
         _bus = bus;
      }

      public void Handle(CargoRegisteredEvent @event)
      {
         _bus.Publish(new CargoRegisteredMessage()
                         {
                            TrackingId = @event.Cargo.TrackingId.IdString,
                            Origin = @event.RouteSpecification.Origin.Name,
                            Destination = @event.RouteSpecification.Destination.Name,
                            ArrivalDeadline = @event.RouteSpecification.ArrivalDeadline
                         });
      }
   }
}