using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Application.AsynchronousEventHandlers.Messages;
using DDDSample.Domain.Cargo;
using DDDSample.Domain.Handling;
using NHibernate;
using NServiceBus;

namespace DDDSample.Application.AsynchronousEventHandlers.MessageHandlers
{
   /// <summary>
   /// Handles <see cref="CargoHasBeenAssignedToRouteMessage"/> and ensures that proper handling history
   /// obejct has been created.
   /// </summary>
   public class CargoHasBeenAssignedToRouteMessageHandler : AbstractMessageHandler<CargoHasBeenAssignedToRouteMessage>
   {
      private readonly IHandlingEventRepository _handlingEventRepository;

      public CargoHasBeenAssignedToRouteMessageHandler(IHandlingEventRepository handlingEventRepository, ISessionFactory sessionFactory)
         : base(sessionFactory)
      {
         _handlingEventRepository = handlingEventRepository;
      }

      protected override void DoHandle(CargoHasBeenAssignedToRouteMessage message)
      {
         TrackingId trackingId = new TrackingId(message.TrackingId);

         HandlingHistory existing = _handlingEventRepository.LookupHandlingHistoryOfCargo(trackingId);
         if (existing != null)
         {
            return;
         }
         HandlingHistory handlingHistory = new HandlingHistory(trackingId);
         _handlingEventRepository.Store(handlingHistory);
      }
   }
}
