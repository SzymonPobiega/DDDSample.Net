using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDDSample.Domain.Cargo;
using DDDSample.Reporting.Persistence.NHibernate;
using NServiceBus;

namespace DDDSample.Domain.EventHandlers
{
   /// <summary>
   /// Handles <see cref="CargoHandledEvent"/> and synchronizes <see cref="Cargo"/> aggregate
   /// according to up-to-date handling history information.
   /// </summary>
   public class CargoWasHandledEventHandler : IEventHandler<Cargo.Cargo, CargoHandledEvent>
   {
      private readonly CargoDataAccess _cargoDataAccess;

      public CargoWasHandledEventHandler(CargoDataAccess cargoDataAccess)
      {
         _cargoDataAccess = cargoDataAccess;
      }

      public void Handle(Cargo.Cargo source, CargoHandledEvent @event)
      {
         var del = @event.Delivery;
         var lastKnownActivity = new Reporting.HandlingActivity(del.LastEventType.Value, del.LastKnownLocation.CodeString);
         Reporting.HandlingActivity nextExpectedActivity = null;
         if (del.NextExpectedActivity != null)
         {
            nextExpectedActivity = new Reporting.HandlingActivity(del.NextExpectedActivity.EventType, del.NextExpectedActivity.Location.CodeString);
         }
         var cargo = _cargoDataAccess.Find(source.Id);
         cargo.UpdateHistory(
            nextExpectedActivity,
            lastKnownActivity,
            del.RoutingStatus,
            del.TransportStatus,
            del.EstimatedTimeOfArrival,
            del.IsUnloadedAtDestination,
            del.IsMisdirected,
            del.CalculatedAt);          
      }
   }
}