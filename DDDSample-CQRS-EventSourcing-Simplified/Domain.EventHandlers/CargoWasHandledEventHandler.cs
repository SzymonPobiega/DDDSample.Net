using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Cargo;
using DDDSample.Messages;
using NServiceBus;

namespace DDDSample.Domain.EventHandlers
{
   /// <summary>
   /// Handles <see cref="CargoHandledEvent"/> and synchronizes <see cref="Cargo"/> aggregate
   /// according to up-to-date handling history information.
   /// </summary>
   public class CargoWasHandledEventHandler : IEventHandler<Cargo.Cargo, CargoHandledEvent>
   {
      private readonly IBus _bus;

      public CargoWasHandledEventHandler(IBus bus)
      {
         _bus = bus;
      }

      public void Handle(Cargo.Cargo source, CargoHandledEvent @event)
      {
         HandlingActivity nextExpectedActivity = @event.Delivery.NextExpectedActivity;
         _bus.Publish(new CargoHandledMessage
                         {
                            CargoId = source.Id,
                            CalculatedAt = @event.Delivery.CalculatedAt,
                            EstimatedTimeOfArrival = @event.Delivery.EstimatedTimeOfArrival,
                            IsMisdirected = @event.Delivery.IsMisdirected,
                            IsUnloaded = @event.Delivery.IsUnloadedAtDestination,
                            RoutingStatus = (int) @event.Delivery.RoutingStatus,
                            TransportStatus = (int) @event.Delivery.TransportStatus,
                            NextExpectedEventType =
                               nextExpectedActivity != null ? (int?) nextExpectedActivity.EventType : null,
                            NextExpectedLocation =
                               nextExpectedActivity != null ? nextExpectedActivity.Location.CodeString : null,
                            LastKnownEventType = (int)@event.Delivery.LastEventType,
                            LastKnownLocation = @event.Delivery.LastKnownLocation.CodeString
                         });
      }
   }
}