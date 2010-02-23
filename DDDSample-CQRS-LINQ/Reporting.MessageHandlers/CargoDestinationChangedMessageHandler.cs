using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Messages;

namespace DDDSample.Reporting.MessageHandlers
{
   public class CargoDestinationChangedMessageHandler : AbstractMessageHandler<CargoDestinationChangedMessage>
   {
      protected override void DoHandle(CargoDestinationChangedMessage message)
      {
         ReportingDataContext context = new ReportingDataContext();
         Cargo cargo = context.Cargos.First(x => x.TrackingId == message.TrackingId);
         cargo.UpdateRouteSpecification(message.Origin, message.Destination, message.ArrivalDeadline);
         
         context.SubmitChanges();
      }
   }
}