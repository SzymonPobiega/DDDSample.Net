using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Messages;

namespace DDDSample.Reporting.MessageHandlers
{
   /// <summary>
   /// Handlers <see cref="CargoHandledMessage"/> and initiates processing in the <see cref="Cargo"/>
   /// agrregate.
   /// </summary>
   public class CargoHandledMessageHandler : AbstractMessageHandler<CargoHandledMessage>
   {
      protected override void DoHandle(CargoHandledMessage message)
      {
         HandlingEventType lastKnownEvent = (HandlingEventType)message.LastKnownEventType;
         HandlingEventType? nextExpectedEvent = null;
         if (message.NextExpectedEventType != null)
         {
            nextExpectedEvent = (HandlingEventType) message.NextExpectedEventType;
         }         
         ReportingDataContext context = new ReportingDataContext();
         Cargo cargo = context.Cargos.First(x => x.TrackingId == message.TrackingId);
         cargo.UpdateHistory(
            nextExpectedEvent,
            message.NextExpectedLocation,
            lastKnownEvent,
            message.LastKnownLocation,
            (RoutingStatus)message.RoutingStatus,
            (TransportStatus)message.TransportStatus,
            message.EstimatedTimeOfArrival,
            message.IsUnloaded,
            message.IsMisdirected,
            message.CalculatedAt);  
         
         context.SubmitChanges();
      }
   }
}