using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;

namespace DDDSample.Application.EventHandlers
{
    /// <summary>
    /// Handles <see cref="CargoWasHandledEvent"/> and synchronizes <see cref="Cargo"/> aggregate
    /// according to up-to-date handling history information.
    /// </summary>
    public class CargoWasHandledEventHandler : IEventHandler<HandlingEvent>
    {
        public void Handle(HandlingEvent handlingEvent)
        {
            handlingEvent.Cargo.DeriveDeliveryProgress(handlingEvent);
        }
    }
}
