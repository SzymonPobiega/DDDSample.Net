using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using DDDSample.Messages;
using NServiceBus;

namespace DDDSample.Domain.EventHandlers
{
   /// <summary>
   /// Handles <see cref="CargoWasAssignedToRouteEvent"/> by publishing corresponding message on the bus.
   /// </summary>
   public class CargoWasAssignedToRouteEventHandler : IEventHandler<CargoWasAssignedToRouteEvent>
   {
      private readonly IBus _bus;

      public CargoWasAssignedToRouteEventHandler(IBus bus)
      {
         _bus = bus;
      }

      public void Handle(CargoWasAssignedToRouteEvent @event)
      {
         _bus.Publish(new CargoAssignedToRouteMessage
                         {
                            TrackingId = @event.Cargo.TrackingId.IdString,
                            Legs = @event.NewItinerary.Legs.Select(x => ConvertLegToDto(x)).ToList()
                         });
      }

      private static LegDTO ConvertLegToDto(Leg x)
      {
         return new LegDTO
                   {
                      LoadDate = x.LoadDate,
                      LoadLocation = x.LoadLocation.Name,
                      UnloadDate = x.UnloadDate,
                      UnloadLocation = x.UnloadLocation.Name
                   };
      }
   }
}