using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain;
using DDDSample.DomainModel;
using DDDSample.DomainModel.Operations.Cargo;
using DDDSample.DomainModel.Operations.Handling;
using HandlingEvent=DDDSample.DomainModel.Operations.Cargo.HandlingEvent;

namespace DDDSample.Application.EventHandlers
{
   /// <summary>
   /// Handles <see cref="CargoWasHandledEvent"/> and synchronizes <see cref="Cargo"/> aggregate
   /// according to up-to-date handling history information.
   /// </summary>
   public class CargoWasHandledEventHandler : IEventHandler<CargoWasHandledEvent>
   {
      private readonly ICargoRepository _cargoRepository;

      public CargoWasHandledEventHandler(ICargoRepository cargoRepository)
      {
         _cargoRepository = cargoRepository;
      }

      public void Handle(CargoWasHandledEvent @event)
      {
         Cargo cargo = _cargoRepository.Find(@event.Source.TrackingId);                  
         
         cargo.DeriveDeliveryProgress(new HandlingEvent(@event.Source.EventType, 
                                                        @event.Source.Location, 
                                                        @event.Source.RegistrationDate, 
                                                        @event.Source.CompletionDate));
      }
   }
}