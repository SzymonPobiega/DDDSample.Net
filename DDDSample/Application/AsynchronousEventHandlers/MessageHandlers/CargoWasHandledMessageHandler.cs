using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Application.AsynchronousEventHandlers.Messages;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using DDDSample.Domain.Location;
using NHibernate;
using HandlingEvent=DDDSample.Domain.Cargo.HandlingEvent;

namespace DDDSample.Application.AsynchronousEventHandlers.MessageHandlers
{
   /// <summary>
   /// Handlers <see cref="CargoWasHandledMessage"/> and initiates processing in the <see cref="Cargo"/>
   /// agrregate.
   /// </summary>
   public class CargoWasHandledMessageHandler : AbstractMessageHandler<CargoWasHandledMessage>
   {
      private readonly ILocationRepository _locationRepository;
      private readonly ICargoRepository _cargoRepository;

      public CargoWasHandledMessageHandler(ICargoRepository cargoRepository, ISessionFactory sessionFactory, ILocationRepository locationRepository) : base(sessionFactory)
      {
         _cargoRepository = cargoRepository;
         _locationRepository = locationRepository;
      }

      protected override void DoHandle(CargoWasHandledMessage message)
      {
         Location location = _locationRepository.Find(new UnLocode(message.Location));
         Cargo cargo = _cargoRepository.Find(new TrackingId(message.TrackingId));

         cargo.DeriveDeliveryProgress(new HandlingEvent(message.EventType, location, message.RegistrationDate, message.CompletionDate));
      }
   }
}
