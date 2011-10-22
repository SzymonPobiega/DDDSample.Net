using DDDSample.Domain;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using DDDSample.DomainModel;

namespace DDDSample.Application.EventHandlers
{
   /// <summary>
   /// Handles <see cref="CargoWasHandledEvent"/> and synchronizes <see cref="Cargo"/> aggregate
   /// according to up-to-date handling history information.
   /// </summary>
   public class CargoWasHandledEventHandler : IEventHandler<CargoWasHandledEvent>
   {
      private readonly ICargoRepository _cargoRepository;
       private readonly IEventPublisher _eventPublisher;

       public CargoWasHandledEventHandler(ICargoRepository cargoRepository, IEventPublisher eventPublisher)
      {
          _cargoRepository = cargoRepository;
          _eventPublisher = eventPublisher;
      }

       public void Handle(CargoWasHandledEvent @event)
      {
         Cargo cargo = _cargoRepository.Find(@event.Source.Cargo.TrackingId);                  
         
         cargo.DeriveDeliveryProgress(@event.Source, _eventPublisher);
      }
   }
}
