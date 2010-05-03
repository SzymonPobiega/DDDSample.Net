using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Application.AsynchronousEventHandlers.Messages;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using DDDSample.Domain.Location;
using NHibernate;

namespace DDDSample.Application.AsynchronousEventHandlers.MessageHandlers
{
   /// <summary>
   /// Handlers <see cref="CargoWasHandledMessage"/> and initiates processing in the <see cref="Cargo"/>
   /// agrregate.
   /// </summary>
   public class CargoWasHandledMessageHandler : AbstractMessageHandler<CargoWasHandledMessage>
   {
      private readonly IHandlingEventRepository _handlingEventRepository;

      public CargoWasHandledMessageHandler(ISessionFactory sessionFactory, IHandlingEventRepository handlingEventRepository) : base(sessionFactory)
      {
         _handlingEventRepository = handlingEventRepository;
      }

      protected override void DoHandle(CargoWasHandledMessage message)
      {
         var @event = _handlingEventRepository.Find(message.EventUniqueId);
         var cargo = @event.Cargo;

         cargo.DeriveDeliveryProgress(@event);
      }
   }
}
