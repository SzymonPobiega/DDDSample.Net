using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Application.AsynchronousEventHandlers.Messages;
using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using NServiceBus;

namespace DDDSample.Application.AsynchronousEventHandlers.EventHandlers
{
    /// <summary>
    /// Handles <see cref="CargoWasHandledEvent"/> and synchronizes <see cref="Cargo"/> aggregate
    /// according to up-to-date handling history information.
    /// </summary>
    public class CargoWasHandledEventHandler : IEventHandler<CargoWasHandledEvent>
    {
        private readonly IBus _bus;

        public CargoWasHandledEventHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(CargoWasHandledEvent @event)
        {                  
            _bus.Publish(new CargoWasHandledMessage
                            {
                               TrackingId = @event.Source.TrackingId.IdString,
                               EventType = @event.Source.EventType,
                               Location = @event.Source.Location.UnLocode.CodeString,
                               RegistrationDate = @event.Source.RegistrationDate,
                               CompletionDate = @event.Source.CompletionDate
                            });
        }
    }
}