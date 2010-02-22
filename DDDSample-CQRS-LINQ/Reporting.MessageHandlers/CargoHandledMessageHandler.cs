using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Application.AsynchronousEventHandlers.MessageHandlers;
using DDDSample.Messages;
using DDDSample.Reporting.Persistence.NHibernate;
using NHibernate;

namespace DDDSample.Reporting.MessageHandlers
{
   /// <summary>
   /// Handlers <see cref="CargoHandledMessage"/> and initiates processing in the <see cref="Cargo"/>
   /// agrregate.
   /// </summary>
   public class CargoHandledMessageHandler : AbstractMessageHandler<CargoHandledMessage>
   {
      private readonly CargoDataAccess _cargoDataAccess;

      public CargoHandledMessageHandler(CargoDataAccess cargoDataAccess, ISessionFactory sessionFactory) : base(sessionFactory)
      {
         _cargoDataAccess = cargoDataAccess;
      }

      protected override void DoHandle(CargoHandledMessage message)
      {         
         HandlingActivity lastKnownActivity = new HandlingActivity((HandlingEventType)message.LastKnownEventType, message.LastKnownLocation);
         HandlingActivity nextExpectedActivity = null;
         if (message.NextExpectedEventType.HasValue && message.NextExpectedLocation != null)
         {
            nextExpectedActivity = new HandlingActivity((HandlingEventType)message.NextExpectedEventType, message.NextExpectedLocation);
         }         
         Cargo cargo = _cargoDataAccess.Find(message.TrackingId);
         cargo.UpdateHistory(
            nextExpectedActivity, 
            lastKnownActivity,
            (RoutingStatus)message.RoutingStatus,
            (TransportStatus)message.TransportStatus,
            message.EstimatedTimeOfArrival,
            message.IsUnloaded,
            message.IsMisdirected,
            message.CalculatedAt);            
      }
   }
}