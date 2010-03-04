using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Domain.Cargo;
using DDDSample.Messages;
using NServiceBus;

namespace DDDSample.Domain.EventHandlers
{
   /// <summary>
   /// Handles <see cref="CargoAssignedToRouteEvent"/> by publishing corresponding message on the bus.
   /// </summary>
   public class CargoWasAssignedToRouteEventHandler : IEventHandler<Cargo.Cargo, CargoAssignedToRouteEvent>
   {
      private readonly IBus _bus;

      public CargoWasAssignedToRouteEventHandler(IBus bus)
      {
         _bus = bus;
      }

      public void Handle(Cargo.Cargo source, CargoAssignedToRouteEvent @event)
      {
         _bus.Publish(new CargoAssignedToRouteMessage
                         {
                            CargoId = source.Id,
                            Legs = @event.NewItinerary.Legs.Select(x => ConvertLegToDto(x)).ToList()
                         });
      }

      private static LegDTO ConvertLegToDto(Leg x)
      {
         return new LegDTO
                   {
                      LoadDate = x.LoadDate,
                      LoadLocation = x.LoadLocation.CodeString,
                      UnloadDate = x.UnloadDate,
                      UnloadLocation = x.UnloadLocation.CodeString
                   };
      }
   }
}