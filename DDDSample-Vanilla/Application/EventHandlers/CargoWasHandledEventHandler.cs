using DDDSample.DomainModel;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Operations.Handling;

namespace DDDSample.Application.EventHandlers
{
    /// <summary>
    /// Handles <see cref="CargoWasHandledEvent"/> and synchronizes <see cref="Cargo"/> aggregate
    /// according to up-to-date handling history information.
    /// </summary>
    public class CargoWasHandledEventHandler : IEventHandler<HandlingEvent>
    {
        private readonly ICargoRepository _cargoRepository;

        public CargoWasHandledEventHandler(ICargoRepository cargoRepository)
        {
            _cargoRepository = cargoRepository;
        }

        public void Handle(HandlingEvent @event)
        {
            Cargo cargo = _cargoRepository.Find(@event.Cargo.TrackingId);

            cargo.DeriveDeliveryProgress(@event);
        }
    }
}
