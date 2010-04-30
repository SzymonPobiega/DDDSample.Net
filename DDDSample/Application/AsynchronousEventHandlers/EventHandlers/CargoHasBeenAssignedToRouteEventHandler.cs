using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Application.AsynchronousEventHandlers.Messages;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using NServiceBus;

namespace DDDSample.Application.AsynchronousEventHandlers.EventHandlers
{
    /// <summary>
    /// Handles <see cref="CargoHasBeenAssignedToRouteEvent"/> and creates corresponding handling history event.
    /// </summary>
    public class CargoHasBeenAssignedToRouteEventHandler : IEventHandler<CargoHasBeenAssignedToRouteEvent>
    {
        private readonly IBus _bus;

        public CargoHasBeenAssignedToRouteEventHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(CargoHasBeenAssignedToRouteEvent @event)
        {
            _bus.Publish(new CargoHasBeenAssignedToRouteMessage{TrackingId = @event.Source.TrackingId.IdString});
        }
    }
}