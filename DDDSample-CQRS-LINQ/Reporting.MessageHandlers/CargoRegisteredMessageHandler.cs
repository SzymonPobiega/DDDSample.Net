using System;
using System.Linq;
using System.Collections.Generic;
using DDDSample.Messages;

namespace DDDSample.Reporting.MessageHandlers
{
   public class CargoRegisteredMessageHandler : AbstractMessageHandler<CargoRegisteredMessage>
   {
      protected override void DoHandle(CargoRegisteredMessage message)
      {
         Cargo cargo = new Cargo(message.TrackingId, message.Origin, message.Destination, message.ArrivalDeadline);
         ReportingDataContext context = new ReportingDataContext();
         context.Cargos.InsertOnSubmit(cargo);

         context.SubmitChanges();
      }
   }
}