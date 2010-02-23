using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Messages;

namespace DDDSample.Reporting.MessageHandlers
{
   /// <summary>
   /// Handles <see cref="CargoAssignedToRouteMessage"/> and ensures that proper handling history
   /// obejct has been created.
   /// </summary>
   public class CargoAssignedToRouteMessageHandler : AbstractMessageHandler<CargoAssignedToRouteMessage>
   {
      protected override void DoHandle(CargoAssignedToRouteMessage message)
      {
         ReportingDataContext context = new ReportingDataContext();
         Cargo cargo = context.Cargos.First(x => x.TrackingId == message.TrackingId);
         cargo.RouteSpecification = message.Legs;         

         context.SubmitChanges();
      }
   }
}